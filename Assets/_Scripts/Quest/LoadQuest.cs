using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LoadQuest : MonoBehaviour {

    public string questId;
    public Text title;
    public Text description;
    public Text currencyAmount;
    public GameObject questRequirementPrefab;
    public GameObject questRewardPrefab;
    public Transform requirementParentTransform;
    public Transform rewardParentTransform;

    private bool isCompletable = false;

    void Start()
    {
        //Testing Purposes
        /*Dictionary<string, string> customData = new Dictionary<string, string>();
        customData.Add("QuestLog", null);
        PlayFabApiCalls.UpdateCharacterData(customData);*/
    }

    void OnEnable()
    {
        string[] requirements = PlayFabDataStore.catalogQuests[questId].requirements.Split('#');

        title.text = PlayFabDataStore.catalogQuests[questId].displayName;
        description.text = PlayFabDataStore.catalogQuests[questId].description;

        foreach (var requirement in requirements)
        {
            GameObject obj = Instantiate(questRequirementPrefab);
            obj.transform.SetParent(requirementParentTransform, false);
            obj.GetComponentInChildren<Text>().text = requirement.ToString();
        }

        foreach (var reward in PlayFabDataStore.catalogQuests[questId].rewards)
        {
            GameObject obj = Instantiate(questRewardPrefab);
            obj.transform.SetParent(rewardParentTransform, false);
            obj.GetComponent<QuestReward>().itemId = reward;
            obj.GetComponent<QuestReward>().SetRewardIcon();
        }

        if(PlayFabDataStore.catalogQuests[questId].currencies != null)
        {
            foreach (var currency in PlayFabDataStore.catalogQuests[questId].currencies)
            {
                currencyAmount.transform.parent.gameObject.SetActive(true);
                currencyAmount.text = currency.Value.ToString();
            }
        }
        

    }

    void OnDisable()
    {
        currencyAmount.transform.parent.gameObject.SetActive(false);
        foreach (Transform child in rewardParentTransform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in requirementParentTransform)
        {
            Destroy(child.gameObject);
        }
    }

    public void AcceptQuest()
    {
        string questLogString = null;
        foreach(var quest in PlayFabDataStore.playerQuestLog)
        {
            questLogString += quest + "#";
        }
        questLogString += questId;

        Dictionary<string, string> customData = new Dictionary<string, string>();
        customData.Add("QuestLog", questLogString);
        PlayFabDataStore.playerQuestLog.Add(questId);
        PlayFabApiCalls.UpdateCharacterData(customData);
        QuestTracker.questTracker.LoadTrackerQuests();
        gameObject.SetActive(false);
        GetComponent<RaycastUI>().OnMouseExit();

    }

    public void CompleteQuest()
    {
        if (PlayFabDataStore.playerQuestLog.Contains(questId))
        {
            PlayFabDataStore.playerQuestLog.Remove(questId);

            if (!PlayFabDataStore.playerCompletedQuests.Contains(questId))
            {
                PlayFabDataStore.playerCompletedQuests.Add(questId);
            }

            string[] items = { questId };
            PlayFabApiCalls.GrantItemsToCharacter(items, null, "Quest");
            gameObject.SetActive(false);

            string questLogString = null;
            foreach (var quest in PlayFabDataStore.playerQuestLog)
            {
                questLogString += quest + "#";
                Debug.Log(questLogString);
            }
            if(PlayFabDataStore.playerQuestLog.Count != 0)
            {
                questLogString = questLogString.Remove(questLogString.Length - 1);
            }
            
            Dictionary<string, string> customData = new Dictionary<string, string>();
            customData.Add("QuestLog", questLogString);
            PlayFabApiCalls.UpdateCharacterData(customData);
            QuestTracker.questTracker.LoadTrackerQuests();
            GetComponent<RaycastUI>().OnMouseExit();
        }
        
        
    }

}
