using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.Chat;
using ExitGames.Client.Photon;

public class ChatManager : MonoBehaviour, IChatClientListener
{
    public static ChatClient chatClient;

    public string photonChatId = "9943f53d-cf40-48fd-b273-797bc306e684";
    public string photonAppVersion = "1.0";
    public InputField messageField;
    public Text chatField;
    public LayoutElement textLayout;


    private string characterName;

    void Start()
    {
        chatClient = new ChatClient(this);
        characterName = PlayFabDataStore.characterName;
        chatField.text = "<color=yellow>Welcome to Soulless! Have Fun!</color>\n";
        Connect();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            if(messageField.gameObject.activeInHierarchy)
            {
                chatClient.PublishMessage("GeneralChat", messageField.textComponent.text);
                messageField.gameObject.SetActive(false);
            }
            else
            {
                messageField.gameObject.SetActive(true);
            }
            
        }

        if (chatClient != null)
        {
            chatClient.Service();
            textLayout.minHeight = chatField.rectTransform.rect.height;
        }

    }

    void Connect()
    {
        ExitGames.Client.Photon.Chat.AuthenticationValues authentication = new ExitGames.Client.Photon.Chat.AuthenticationValues();
        authentication.UserId = PlayFabDataStore.characterName;
        chatClient.Connect(photonChatId, photonAppVersion, authentication);
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log("OnChatStateChange");
        Debug.Log(state);
    }

    public void OnConnected()
    {
        chatClient.Subscribe(new string[] { "GeneralChat" });
        Debug.Log("Connected to Chat!");
        //throw new NotImplementedException();
    }

    public void OnDisconnected()
    {
        Debug.Log("Disconnected from the Chat!");
        //throw new NotImplementedException();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for(int i = 0; i < senders.Length; i++)
        {
            chatField.text += "<color=green>" + senders[i] + ": </color>" + messages[i] + "\n";
        }
        //throw new NotImplementedException();
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        throw new NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log("OnSubscribed");
        for (int i = 0; i < channels.Length; i++)
        {
            Debug.Log(channels[i] + " - " + results[i]);
        }
        
        //throw new NotImplementedException();
    }

    public void OnUnsubscribed(string[] channels)
    {
        throw new NotImplementedException();
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log(level);
        Debug.Log(message);
    }

    void OnApplicationQuit()
    {
        chatClient.Disconnect();
    }
}
