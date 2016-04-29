using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

    public float smoothing = 5f;
    public Vector3 offset = new Vector3(0.1f, 13.3f, -31.1f);

    private Transform player;
    //taken from original calculations, can be modified if necessary
    
    private bool following = false;

    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
           
            player = GameObject.FindGameObjectWithTag("Player").transform;
            transform.position = player.position + offset;
            following = true;
        }
    }


    void FixedUpdate()
    {
        if(GameObject.FindGameObjectWithTag("Player") != null && following == false)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            transform.position = player.position + offset;
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
                Vector3 targetCamPos = player.position + offset;
                transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
            }
        }
    }

}