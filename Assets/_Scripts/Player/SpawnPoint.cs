using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour {

    GameObject player;
    Health playerHealth;
	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<Health>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        

	}
    public void Respawn()
    {
        //if player is dead
        if (playerHealth.IsDead())
        {
            //set player poisition to position of the spawnpoint
            player.transform.position = transform.position;
            player.tag = "Player";
            player.GetComponent<PlayerCombatManager>().enabled = true;
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<Health>().enabled = true;
            GetComponent<CapsuleCollider>().enabled = true;
        }
    }
}
