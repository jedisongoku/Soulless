using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon;

public class PhotonCalls : PunBehaviour
{
    GameObject spawnPoint;
    
    // static string friendRoomName = null; // get this directly from playfabdatastore
    //exits the current room
    public static void LeaveRoom()
    {
        
        PhotonNetwork.LeaveRoom();
    }
    //exits the current room, but also preps to join a friends room
    public static void JoinFriendRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    //when the player leaves their current room, reenter the lobby
    public override void OnLeftRoom()
    {
        PhotonNetwork.JoinLobby();
    }
    //upon reaching the lobby, join a random room 
    public override void OnJoinedLobby()
    {
        if (PlayFabDataStore.friendsCurrentRoomName != null)
        {
            PhotonNetwork.JoinRoom(PlayFabDataStore.friendsCurrentRoomName);
        }
        else
        {
            Debug.Log("Looking for room to join");
            PhotonNetwork.JoinRandomRoom();
        }
    }

    //if the player fails to join a random room
    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Can't join random room!");
        Debug.Log("Creating a new room!");
        ///argument is room name (null to assign a random name)
        PhotonNetwork.CreateRoom("Tester");

       
    }
    //upon joining a new room, output the room name
    public override void OnJoinedRoom()
    {
        //reset to false for next check
        PlayFabDataStore.friendsCurrentRoomName = null;
        spawnPoint = GameObject.Find("SpawnPoint");
        Debug.Log("Join Room Successfully!");
        Debug.Log("Room name is: " + PhotonNetwork.room);

        GameObject player = PhotonNetwork.Instantiate("Elf", spawnPoint.transform.position, Quaternion.identity, 0);
       
        player.GetComponent<PlayerCombatManager>().enabled = true;
        player.GetComponent<Runes>().enabled = true;
        
        //set entering player to full health. This is now dealed someplace else
        //player.GetComponent<Health>().health = player.GetComponent<Health>().maxHealth;


    }


}

