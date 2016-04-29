using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JoinFriendsRoom : MonoBehaviour
{

    public void JoinRoom(Text name)
    {
        PlayFabDataStore.friendUsername = name.text;
        PlayFabApiCalls.GetUserRoomName(PlayFabDataStore.friendsList[name.text]);

        Invoke("JoinFriend", 0.2f);

    }

    //joins a friend's room
    public void JoinFriend()
    {
        //photonnetwork.friendsCurrentRoomName must be set first
        PhotonCalls.JoinFriendRoom();
    }

}
