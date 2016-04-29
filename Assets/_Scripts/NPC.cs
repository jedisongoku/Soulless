using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPC : MonoBehaviour {

    public List<string> dialogue;
    public Shader defaultShader;
    public Shader outlineShader;

    
    [Header("Drop Quest Panel Here")]
    public Canvas questPanel;
    public GameObject acceptButton;
    public GameObject declineButton;
    public GameObject completeButton;


    //what quests this npc can grant
    [Header("Quest IDs this NPC can start")]
    public List<string> startingQuests;
    //what quests this npc can complete
    [Header("Quest IDs this NPC can end")]
    public List<string> endingQuests;

    //offset from transform.position for each npc
    public Vector3 faceLocation;
    //how close to set camera to npc's face
    public float faceDistance;
    //name of npc
    public string npcName;

    private bool finishingQuest = false;
    private int endQuestId;
    private GameObject player;
    //private Camera dialogueCamera;
    private Dialogue dialogueManager;
    private GameObject gameManager;

    public Texture portraitImage;
    //private Color questOutlineColor = new Color(255, 255, 0, 255);


    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameObject.Find("GameManager");
        dialogueManager = gameManager.GetComponent<Dialogue>();
        //dialogueCamera = GameObject.Find("DialogueCamera").GetComponent<Camera>();
        questPanel = gameManager.GetComponent<GameManager>().quest;
        acceptButton = questPanel.gameObject.transform.Find("Button (Accept)").gameObject;
        declineButton = questPanel.gameObject.transform.Find("Button (Decline)").gameObject;
        completeButton = questPanel.gameObject.transform.Find("Button (Complete)").gameObject;
    }

    void Update()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if(!finishingQuest)
        {
            if (player != null && Vector3.Distance(player.transform.position, transform.position) < 30)
            {
                foreach (var quest in PlayFabDataStore.playerQuestLog)
                {
                    if (endingQuests.Contains(quest))
                    {
                        //GetComponentInChildren<SkinnedMeshRenderer>().material.shader = outlineShader;
                        //GetComponentInChildren<SkinnedMeshRenderer>().material.SetColor("_OutlineColor", questOutlineColor);
                        endQuestId = endingQuests.IndexOf(quest);
                        finishingQuest = true;
                        Debug.Log(finishingQuest);
                        break;
                    }
                }
            }
        }
        
    }
    
    public void ClickedNPC()
    {
        //rotate to face player
        Vector3 position = transform.position;
        position.y = 0;
        Vector3 playerPosition = player.transform.position;
        playerPosition.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(playerPosition-position);
        transform.rotation = targetRotation;
       
        //activate dialogue box
        dialogueManager.StartDialogue(npcName,dialogue, transform.position, portraitImage);
        /*
        dialogueCamera.transform.position = transform.position+faceLocation;
        dialogueCamera.transform.rotation = transform.rotation;
        dialogueCamera.transform.Rotate(0,180,0);
        dialogueCamera.transform.Translate(Vector3.back*faceDistance);
        */
        //dialogueCamera.transform.Translate(Vector3.up * 5);


    }


    public void OnMouseDown()
    {
        /*/////////////////////////////test code
        //PlayFabDataStore.playerCompletedQuests.Clear();
        Debug.Log("Current Quests: ");
        foreach(string quests in PlayFabDataStore.playerQuestLog)
        {
            Debug.Log(quests);
        }
        Debug.Log("Completed Quests: " );
        foreach (string quests in PlayFabDataStore.playerCompletedQuests)
        {
            Debug.Log(quests);
        }
        ///////////////////////////*/

        //if the player is within 3 units of the npc
        if (Vector3.Distance(player.transform.position, transform.position) < 3)
        {
            //finishingQuest = false;
            ClickedNPC();
            /*for(int i = 0; i < endQuestId.Count; i++)
            {
                EndQuest(i);
                if(finishingQuest == true)
                {
                    break;
                }
            }*/
            if (finishingQuest)
            {
                finishingQuest = false;
                EndQuest();
            }
            //if you aren't finishing a quest and this npc has quests to give
            if (finishingQuest == false && startingQuests.Count != 0)
            {
                StartQuest(0);
            }
            
        }
    }
    public void StartQuest(int questIndex)
    {
        //if this npc is not a quest giver
        if(startingQuests.Count == 0)
        {
            return;
        }
        //if the player has not already accepted this quest or completed this quest
        if (!PlayFabDataStore.playerQuestLog.Contains(startingQuests[questIndex]) 
            && !PlayFabDataStore.playerCompletedQuests.Contains(startingQuests[questIndex]))
        {
            //set the quest panel's quest id to the quest carried by the current npc
            questPanel.GetComponent<LoadQuest>().questId = startingQuests[questIndex];
            acceptButton.SetActive(true);
            declineButton.SetActive(true);
            completeButton.SetActive(false);
            questPanel.gameObject.SetActive(true);
        }
    }
    public void EndQuest()
    {
        //if this npc is not a quest ender
        if (endingQuests.Count == 0)
        {
            return;
        }
        Debug.Log("Ending Quest");
        questPanel.GetComponent<LoadQuest>().questId = endingQuests[endQuestId];
        acceptButton.SetActive(false);
        declineButton.SetActive(false);
        completeButton.SetActive(true);
        questPanel.gameObject.SetActive(true);

        /*//if the player has accepted this quest and has not completed it
        if (PlayFabDataStore.playerQuestLog.Contains(endQuestId[questIndex])
            && !PlayFabDataStore.playerCompletedQuests.Contains(endQuestId[questIndex]))
        {
            //complete quest
            finishingQuest = true;
            PlayFabDataStore.playerCompletedQuests.Add(endQuestId[questIndex]);
            PlayFabDataStore.playerQuestLog.Remove(endQuestId[questIndex]);
            Debug.Log("You completed "+ endQuestId[questIndex]);
            //questPanel.GetComponent<LoadQuest>().questId = questId[questIndex];
            //questPanel.SetActive(true);
        }*/
    }


    void OnMouseOver()
    {
        GetComponentInChildren<SkinnedMeshRenderer>().material.shader = outlineShader;
    }

    void OnMouseExit()
    {
        GetComponentInChildren<SkinnedMeshRenderer>().material.shader = defaultShader;
    }
}
