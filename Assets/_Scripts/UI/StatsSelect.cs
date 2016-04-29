using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StatsSelect : MonoBehaviour
{
    public string statsId;
    public string playFabDataId;
    public string description;
    public Text statsText;
    public Image statsDisabledImage;
    public Toggle statsToggle;

    public static List<Image> statsImages = new List<Image>();
    public static List<Toggle> statsToggles = new List<Toggle>();
    public static Dictionary<string, string> statsData = new Dictionary<string, string>();

    private static bool changesMade = false;

    void Awake()
    {
        statsImages.Add(statsDisabledImage);
        statsToggles.Add(statsToggle);
        Invoke("SetInitialValues", 2);
    }

    void SetInitialValues()
    {
        statsText.text = PlayFabDataStore.statsBuilder[statsId].ToString();
        if(PlayFabDataStore.playerStatBuilderUsedPoint < PlayFabDataStore.playerStatBuilderMaxPoint)
        {
            statsDisabledImage.enabled = false;
            statsToggle.interactable = true;
        }
    }

    public static void CalculateStatsBuilderPoints()
    {
        PlayFabDataStore.playerStatBuilderMaxPoint = (PlayFabDataStore.playerLevel) * 3;
        PlayFabDataStore.playerStatBuilderUsedPoint = 0;
        foreach (var stats in PlayFabDataStore.statsBuilder)
        {
            PlayFabDataStore.playerStatBuilderUsedPoint += stats.Value;
        }
        Debug.Log("MAX STATS POINT: " + PlayFabDataStore.playerStatBuilderMaxPoint);
        Debug.Log("USED STATS POINT: " + PlayFabDataStore.playerStatBuilderUsedPoint);
    }

    public void SelectStats()
    {
        if(PlayFabDataStore.playerStatBuilderUsedPoint < PlayFabDataStore.playerStatBuilderMaxPoint)
        {
            PlayFabDataStore.playerStatBuilderUsedPoint++;
            PlayFabDataStore.statsBuilder[statsId]++;
            statsText.text = PlayFabDataStore.statsBuilder[statsId].ToString();
            if(!statsData.ContainsKey(playFabDataId))
            {
                statsData.Add(playFabDataId, PlayFabDataStore.statsBuilder[statsId].ToString());
            }
            else
            {
                statsData[playFabDataId] = PlayFabDataStore.statsBuilder[statsId].ToString();
            }
            changesMade = true;
            
        }
        if (PlayFabDataStore.playerStatBuilderUsedPoint >= PlayFabDataStore.playerStatBuilderMaxPoint)
        {
            StatsDisabled();
        }

    }

    public static void StatsDisabled()
    {
        if(PlayFabDataStore.playerStatBuilderUsedPoint >= PlayFabDataStore.playerStatBuilderMaxPoint)
        {
            for(int i=0; i< statsImages.Count; i++)
            {
                statsImages[i].enabled = true;
                statsToggles[i].interactable = false;
                Debug.Log("STATS IMAGES CALL FALSE!");
            }
        }
        else
        {
            for (int i = 0; i < statsImages.Count; i++)
            {
                statsImages[i].enabled = false;
                statsToggles[i].interactable = true;
                Debug.Log("STATS IMAGES CALL TRUE!");
            }
        }
        
    }

    public static void SendData()
    {
        if(changesMade)
        {
            changesMade = false;
            PlayFabApiCalls.UpdateCharacterData(statsData);
            CharacterStats.characterStats.SetStatsText();
            Debug.Log("Stats UPDATED to PlayFab!");
            CharacterStats.characterStats.CalculateStats();
        }
        
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
        UITooltip.AddTitle(statsId);

        UITooltip.AddDescription(description);

        UITooltip.AnchorToRect(this.transform as RectTransform);
        UITooltip.Show();
    }

    void OnApplicationQuit()
    {
        SendData();
    }
}
