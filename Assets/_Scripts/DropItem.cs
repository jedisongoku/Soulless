using UnityEngine;
using System.Collections;

public class DropItem : MonoBehaviour
{
    public string dropTableId;
    public string dropItemId;
    public Mesh skillRuneMesh;
    public Mesh modifierRuneMesh;
    public Mesh itemMesh;
    public Material skillRuneMaterial;
    public Material modifierRuneMaterial;
    public Material itemMaterial;
    public bool isItemReceived = false;
    private bool itemReceived = false;
    private PhotonView photonView;

    void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }
	
    public void GetDropItemId()
    {
        string[] items = { dropTableId };
        PlayFabApiCalls.GetLoot(items, gameObject);

        StartCoroutine(DropItemToGround());
    }

    //Checks until item information returns from the playFab
    IEnumerator DropItemToGround()
    {
        Debug.Log("Dropitemid " + dropItemId);
        if(isItemReceived)
        {
            if (PlayFabDataStore.catalogItems.ContainsKey(dropItemId))
            {
                GameObject item = PhotonNetwork.Instantiate("DropItem", transform.position, transform.rotation, 0);
                photonView.RPC("SetItemDetails", PhotonTargets.AllBufferedViaServer, item.GetComponent<PhotonView>().viewID, PlayFabDataStore.catalogItems[dropItemId].displayName, dropItemId, "Item");
                /*item.GetComponent<TextMesh>().text = PlayFabDataStore.catalogItems[dropItemId].displayName;
                item.GetComponent<DroppedItem>().itemId = dropItemId;*/
                Debug.Log("Item Dropped : " + dropItemId);
            }
            else
            if (PlayFabDataStore.catalogRunes.ContainsKey(dropItemId))
            {
                GameObject item = PhotonNetwork.Instantiate("DropItem", transform.position, transform.rotation, 0);
                photonView.RPC("SetItemDetails", PhotonTargets.AllBufferedViaServer, item.GetComponent<PhotonView>().viewID, PlayFabDataStore.catalogRunes[dropItemId].displayName, dropItemId, PlayFabDataStore.catalogRunes[dropItemId].itemClass);
                /*item.GetComponent<TextMesh>().text = PlayFabDataStore.catalogItems[dropItemId].displayName;
                item.GetComponent<DroppedItem>().itemId = dropItemId;*/
                Debug.Log("Item Dropped : " + dropItemId);
            }
            else
            {
                GameObject item = PhotonNetwork.Instantiate("DropItem", transform.position, transform.rotation, 0);
                photonView.RPC("SetItemDetails", PhotonTargets.AllBufferedViaServer, item.GetComponent<PhotonView>().viewID, "Gold", dropItemId, "Gold");
                /*item.GetComponent<TextMesh>().text = "Gold";
                item.GetComponent<DroppedItem>().itemId = dropItemId;*/
                Debug.Log("Item Dropped : " + dropItemId);
            }
            itemReceived = true; // this is to double check the condition, in case of isItemreceived becomes true after the if statement
        }


        yield return new WaitForSeconds(0.5f);

        if(isItemReceived == false || itemReceived == false)
        {
            StartCoroutine(DropItemToGround());
        }
        
    }

    [PunRPC]
    void SetItemDetails(int sourceID, string name, string itemID, string itemClass)
    {
        GameObject source = PhotonView.Find(sourceID).gameObject;
        source.GetComponent<TextMesh>().text = name;
        source.GetComponent<DroppedItem>().itemId = itemID;
        Debug.Log("Item Details: " + itemID);
        if(itemClass == "Item")
        {
            source.GetComponent<TextMesh>().color = new Color(255, 0, 255);
        }
        else
        if (itemClass == "Skill")
        {
            source.GetComponent<TextMesh>().color = new Color(255, 0, 0);
            source.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = skillRuneMesh;
            source.GetComponentInChildren<SkinnedMeshRenderer>().material = skillRuneMaterial;
            
        }
        else
        if (itemClass == "Modifier")
        {
            source.GetComponent<TextMesh>().color = new Color(0, 255, 255);
            source.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = modifierRuneMesh;
            source.GetComponentInChildren<SkinnedMeshRenderer>().material = modifierRuneMaterial;
        }
        source.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;



    }
}
