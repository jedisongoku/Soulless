using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabCreateCharacter : MonoBehaviour
{
    public InputField characterName;
    public Canvas mainMenu;
    public Text errorText;

    public static PlayFabCreateCharacter playFabCreateCharacter;

    void Awake()
    {
        playFabCreateCharacter = this;
    }

    public void Create()
    {
        PlayFabApiCalls.CreateNewCharacter(characterName.text);
    }

    public void Cancel()
    {
        mainMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }


}
