using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using PlayFab;
using PlayFab.ClientModels;

public class RuneAssignSkillSlot : MonoBehaviour {

    public int skillSlot;
    public Image runeIcon;

    private float cooldownTimer;

    void Start()
    {
        ActionBar.skillSlots.Add(this);
    }

    public void AssignRune()
    {
        if(PlayFabDataStore.playerActiveRuneImages.ContainsKey(skillSlot))
        {
            runeIcon.sprite = PlayFabDataStore.playerActiveRuneImages[skillSlot];
            runeIcon.gameObject.SetActive(true);
        }
        
    }


}
