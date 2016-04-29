using UnityEngine;
using System.Collections;

public class MouseProjection : MonoBehaviour
{
    RaycastHit hit;
    private float raycastLength = 500;
    private static Vector3 mouseClickPoint;
    float projectionTime = 3f;
    
    
  



    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.name == "Terrain")
            {
                if (Input.GetMouseButtonDown(0))    //click left mouse button
                {
                      //transform.FindChild("MProjection").gameObject.SetActive(true);
                      mouseClickPoint = hit.point;    //gets point from mouse click
                      
                }
            }
           
            transform.position = mouseClickPoint + (transform.forward * -10); //adds distance to z direction so projector isn't in the ground
        }

       /* projectionTime -= Time.deltaTime;
        if ((projectionTime == 0 || projectionTime < 0))
        {
            transform.FindChild("MProjection").gameObject.SetActive(false);
            projectionTime = 3f;
        }
        */

        Debug.DrawRay(ray.origin, ray.direction * raycastLength, Color.green);
    }
}
