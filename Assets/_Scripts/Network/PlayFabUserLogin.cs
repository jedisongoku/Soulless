using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabUserLogin : MonoBehaviour
{

    public InputField loginUsernameField;
    public InputField loginPasswordField;
    public InputField registerUsernameField;
    public InputField registerPasswordField;
    public InputField registerEmailField;
    public Text authenticationText;
    public Button authenticationButton;
    public Texture2D cursorImage;

    public static PlayFabUserLogin playfabUserLogin;
    public Canvas mainMenu;
    public GameObject registerFlag;

    void Awake()
    {
        playfabUserLogin = this;
        Cursor.SetCursor(cursorImage, Vector2.zero, CursorMode.Auto);
    }

    public void Login()
    {
        PlayFabApiCalls.PlayFabLogin(loginUsernameField.text, loginPasswordField.text);
        authenticationText.text = "CONNECTING...";
        authenticationText.transform.parent.gameObject.SetActive(true);
    }

    public void Register()
    {
        PlayFabApiCalls.PlayFabRegister(registerUsernameField.text, registerPasswordField.text, registerEmailField.text);
        authenticationText.text = "REGISTERING...";
        authenticationText.transform.parent.gameObject.SetActive(true);
    }

    public void CreateAccount()
    {
        registerFlag.SetActive(true);
    }

    public void Authentication(string text, int code)
    {
        authenticationText.text = text;
        if(code == 1) // Code 1: Authenticating and enables mainmenu at the back to connect playfab
        {
            mainMenu.gameObject.SetActive(true);
        }
        if(code == 2) // Code 2: Successfully connected PhotonServer and will go to mainmenu
        {
            Invoke("LoginSuccess", 1);
        }
        if(code == 3) // Code 3: Error. Will show the error code and stay on login.
        {
            authenticationButton.gameObject.SetActive(true);
        }
    }

    public void AuthenticationButtonClick()
    {
        authenticationButton.gameObject.SetActive(false);
        authenticationText.transform.parent.gameObject.SetActive(false);
    }

    void LoginSuccess()
    {
        authenticationText.transform.parent.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

   
}
