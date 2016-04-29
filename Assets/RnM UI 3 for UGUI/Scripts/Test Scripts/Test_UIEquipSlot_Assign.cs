using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Test_UIEquipSlot_Assign : MonoBehaviour {
	
	public UIEquipSlot slot;
	public UIItemDatabase itemDatabase;
	public string itemType;
	
	void OnEnable()
	{
		if (this.slot == null)
			this.slot = this.GetComponent<UIEquipSlot>();
	}
	
	void Start()
	{
		if (this.slot == null || this.itemDatabase == null)
		{
			this.Destruct();
			return;
		}
		
        if(PlayFabDataStore.playerEquippedItems.ContainsKey(itemType))
        {
            this.slot.Assign(PlayFabDataStore.catalogItems[PlayFabDataStore.playerEquippedItems[itemType].itemId]);
            this.Destruct();
        }
		
	}
	
	private void Destruct()
	{
		DestroyImmediate(this);
	}
}
