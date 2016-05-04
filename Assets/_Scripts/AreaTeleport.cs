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
        player = GameObject.FindGameObjectWithTag("Player");
        playerNavigation = player.GetComponent<NavMeshAgent>();
    }
    void OnEnable()
    {
        GetLocalPlayer();
    }
    void OnMouseDown()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerNavigation = player.GetComponent<NavMeshAgent>();

        if (Vector3.Distance(player.transform.position, transform.position) <= 8)
        {
            if (dungeon != null)
            {
                dungeon.SetActive(true);
                Debug.Log("Moing to the Dungeon");
            }
           
            playerNavigation.Stop();
            playerNavigation.ResetPath();
            //playerNavigation.Warp(dungeonStartPoint.position);
            //playerNavigation.updatePosition = dungeonStartPoint;
            playerNavigation.enabled = false;
            HUD_Manager.hudManager.ShowLoading(2);
            player.transform.position = dungeonStartPoint.position;
            Invoke("ActivatePlayerNavigation", 1);
            player.GetComponent<PhotonView>().RPC("SendPosition", PhotonTargets.Others, player.GetComponent<PhotonView>().viewID, dungeonStartPoint.position);
        }
    }

    void ActivatePlayerNavigation()
    {
        playerNavigation.enabled = true;
    }

}
