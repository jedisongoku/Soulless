using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {

    GameObject dialogueBox;
    Text dialogueText;
    Text displayName;
    RawImage dialogueImage;
    double textTimer = 0;
    double timeToSwitch = 5;

    List<string> messageList;
    int messageCount = 0;
    int totalMessages = 0;

    Vector3 npcPosition = Vector3.zero;
    bool inConversation = false;
    GameObject player;
	// Use this for initialization
	void Start ()
    {
       
        dialogueBox = GameObject.Find("DialogueBox");
        dialogueText = GameObject.Find("DialogueText").GetComponent<Text>();
        displayName = GameObject.Find("DialogueName").GetComponent<Text>();
        dialogueImage = GameObject.Find("FaceImage").GetComponent<RawImage>();
        dialogueBox.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
    }
	void Update()
    {
        if(dialogueBox.activeSelf == true)
        {
            textTimer += Time.deltaTime;
            
        }
        if (totalMessages > 0 && ((Input.GetKeyDown(KeyCode.Space)) || textTimer > timeToSwitch))
        {
            NextMessage();
        }
        if(inConversation && Vector3.Distance(player.transform.position, npcPosition) > 6)
        {
            //Debug.Log(Vector3.Distance(player.transform.position, npcPosition));
            EndDialogue();
        }
        
    }
	
    public void StartDialogue(string name,List<string> messages, Vector3 npcPos, Texture portraitImage)
    {
        
        npcPosition = npcPos;
        inConversation = true;
        displayName.text = name;
        messageList = messages;
        dialogueBox.SetActive(true);
        dialogueImage.texture = portraitImage;
        //set count to 0
        messageCount = 0;
        //get number of messages in dialogue
        totalMessages = messages.Count;
        //display first message
        dialogueText.text = messages[0];
        /*
        while(messageCount != totalMessages)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                //increase message count
                messageCount++;
                //display next message
                dialogueText.text = messages[messageCount];
            }
        }
        dialogueBox.SetActive(false);
        */
    }
    public void NextMessage()
    {
        textTimer = 0;
        messageCount++;
        if (messageCount < totalMessages)
        {
            dialogueText.text = messageList[messageCount];
        }
        else
        {
            EndDialogue();
        }
    }
    public void EndDialogue()
    {
       
        totalMessages = 0;
        messageCount = 0;
        dialogueBox.SetActive(false);
        textTimer = 0;
        inConversation = false;
        npcPosition = Vector3.zero;
        
    }
   

    }
