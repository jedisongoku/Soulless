using UnityEngine;
using System.Collections;
using PlayFab;
using PlayFab.ClientModels;

public class PlayerQuest
{
    public static PlayerQuest quest;

    public string itemId;
    public string instanceId;
    public string itemClass;
    public string displayName;
    public string completed;

    void Awake()
    {
        quest = this;
    }

    public PlayerQuest(string _itemId, string _instanceId, string _itemClass, string _displayName, string _completed)
    {
        itemId = _itemId;
        instanceId = _instanceId;
        itemClass = _itemClass;
        displayName = _displayName;
        completed = _completed;

    }
}
