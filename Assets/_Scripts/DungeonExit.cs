using UnityEngine;
using System.Collections;

public class DungeonExit : MonoBehaviour
{

    public GameObject portal;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(CheckActive());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator CheckActive()
    {
        foreach (var quest in PlayFabDataStore.playerQuestLog)
        {
            if (quest == "Quest_ReturnArtifact")
            {
                portal.SetActive(true);
                yield return null;
            }

        }

        StartCoroutine(CheckActive());

        yield return null;
    }


}
