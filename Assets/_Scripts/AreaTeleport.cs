using UnityEngine;
using System.Collections;

public class AreaTeleport : MonoBehaviour {

    public GameObject dungeon;
    public Transform dungeonStartPoint;


    private GameObject player;
    private NavMeshAgent playerNavigation;

    void Start()
    {
        Invoke("GetLocalPlayer", 3);
    }

    void GetLocalPlayer()
    {
        player = GameManager.players[0];
        playerNavigation = player.GetComponent<NavMeshAgent>();
    }

    void OnMouseDown()
    {
        if(Vector3.Distance(player.transform.position, transform.position) <= 8)
        {
            dungeon.SetActive(true);
            Debug.Log("Moing to the Dungeon");
            playerNavigation.Stop();
            playerNavigation.ResetPath();
            //playerNavigation.Warp(dungeonStartPoint.position);
            //playerNavigation.updatePosition = dungeonStartPoint;
            playerNavigation.enabled = false;
            HUD_Manager.hudManager.ShowLoading(2);
            player.transform.position = dungeonStartPoint.position;
            Invoke("ActivatePlayerNavigation", 1);
        }
    }

    void ActivatePlayerNavigation()
    {
        playerNavigation.enabled = true;
    }

    /*public string levelToTeleport;
    public string spawnPointName = "SpawnPoint";

    GameObject player;
    NetworkManagerScript networkManager;
	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManagerScript>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (player != null && Vector3.Distance(player.transform.position, transform.position) < 2)
        {
            if (spawnPointName != null)
            {
               networkManager.spawnPointName = spawnPointName;
            }
            PhotonNetwork.LoadLevel(levelToTeleport);
        }
    }*/

   
}
