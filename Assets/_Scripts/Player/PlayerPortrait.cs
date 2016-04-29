using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerPortrait : MonoBehaviour
{

    public Camera portraitCamera;
    private Transform player;
    //offset from transform.position for each npc
    public Vector3 faceLocation;
    //how close to set camera to npc's face
    public float faceDistance;

    private bool following = false;

    Texture2D RTImage(Camera cam)
    {
        cam.targetTexture = RenderTexture.GetTemporary(128, 128, 16);
        cam.Render();
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = cam.targetTexture;
        
        Texture2D image = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = currentRT;
        Debug.Log("Sent Image to Portrait");
        return image;
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Debug.Log("Starting portrait for: "+ player);
        UpdatePortrait();
        //SetCameraPosition();
        //this.GetComponent<RawImage>().texture = RTImage(portraitCamera);
    }
   void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.P))
        {
            UpdatePortrait();
        }
        */
    }
    public void UpdatePortrait()
    {
        SetCameraPosition();
        //this.GetComponent<RawImage>().texture = RTImage(portraitCamera);
    }
    void SetCameraPosition()
    {
        if (player != null)
        {

            //player = GameObject.FindGameObjectWithTag("Player").transform;
            portraitCamera.transform.position = player.position + faceLocation + (-portraitCamera.transform.forward * faceDistance);
            portraitCamera.transform.rotation = Quaternion.Euler(0, player.transform.rotation.eulerAngles.y + 180, 0);
            Debug.Log("Camera Position is: "+ portraitCamera.transform.position);
            this.GetComponent<RawImage>().texture = RTImage(portraitCamera);

        }
    }

}
