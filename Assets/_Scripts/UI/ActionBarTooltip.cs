using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActionBarTooltip : MonoBehaviour
{
    private int actionBarId;
    private string globeName;
    private Transform actionBarTransform;


    public void ShowTooltip(int id)
    {
        actionBarId = id;
        if(PlayFabDataStore.playerActiveSkillRunes.ContainsKey(actionBarId))
        {
            Invoke("SetTooltipData", 0.25f);
        }
    }

    public void HideTooltip()
    {
        CancelInvoke("SetTooltipData");
        UITooltip.Hide();
    }

    void SetTooltipData()
    {
        UITooltip.AddTitle(PlayFabDataStore.catalogRunes[PlayFabDataStore.playerActiveSkillRunes[actionBarId]].displayName);

        UITooltip.AddDescription(PlayFabDataStore.catalogRunes[PlayFabDataStore.playerActiveSkillRunes[actionBarId]].description);

        UITooltip.AnchorToRect(this.transform as RectTransform);
        UITooltip.Show();
    }

    public void ShowTooltipGlobe(string name)
    {
        globeName = name;
        Invoke("SetTooltipDataGlobe", 0.25f);
    }
    public void HideTooltipGlobe()
    {
        CancelInvoke("SetTooltipDataGlobe");
        UITooltip.Hide();
    }

    void SetTooltipDataGlobe()
    {
        UITooltip.AddTitle(globeName);

        if(globeName == "Health")
        {
            UITooltip.AddDescription(PlayFabDataStore.playerCurrentHealth + " / " + PlayFabDataStore.playerMaxHealth);
        }
        else
        {
            UITooltip.AddDescription(PlayFabDataStore.playerCurrentResource + " / " + PlayFabDataStore.playerMaxResource);
        }
        

        UITooltip.AnchorToRect(this.transform as RectTransform);
        UITooltip.Show();
    }
}
