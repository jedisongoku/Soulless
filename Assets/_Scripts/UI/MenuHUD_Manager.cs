using UnityEngine;
using System.Collections;

public class MenuHUD_Manager : MonoBehaviour
{
    public Canvas login;
    public Canvas mainMenu;
    public Texture2D cursor;

    void Awake()
    {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
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
