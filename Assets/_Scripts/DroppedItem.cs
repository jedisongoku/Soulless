using UnityEngine;
using System.Collections;

public class DroppedItem : MonoBehaviour
{

    public string itemId;
    private PhotonView photonView;
    private bool isLootable = false;

    void Awake ()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(new Vector3(0, 180, 0));
        photonView = GetComponent<PhotonView>();
    }

    public void OnMouseDown()
    {
        photonView.RPC("LootTheItem", PhotonTargets.AllViaServer, PhotonNetwork.player.ID);
    }

    [PunRPC]
    void LootTheItem(int playerID)
    {
        if(PlayFabDataStore.catalogItems.ContainsKey(itemId))
        {
            if (PlayFabDataStore.playerInventory.Count < PlayFabDataStore.playerInventorySlotCount)
            {
                isLootable = true;
            }
        }
        else 
        if(!PlayFabDataStore.playerAllRunes.ContainsKey(itemId))
        {
            isLootable = true;
        }


        if(isLootable)
        {
            if (PhotonNetwork.player.ID == playerID)
            {

                string[] items = { itemId };
                if (PlayFabDataStore.catalogItems.ContainsKey(itemId))
                {
                    PlayFabApiCalls.GrantItemsToCharacter(items, "IsEquipped", "Item");

                }
                else if (PlayFabDataStore.catalogRunes.ContainsKey(itemId))
                {
                    PlayFabApiCalls.GrantItemsToCharacter(items, "Active", PlayFabDataStore.catalogRunes[itemId].itemClass);
                }

                if (itemId == "Item_Gold")
                {
                    Debug.Log("currency added");
                    int gold = Random.Range(5, 50);
                    PlayFabDataStore.playerUnupdatedCurrency += gold;
                    PlayFabDataStore.playerCurrency += gold;
                    CharacterStats.characterStats.SetGoldText();
                }
                Debug.Log(itemId + " Item Looted from the ground");
            }
            if (photonView.isMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
        
        
        
    }

}
