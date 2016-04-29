using UnityEngine;
using System.Collections;

public class NetworkManagerScript : MonoBehaviour {

    GameObject player;
    PlayerCombatManager combatManager;
    Runes playerRunes;
    CameraFollow cameraFollow;
    Health playerHealth;
    Transform spawnPoint;
    public Transform enemySpawnPoint;
    public string spawnPointName = "SpawnPoint";
    // Use this for initialization
    /*
    void Awake ()
    {
        enemySpawnPoint = GameObject.Find("SpawnPoint2").GetComponent<Transform>();
        if (GameObject.Find(spawnPointName) != null)
        {
            spawnPoint = GameObject.Find(spawnPointName).GetComponent<Transform>();
        }
        else
        {
            spawnPoint = GameObject.Find("SpawnPoint").GetComponent<Transform>();
        }
        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.Instantiate("Orc", enemySpawnPoint.position, enemySpawnPoint.rotation, 0);
        }
        player = PhotonNetwork.Instantiate("Elf", spawnPoint.position, spawnPoint.rotation, 0);
        combatManager = player.GetComponent<PlayerCombatManager>();
        combatManager.enabled = true;
        playerRunes = player.GetComponent<Runes>();
        playerRunes.enabled = true;
        //player.GetComponent<Health>().enabled = true;
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        cameraFollow.enabled = true;
        playerHealth = player.GetComponent<Health>();
        //playerHealth.enabled = true;

        
    }
    */
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnJoinedRoom()
    {
        PlayFabDataStore.playerCurrentHealth = PlayFabDataStore.playerMaxHealth;
        PlayFabDataStore.playerCurrentResource = 0;
    }
    void OnPhotonPlayerConnected(PhotonPlayer connected)
    {
        Debug.Log("New Player Joined Room!");
    }
    void OnLevelWasLoaded(int level)
    {
        enemySpawnPoint = GameObject.Find("SpawnPoint2").GetComponent<Transform>();
        if (GameObject.Find(spawnPointName) != null)
        {
            spawnPoint = GameObject.Find(spawnPointName).GetComponent<Transform>();
        }
        else
        {
            spawnPoint = GameObject.Find("SpawnPoint").GetComponent<Transform>();
        }
        if (PhotonNetwork.isMasterClient)
        {
            //PhotonNetwork.InstantiateSceneObject("Orc", enemySpawnPoint.position, enemySpawnPoint.rotation, 0, null);
        }
        player = PhotonNetwork.Instantiate("Elf", spawnPoint.position, spawnPoint.rotation, 0);
        combatManager = player.GetComponent<PlayerCombatManager>();
        combatManager.enabled = true;
        playerRunes = player.GetComponent<Runes>();
        playerRunes.enabled = true;
        //player.GetComponent<Health>().enabled = true;
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        cameraFollow.enabled = true;
        playerHealth = player.GetComponent<Health>();
        //playerHealth.enabled = true;

    }

}
