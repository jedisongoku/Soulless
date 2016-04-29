using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FriendsList : MonoBehaviour
{
    public static FriendsList friendsList;

    public GameObject friendPrefab;
    public GameObject addFriendPanel;
    public InputField friendEmail;

    private List<GameObject> friendList = new List<GameObject>();

    void Start()
    {
        friendsList = this;
        LoadFriendsList();
    }

    public void LoadFriendsList()
    {
        friendList.Clear();
        foreach(var friend in PlayFabDataStore.friendsList)
        {
            GameObject obj = Instantiate(friendPrefab);
            friendList.Add(obj);
            obj.transform.SetParent(this.transform, false);
            obj.GetComponentInChildren<Text>().text = friend.Key.ToString();
            obj.GetComponent<Toggle>().group = GetComponent<ToggleGroup>();
        }
    }

    public void AddFriend()
    {
        addFriendPanel.gameObject.SetActive(true);
    }

    public void AddFriendPlayFab()
    {
        PlayFabApiCalls.AddFriend(friendEmail.text);
    }

    public void CancelAddFriend()
    {
        addFriendPanel.gameObject.SetActive(false);
    }

    public void RemoveFriend()
    {

    }

    










}
