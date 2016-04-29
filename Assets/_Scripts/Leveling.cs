using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Leveling : MonoBehaviour
{
    private static List<int> levelMaxExperienceList = new List<int>();

    private int experience;
    private int kill;
    private static bool isUpdated = false;
    private Dictionary<string, string> customData = new Dictionary<string, string>();

    void Start()
    {
        for(int i = 1; i <= PlayFabDataStore.gameMaxLevel; i++)
        {
            if(i < 5)
            {
                levelMaxExperienceList.Add(Mathf.RoundToInt(40 * Mathf.Pow(i, 2) + 260 * i));
                
                experience = Mathf.RoundToInt(levelMaxExperienceList[i - 1] * (5 - Mathf.Log10(PlayFabDataStore.gameMaxLevel * Mathf.Pow(i, 2) * 5)) / 100) + 5;
                kill = Mathf.RoundToInt(levelMaxExperienceList[i - 1] / experience);
                //Debug.Log("Level " + i + " XP Cap is " + Mathf.RoundToInt(40 * Mathf.Pow(i, 2) + 260 * i) + " need " + kill + " kills each " + experience + " XP");
            }
            else
            if (i < 11)
            {
                levelMaxExperienceList.Add(Mathf.RoundToInt(40 * Mathf.Pow(i, 2) + 360 * i));
                
                experience = Mathf.RoundToInt(levelMaxExperienceList[i - 1] * (5 - Mathf.Log10(PlayFabDataStore.gameMaxLevel * Mathf.Pow(i, 2) * 5)) / 100);
                kill = Mathf.RoundToInt(levelMaxExperienceList[i - 1] / experience);
                //Debug.Log("Level " + i + " XP Cap is " + Mathf.RoundToInt(40 * Mathf.Pow(i, 2) + 360 * i) + " need " + kill + " kills each " + experience + " XP");
                
            }
            else
            if (i < 27)
            {
                levelMaxExperienceList.Add(Mathf.RoundToInt(-0.4f * Mathf.Pow(i, 3) + 40.4f * Mathf.Pow(i, 2) + 396 * i) / 100 * 100);
                
                experience = Mathf.RoundToInt(levelMaxExperienceList[i - 1] * ((5 + Mathf.Log10(PlayFabDataStore.gameMaxLevel * Mathf.Pow(i, 2))) / i / 100));
                kill = Mathf.RoundToInt(levelMaxExperienceList[i - 1] / experience);
                //Debug.Log("Level " + i + " XP Cap is " + Mathf.RoundToInt(-0.4f * Mathf.Pow(i, 3) + 40.4f * Mathf.Pow(i, 2) + 396 * i) / 100 * 100 + " need " + kill + " kills each " + experience + " XP");
                
            }
            else
            if (i < 60)
            {
                levelMaxExperienceList.Add(Mathf.RoundToInt((65 * Mathf.Pow(i, 2) - 165 * i - 6750) * 0.89f) / 100 * 100);
                
                experience = Mathf.RoundToInt(levelMaxExperienceList[i - 1] * ((5 + Mathf.Log10(PlayFabDataStore.gameMaxLevel * Mathf.Pow(i, 2))) / i / 100));
                kill = Mathf.RoundToInt(levelMaxExperienceList[i - 1] / experience);
                //Debug.Log("Level " + i + " XP Cap is " + levelMaxExperienceList[i-1] + " need " + kill + " kills each " + experience + " XP");
                
            }
        }

        Invoke("SetHUD", 3);
        InvokeRepeating("SetPlayFabData", 60f, 60f);

    }

    void SetHUD()
    {
        PlayFabDataStore.maxExperienceToLevel = levelMaxExperienceList[PlayFabDataStore.playerLevel - 1];
        HUD_Manager.hudManager.SetHealthAndResource();
        HUD_Manager.hudManager.playerLevelText.text = PlayFabDataStore.playerLevel.ToString();
    }

    public static void GrantExperince()
    {
        int experience = 0;
        int level = PlayFabDataStore.playerLevel;

        if (PlayFabDataStore.playerLevel < 5)
        {
            experience = Mathf.RoundToInt(levelMaxExperienceList[PlayFabDataStore.playerLevel - 1] * (5 - Mathf.Log10(PlayFabDataStore.gameMaxLevel * Mathf.Pow(PlayFabDataStore.playerLevel, 2) * 5)) / 100) + 5;
        }
        else
        
        if (PlayFabDataStore.playerLevel < 11)
        {
            experience = Mathf.RoundToInt(levelMaxExperienceList[PlayFabDataStore.playerLevel - 1] * (5 - Mathf.Log10(PlayFabDataStore.gameMaxLevel * Mathf.Pow(PlayFabDataStore.playerLevel, 2) * 5)) / 100);
            
        }
        else
            if (PlayFabDataStore.playerLevel < 27)
        {
            experience = Mathf.RoundToInt(levelMaxExperienceList[PlayFabDataStore.playerLevel - 1] * ((5 + Mathf.Log10(PlayFabDataStore.gameMaxLevel * Mathf.Pow(PlayFabDataStore.playerLevel, 2))) / PlayFabDataStore.playerLevel / 100));
        }
        else
            if (PlayFabDataStore.playerLevel < 60)
        {
            experience = Mathf.RoundToInt(levelMaxExperienceList[PlayFabDataStore.playerLevel - 1] * ((5 + Mathf.Log10(PlayFabDataStore.gameMaxLevel * Mathf.Pow(PlayFabDataStore.playerLevel, 2))) / PlayFabDataStore.playerLevel / 100));
        }

        if (PlayFabDataStore.playerExperience + experience < levelMaxExperienceList[PlayFabDataStore.playerLevel - 1])
        {
            PlayFabDataStore.playerExperience += experience;
        }
        else
        {
            PlayFabDataStore.playerExperience = experience - (levelMaxExperienceList[PlayFabDataStore.playerLevel - 1] - PlayFabDataStore.playerExperience);
            //Debug.Log("Before" + PlayFabDataStore.playerLevel);
            PlayFabDataStore.playerLevel++;
            PlayFabDataStore.maxExperienceToLevel = levelMaxExperienceList[PlayFabDataStore.playerLevel - 1];
            //Debug.Log("After" + PlayFabDataStore.playerLevel);
        }
        
        HUD_Manager.hudManager.SetHealthAndResource();
        isUpdated = true;
    }

    void SetPlayFabData()
    {
        
        if(isUpdated)
        {
            isUpdated = false;
            customData.Clear();

            customData.Add("Experience", PlayFabDataStore.playerExperience.ToString());
            customData.Add("Level", PlayFabDataStore.playerLevel.ToString());
            PlayFabApiCalls.AddUserCurrency(PlayFabDataStore.playerUnupdatedCurrency);

            PlayFabApiCalls.UpdateCharacterData(customData);
        }
        
    }

    void OnApplicationQuit()
    {
        isUpdated = true;
        SetPlayFabData();
    }
    

}
