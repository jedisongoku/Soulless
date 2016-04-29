using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QuestReward : MonoBehaviour {

    public string itemId;
    public Image icon;

    public void SetRewardIcon()
    {
        icon.sprite = PlayFabDataStore.catalogRuneImages[itemId];

    }

    public void ShowTooltip()
    {
        Invoke("SetTooltipData", 0.25f);
    }

    public void HideTooltip()
    {
        CancelInvoke("SetTooltipData");
        UITooltip.Hide();
    }

    void SetTooltipData()
    {
        UITooltip.AddTitle(PlayFabDataStore.catalogRunes[itemId].displayName);
        if (PlayFabDataStore.catalogRunes[itemId].itemClass == "Skill")
        {
            if (PlayFabDataStore.catalogRunes[itemId].resourceGeneration == 0)
            {
                UITooltip.AddLineColumn("Cost: " + PlayFabDataStore.catalogRunes[itemId].resourceUsage + " Resource");
            }
            else
            {
                UITooltip.AddLineColumn("Generate: " + PlayFabDataStore.catalogRunes[itemId].resourceGeneration + " Resource");
            }
        }


        UITooltip.AddDescription(PlayFabDataStore.catalogRunes[itemId].description);

        UITooltip.AnchorToRect(this.transform as RectTransform);
        UITooltip.Show();
    }
}
