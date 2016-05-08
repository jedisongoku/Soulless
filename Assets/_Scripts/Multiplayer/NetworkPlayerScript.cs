using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class NetworkPlayerScript : MonoBehaviour {

    //determines if a player can attack other players
    bool battleArena = false;
    //Reference to player's photon view and animator components
    private PhotonView photonView;
    private Animator anim;

    private Vector3 playerPosition = Vector3.zero;
    private Quaternion playerRotation = Quaternion.identity;
    


    // Use this for initialization
    void Start ()
    {
        photonView = gameObject.GetComponent<PhotonView>();
        //playerHealth = GetComponent<Health>().health;
        anim = GetComponent<Animator>();

        if (anim == null)
        {
            Debug.LogError("This has no animator attached to sync");
        }
       
        //set proper name and tag to distinguish local player from others
        if (photonView.isMine)//isLocalPlayer)
        {
            gameObject.tag = "Player";
            gameObject.name = "LOCAL player";
           
        }
        else
        {
          
            if (battleArena)//SceneManager.GetActiveScene().name == "BattleArena")
            {
                gameObject.tag = "Enemy";
                //set player's layer to default so you can click on them
                gameObject.layer = LayerMask.NameToLayer("Enemy");
                gameObject.name = "Network Enemy";
               
            }
            else
            {
                gameObject.tag = "Player";
                gameObject.name = "Network player";
                
            }
           
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (photonView.isMine)
        {
            //do nothing, character is being controlled by player
        }
        else
        {
            //prevent syncing on entrance to room
            if (playerPosition != Vector3.zero)
            {
                transform.position = Vector3.Lerp(transform.position, playerPosition, 0.1f);
                transform.rotation = Quaternion.Lerp(transform.rotation, playerRotation, 0.1f);
            }
        }

    }
    //RPC call to send animation triggers to other clients
    [PunRPC]
    void SendTrigger(int sentId, string triggerName)
    {
        
        if (photonView.viewID == sentId)
        {
           anim.SetTrigger(triggerName);
        }
    }
    //RPC call to send particle effects to other clients
    [PunRPC]
    void InstantiateParticleEffects(int viewID, string particleName, Vector3 position, Quaternion rotation, int targetViewID, bool isChild)
    {
        if (photonView.viewID == viewID)
        {
            GameObject particle = Instantiate(Resources.Load(particleName), position, rotation) as GameObject;
            if(particle.GetComponent<ParticleFollowTarget>() != null)
            {
                particle.GetComponent<ParticleFollowTarget>().SetTarget(PhotonView.Find(targetViewID).gameObject);
            }
            if(isChild)
            {
                particle.transform.SetParent(gameObject.transform);
            }
            if(particleName == "MeteorFireArea")
            {
                if(photonView.isMine)
                {
                    particle.GetComponent<FireArea>().weaponDamage = PlayFabDataStore.playerWeaponDamage;
                    particle.GetComponent<FireArea>().photonView = photonView;
                    particle.GetComponent<FireArea>().damageType = PlayFabDataStore.catalogRunes["Rune_RainOfFire"].damageType;
                }
                
            }
            
        }

    }

    //sync position when a player joins
    void OnPhotonPlayerConnected(PhotonPlayer connected)
    {
        Debug.Log("OnPhotonPlayerConnected position rpc");
        photonView.RPC("SendPosition", PhotonTargets.Others, photonView.viewID, gameObject.transform.position);
    }
    //RPC call to sync position to other clients
    [PunRPC]
    void SendPosition(int viewID, Vector3 position)
    {

        if(photonView.viewID == viewID)
        {
            GetComponent<NavMeshAgent>().Stop();
            GetComponent<NavMeshAgent>().ResetPath();
            GetComponent<NavMeshAgent>().enabled = false;
            gameObject.transform.position = position;
            GetComponent<NavMeshAgent>().enabled = true;
        }

    }
    //RPC call to send movement destination to other clients
    [PunRPC]
    void SendMoveDestination(int viewID, Vector3 movePosition, float stopDistance)
    {
        if(photonView.viewID == viewID)
        {
            
            GameObject source = PhotonView.Find(viewID).gameObject;
            source.transform.LookAt(movePosition);
            source.GetComponent<NavMeshAgent>().SetDestination(movePosition);
            source.GetComponent<NavMeshAgent>().stoppingDistance = stopDistance;
        }
    }
    //constantly sync rotation of players across clients
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //We own this player: send the others our data
            stream.SendNext(transform.rotation);
        }
        else
        {
            //Network player, receive data          
            playerRotation = (Quaternion)stream.ReceiveNext();
        }
    }


}
