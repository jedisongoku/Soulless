using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QuestTracker : MonoBehaviour
{
    public static QuestTracker questTracker;

    public GameObject questPrefab;
    public GameObject requirementPrefab;
    public Transform questPanelTransform;
    public Transform questGridTransform;
    //public Transform requirementTransform;

    void Awake()
    {
        questTracker = this;
    }

    public void OnTrackerClicked()
    {
        questPanelTransform.gameObject.SetActive(!questPanelTransform.gameObject.activeInHierarchy);
        LoadTrackerQuests();
    
    }

    public void LoadTrackerQuests()
    {
        foreach (Transform child in questGridTransform)
        {
            Destroy(child.gameObject);
        }
        if (questPanelTransform.gameObject.activeInHierarchy == true)
        {
            foreach (var quest in PlayFabDataStore.playerQuestLog)
            {
                GameObject questObj = Instantiate(questPrefab);
                questObj.transform.SetParent(questGridTransform, false);
                questObj.GetComponentInChildren<Text>().text = PlayFabDataStore.catalogQuests[quest].displayName;

                string[] requirements = PlayFabDataStore.catalogQuests[quest].requirements.Split('#');

                foreach (var requirement in requirements)
                {
                    GameObject requirementObj = Instantiate(requirementPrefab);
                    requirementObj.transform.SetParent(questObj.GetComponentInChildren<VerticalLayoutGroup>().transform.parent, false);
                    requirementObj.GetComponentInChildren<Text>().text = requirement;
                }
            }
        }
    }
}
