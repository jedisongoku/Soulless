using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon;

public class PhotonRandomMatchmaker : PunBehaviour
{
    public Transform spawnPoint;
    public Transform enemySpawnPoint;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
    }


    public override void OnJoinedLobby()
    {
        PhotonNetwork.automaticallySyncScene = true;
        //PhotonNetwork.JoinRandomRoom();
        PhotonNetwork.CreateRoom(null);
    }

    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Can't join random room!");
        
        ///argument is room name (null to assign a random name)
        PhotonNetwork.CreateRoom(null);
        
        Debug.Log("Creating a new room!");

    }

    public override void OnJoinedRoom()
    {
        PlayFabUserLogin.playfabUserLogin.Authentication("SUCCESS!", 2); //change the text of authentication text
        Debug.Log("Join Room Successfully!");
        Debug.Log("Room name is: "+PhotonNetwork.room);


        //Everytime a room created by a user, this part of the code has to be called to store the room name on playfab
        string[] roomName = PhotonNetwork.room.ToString().Split('\'');
        PlayFabDataStore.currentRoomName = roomName[1]; //gets the actual room value
        Dictionary<string, string> customData = new Dictionary<string, string>();
        customData.Add("RoomName", PlayFabDataStore.currentRoomName);
        PlayFabApiCalls.UpdateUserData(customData);


        //GameObject player = PhotonNetwork.Instantiate("PlayerCharacter", spawnPoint.position, Quaternion.identity, 0);
        //player.GetComponent<PlayerCombatManager>().enabled = true;

    }


}
