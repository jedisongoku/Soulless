using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIItemSlot_Assign : MonoBehaviour {

    public static List<UIItemSlot_Assign> inventorySlots = new List<UIItemSlot_Assign>();
    public UIItemSlot slot;
	public UIItemDatabase itemDatabase;
	public int assignItem;

    private bool isAssigned = false;

    void Awake()
    {
        inventorySlots.Add(this);
    }

    void OnEnable()
	{
		if (this.slot == null)
			this.slot = this.GetComponent<UIItemSlot>();
        
        if (PlayFabDataStore.playerInventory.Count >= assignItem && !isAssigned)
        {
            isAssigned = true;
            itemDatabase.items.Add(PlayFabDataStore.catalogItems[PlayFabDataStore.playerInventory[assignItem - 1]]);
            SetItemToSlot();
        }

    }
	
	void SetItemToSlot()
	{
        if (this.slot == null || PlayFabDataStore.playerInventory == null)
        {
            this.Destruct();
            return;
        }
        slot.Assign(itemDatabase.GetByID(assignItem - 1));
        //Destruct();

    }

    public void SetItemToSlotInstant()
    {
        OnEnable();
    }


    private void Destruct()
	{
		DestroyImmediate(this);
	}

    void OnApplicationQuit()
    {
        itemDatabase.items.Clear();
    }
}
