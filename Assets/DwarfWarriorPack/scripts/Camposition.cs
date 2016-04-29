using UnityEngine;
using System.Collections;

public class Camposition : MonoBehaviour {

public GameObject charact;
public GameObject position1;
public GameObject position2;
public GameObject position3;
Vector3 destiny;
Vector3 punto;





	// Use this for initialization
	void Start () {
	destiny = position1.transform.position;
	punto = charact.transform.position;

	
	}
	
	public void ChangePos1()
	{
	
		destiny = position2.transform.position;
		punto = charact.transform.position;
	}
	
	public void ChangePos2()
	{
		
		destiny = position1.transform.position;
		punto = charact.transform.position;
	}
	
	public void ChangePos3()
	{
	
		destiny = position3.transform.position;
		punto = charact.transform.position + Vector3.forward*3;
		
	}
	
	// Update is called once per frame
	void Update () {
	
	
	transform.LookAt(punto + Vector3.up*2.8f);
	
	transform.position = Vector3.Lerp(transform.position, destiny, 0.1f);
	
	}
}
