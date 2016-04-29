using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;


public class HUD_Manager : MonoBehaviour {

    public delegate void RespawnAction();
    public static event RespawnAction OnRespawn;
    

    public static HUD_Manager hudManager;

    public Canvas characterWindow;
    public Canvas runeWindow;
    public Canvas cheatWindow;
    public Canvas friendsWindow;
    public Canvas questWindow;
    public Canvas optionWindow;
    public Canvas respawnWindow;
    public Canvas loading;

    public Image healthGlobe;
    public Image resourceGlobe;
    public Image enemyHealth;
    public Image experienceImage;
    public Text enemyHealthText;
    public Text playerHealthText;
    public Text playerResourceText;
    public Text experienceText;
    public Text playerLevelText;

    public Image ActionBarCooldownImage1;
    public Image ActionBarCooldownImage2;
    public Image ActionBarCooldownImage3;
    public Image ActionBarCooldownImage4;

    public Image ActionBarDisabledImage1;
    public Image ActionBarDisabledImage2;
    public Image ActionBarDisabledImage3;
    public Image ActionBarDisabledImage4;
    public Image ActionBarDisabledImage6;

    public Text playerName;

    

    public void SetHealthAndResource()
    {
        healthGlobe.fillAmount = (float)PlayFabDataStore.playerCurrentHealth / (float)PlayFabDataStore.playerMaxHealth;
        resourceGlobe.fillAmount = (float)PlayFabDataStore.playerCurrentResource / (float)PlayFabDataStore.playerMaxResource;
        experienceImage.fillAmount = (float)PlayFabDataStore.playerExperience / (float)PlayFabDataStore.maxExperienceToLevel;
        playerHealthText.text = PlayFabDataStore.playerCurrentHealth + "/" + PlayFabDataStore.playerMaxHealth;
        playerResourceText.text = PlayFabDataStore.playerCurrentResource + "/" + PlayFabDataStore.playerMaxResource;
        experienceText.text = PlayFabDataStore.playerExperience + "/" + PlayFabDataStore.maxExperienceToLevel;

    }

    void Update()
    {
        SetHealthAndResource();

        if (PlayFabDataStore.playerActiveSkillRunes.ContainsKey(1))
        {
            if (PlayFabDataStore.playerCurrentResource < PlayFabDataStore.catalogRunes[PlayFabDataStore.playerActiveSkillRunes[1]].resourceUsage)
            {
                ActionBarDisabledImage1.enabled = true;
            }
            else
            {
                ActionBarDisabledImage1.enabled = false;
            }
        }
        if (PlayFabDataStore.playerActiveSkillRunes.ContainsKey(2))
        {
            if (PlayFabDataStore.playerCurrentResource < PlayFabDataStore.catalogRunes[PlayFabDataStore.playerActiveSkillRunes[2]].resourceUsage)
            {
                ActionBarDisabledImage2.enabled = true;
            }
            else
            {
                ActionBarDisabledImage2.enabled = false;
            }
        }
        if (PlayFabDataStore.playerActiveSkillRunes.ContainsKey(3))
        {
            if (PlayFabDataStore.playerCurrentResource < PlayFabDataStore.catalogRunes[PlayFabDataStore.playerActiveSkillRunes[3]].resourceUsage)
            {
                ActionBarDisabledImage3.enabled = true;
            }
            else
            {
                ActionBarDisabledImage3.enabled = false;
            }
        }
        if (PlayFabDataStore.playerActiveSkillRunes.ContainsKey(4))
        {
            if (PlayFabDataStore.playerCurrentResource < PlayFabDataStore.catalogRunes[PlayFabDataStore.playerActiveSkillRunes[4]].resourceUsage)
            {
                ActionBarDisabledImage4.enabled = true;
            }
            else
            {
                ActionBarDisabledImage4.enabled = false;
            }
        }
        if (PlayFabDataStore.playerActiveSkillRunes.ContainsKey(6))
        {
            if (PlayFabDataStore.playerCurrentResource < PlayFabDataStore.catalogRunes[PlayFabDataStore.playerActiveSkillRunes[6]].resourceUsage)
            {
                ActionBarDisabledImage6.enabled = true;
            }
            else
            {
                ActionBarDisabledImage6.enabled = false;
            }
        }
    }

    void OnEnable()
    {
        SetPlayerNameOnUnitFrame();
        hudManager = this;
    }
	
    public void SetPlayerNameOnUnitFrame()
    {
        playerName.text = PlayFabDataStore.characterName;
    }
	public void ToggleCharacterWindow()
    {
        GetComponent<RaycastUI>().OnMouseExit();
        characterWindow.gameObject.SetActive(!characterWindow.gameObject.activeInHierarchy);
    }

    public void ToggleRuneWindow()
    {
        GetComponent<RaycastUI>().OnMouseExit();
        runeWindow.gameObject.SetActive(!runeWindow.gameObject.activeInHierarchy);
        
    }

    public void ToggleCheatPanel()
    {
        GetComponent<RaycastUI>().OnMouseExit();
        cheatWindow.gameObject.SetActive(!cheatWindow.gameObject.activeInHierarchy);
    }

    public void ToggleFriendsList()
    {
        GetComponent<RaycastUI>().OnMouseExit();
        friendsWindow.gameObject.SetActive(!friendsWindow.gameObject.activeInHierarchy);
    }

    public void ToggleQuestWindow()
    {
        GetComponent<RaycastUI>().OnMouseExit();
        questWindow.gameObject.SetActive(!questWindow.gameObject.activeInHierarchy);
    }

    public void ToggleOptionWindow()
    {
        GetComponent<RaycastUI>().OnMouseExit();
        optionWindow.gameObject.SetActive(!optionWindow.gameObject.activeInHierarchy);
    }

    public void Logout()
    {
        PhotonNetwork.LoadLevel("Login");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ReturnToGame()
    {
        GetComponent<RaycastUI>().OnMouseExit();
        optionWindow.gameObject.SetActive(false);
    }

    public void ToggleRespawnWindow()
    {
        GetComponent<RaycastUI>().OnMouseExit();
        respawnWindow.gameObject.SetActive(!respawnWindow.gameObject.activeInHierarchy);
    }

    public void RespawnWindowConfirm()
    {
        if(OnRespawn != null)
        {
            OnRespawn();
        }
    }

    public void ShowLoading(int time)
    {
        loading.GetComponentInChildren<LoadingBar>().Duration = time;
        loading.GetComponentInChildren<LoadingBar>().enableTweenFinished = true;
        loading.gameObject.SetActive(true);
    }

}
