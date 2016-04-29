using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CharacterSelect : MonoBehaviour {

    public Toggle characterSelected;
    public Text characterName;



    public void Selected()
    {
        PlayFabDataStore.characterName = characterName.text;
        PlayFabDataStore.characterId = PlayFabDataStore.characters[characterName.text];
        Debug.Log(PlayFabDataStore.characterId);
    }

    public void RemoveCharacter()
    {
        PlayFabApiCalls.RemoveCharacter(PlayFabDataStore.characters[characterName.text]);
        PlayFabDataStore.characters.Remove(characterName.text);
        
    }
}
