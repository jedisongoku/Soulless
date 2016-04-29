using UnityEngine;
using System.Collections;

public class MiniMapCameraFollow : MonoBehaviour {

    //public float smoothing = 5f;
    //public Vector3 offset = new Vector3(0f, 100f, 0f);
    public float cameraHeight = 100f;
    private float maxHeight = 100f;
    private float minHeight = 20f;
    private float zoomAmount = 10f;

    private Transform player;

    private RectTransform playerPin;
    //taken from original calculations, can be modified if necessary

    private bool following = false;

    void Start()
    {
        playerPin = GameObject.Find("Unit Pin (Player)").GetComponent<RectTransform>();
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {

            player = GameObject.FindGameObjectWithTag("Player").transform;
            transform.position = new Vector3(player.position.x,cameraHeight, player.position.z);
            following = true;
        }
    }


    void FixedUpdate()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null && following == false)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            transform.position = new Vector3(player.position.x, cameraHeight, player.position.z);
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
                transform.position = new Vector3(player.position.x, cameraHeight, player.position.z);
                //set pin rotation
                //store the player's y rotation
                float playerRot = -player.eulerAngles.y;
                //create a new rotation that will change the z value by the amount of the player's y rotation
                Quaternion newRot = Quaternion.Euler(0, 0, playerRot);
                //lerp the rotation of the player pin from its original rotation to the new rotation at speed 4
                playerPin.rotation = Quaternion.Lerp(playerPin.rotation, newRot, Time.deltaTime*4);

            }
        }
    }

    public void ZoomIn()
    {
        if(cameraHeight > minHeight)
        {
            cameraHeight -= zoomAmount;
        }
        else
        {
            cameraHeight = minHeight;
        }
    }
    public void ZoomOut()
    {
        if (cameraHeight < maxHeight)
        {
            cameraHeight += zoomAmount;
        }
        else
        {
            cameraHeight = maxHeight;
        }
    }
}
