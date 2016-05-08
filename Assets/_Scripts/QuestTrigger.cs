using UnityEngine;
using System.Collections;

public class QuestTrigger : MonoBehaviour {

    public GameObject npc;
    [Header("False activates npc")]
    public bool deactivate = true;
    public string triggerQuest;
    [Header("False activates only if quest is complete")]
    public bool containsQuest = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (containsQuest)
        {
            if (PlayFabDataStore.playerQuestLog.Contains(triggerQuest) ||
                PlayFabDataStore.playerCompletedQuests.Contains(triggerQuest))
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
        else
        {
            if (PlayFabDataStore.playerCompletedQuests.Contains(triggerQuest))
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
}
