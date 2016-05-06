using UnityEngine;
using System.Collections;

public class NPCDeath : MonoBehaviour {

    public GameObject npc;
    public bool deactivate = true;
    public string triggerQuest;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (PlayFabDataStore.playerQuestLog.Contains(triggerQuest))
        {
            if (deactivate)
            {
                this.gameObject.SetActive(false);
            }
            else
            {
                npc.SetActive(true);
            }
        }

    }
}
