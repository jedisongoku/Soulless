using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using PlayFab;
using PlayFab.ClientModels;

public class CheatPanel : MonoBehaviour
{
    public InputField grantItemText;
    private string itemInstanceId;

    public void GrantItem()
    {
        string[] items = { grantItemText.text };
        PlayFabApiCalls.GetLoot(items, null);
    }

    public void RevokeItem()
    {
        PlayFabApiCalls.RevokeInventoryItem(null, itemInstanceId);
    }


    public void GetCharacterRunes()
    {
        PlayFabApiCalls.GetAllCharacterRunes();
    }

    public void ListRuneImages()
    {
        foreach(var image in PlayFabDataStore.catalogRuneImages)
        {
            Debug.Log(image);
        }
    }

    public void ListActiveRunes()
    {
        foreach (var rune in PlayFabDataStore.playerAllRunes)
        {
            Debug.Log(rune.Key + " - " + rune.Value.itemClass + " - " + rune.Value.active);
        }
    }

    public void JoinRandomRoom()
    {
        PhotonCalls.LeaveRoom();
    }

}
