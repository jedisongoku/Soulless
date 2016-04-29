using UnityEngine;
using System.Collections;

public class PortraitCameraFollow : MonoBehaviour
{

    private Transform player;
    //offset from transform.position for each npc
    public Vector3 faceLocation;
    //how close to set camera to npc's face
    public float faceDistance;

    private bool following = false;

    void Start()
    {
        /////////////////vector 3 back is going to side, maybe a different way to position camera.
        /////////////////also, player doesnt show up for ignore raycast layer
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {

            player = GameObject.FindGameObjectWithTag("Player").transform;
            transform.position = player.position + faceLocation + (-transform.forward * faceDistance);
            transform.rotation = Quaternion.Euler(0, player.transform.rotation.eulerAngles.y + 180, 0);
            following = true;
        }
    }


    void FixedUpdate()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null && following == false)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            transform.position = player.position + faceLocation + (-transform.forward * faceDistance);

            transform.rotation = Quaternion.Euler(0, player.transform.rotation.eulerAngles.y + 180,0);
            following = true;
        }
        if (following == true)
        {
            //if player disconnects
            if (player == null)
            {
                following = false;
            }
            else
            {
                transform.position = player.position + faceLocation + (-transform.forward * faceDistance);
                transform.rotation = Quaternion.Euler(0, player.transform.rotation.eulerAngles.y + 180, 0);


            }
        }
    }
}
