using UnityEngine;
using System.Collections;

public class MenuHUD_Manager : MonoBehaviour
{
    public Canvas login;
    public Canvas mainMenu;

    void Awake()
    {
        if(PlayFabDataStore.playFabId != null)
        {
            login.gameObject.SetActive(false);
            mainMenu.gameObject.SetActive(true);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Settings()
    {

    }

}
