using UnityEngine;
using System.Collections;

public class NetworkParticleScript : MonoBehaviour {

    private Vector3 particlePosition = Vector3.zero;
    private Quaternion particleRotation = Quaternion.identity;
    private PhotonView photonView;

    void Start()
    {
        photonView = gameObject.GetComponent<PhotonView>();
    }

    void Update()
    {
        if (photonView.isMine)
        {
            //do nothing, character is being controlled by player
        }
        else
        {
            //prevent syncing on entrance to room
            if (particlePosition != Vector3.zero)
            {
                transform.position = Vector3.Lerp(transform.position, particlePosition, 0.1f);
                transform.rotation = Quaternion.Lerp(transform.rotation, particleRotation, 0.1f);
            }
        }

    }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            //Network player, receive data
            particlePosition = (Vector3)stream.ReceiveNext();
            particleRotation = (Quaternion)stream.ReceiveNext();

        }
    }

    /*[PunRPC]
    void InstantiateParticleEffects(int sentID, string particleName, Vector3 position, Quaternion rotation, int targetViewID)
    {
        GameObject particle = Instantiate(Resources.Load(particleName), position, rotation) as GameObject;
        particle.GetComponent<ParticleFollowTarget>().SetTarget(PhotonView.Find(targetViewID).gameObject);
    }*/
}
