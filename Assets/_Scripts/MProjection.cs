using UnityEngine;
using System.Collections;

public class MProjection : MonoBehaviour 
{

    RaycastHit hit;
    private float raycastLength = 500;
    private static Vector3 mouseClickPoint;
  




    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.name == "Terrain")
            {
                if (Input.GetMouseButtonDown(0))    //click left mouse button
                {
                    mouseClickPoint = hit.point;    //gets point from mouse click
                }
            }

            transform.position = mouseClickPoint + (transform.forward * -10); //adds distance to z direction so projector isn't in the ground
        }
        Debug.DrawRay(ray.origin, ray.direction * raycastLength, Color.green);
    }
}
