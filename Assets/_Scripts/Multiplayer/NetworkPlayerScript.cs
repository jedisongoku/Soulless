using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class NetworkPlayerScript : MonoBehaviour {

    public bool battleArena = false;
    private PhotonView photonView;
    private Animator anim;

    private Vector3 playerPosition = Vector3.zero;
    private Quaternion playerRotation = Quaternion.identity;
    //private int playerHealth;


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
            //new name change
            //gameObject.name = photonView.ownerId.ToString();
        }
        else
        {
          
            if (battleArena)//SceneManager.GetActiveScene().name == "BattleArena")
            {
                gameObject.tag = "Enemy";
                //set player's layer to default so you can click on them
                gameObject.layer = LayerMask.NameToLayer("Enemy");
                gameObject.name = "Network Enemy";
                //gameObject.name = photonView.ownerId.ToString();
            }
            else
            {
                gameObject.tag = "Player";
                gameObject.name = "Network player";
                //gameObject.name = photonView.ownerId.ToString();
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
    [PunRPC]
    void SendTrigger(int sentId, string triggerName)
    {
        //Debug.Log("I received an animation trigger for " + sentId + " and my id is " + photonView.viewID);
        if (photonView.viewID == sentId)
        {
           anim.SetTrigger(triggerName);
        }
    }

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
    void OnPhotonPlayerConnected(PhotonPlayer connected)
    {
        Debug.Log("OnPhotonPlayerConnected position rpc");
        photonView.RPC("SendPosition", PhotonTargets.Others, photonView.viewID, gameObject.transform.position);
    }

    [PunRPC]
    void SendPosition(int viewID, Vector3 position)
    {
        if(photonView.viewID == viewID)
        {

            gameObject.transform.position = position;
        }
        
    }

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

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //We own this player: send the others our data
            //stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            //send all animator variables
            if (anim != null)
            {
                //stream.SendNext(anim.GetFloat("MOVE"));
                //stream.SendNext(anim.GetBool("INCOMBAT"));
                //stream.SendNext(anim.GetBool("Attack"));
            }
        }
        else
        {
            //Network player, receive data
            //playerPosition = (Vector3)stream.ReceiveNext();
            playerRotation = (Quaternion)stream.ReceiveNext();
            //receive animator variables from other player
            if (anim != null)
            {
                //anim.SetFloat("MOVE", (float)stream.ReceiveNext());
                //anim.SetBool("INCOMBAT", (bool)stream.ReceiveNext());
                //anim.SetBool("Attack", (bool)stream.ReceiveNext());
            }

        }
    }


}
