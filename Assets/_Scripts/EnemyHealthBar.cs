using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealthBar : MonoBehaviour {

    public Image enemyHealthFillImage;
    public Text enemyHealthText;

    private int health;
    private int maxHealth;
    // Use this for initialization

    void Start()
    {
        enemyHealthFillImage = HUD_Manager.hudManager.enemyHealth;
        enemyHealthText = HUD_Manager.hudManager.enemyHealthText;
    }

    void Update()
    {
        HUD_Manager.hudManager.healthGlobe.fillAmount = (float)GetComponent<Health>().health / (float)GetComponent<Health>().maxHealth;
        HUD_Manager.hudManager.resourceGlobe.fillAmount = (float)PlayFabDataStore.playerCurrentResource / (float)PlayFabDataStore.playerMaxResource;
        HUD_Manager.hudManager.playerHealthText.text = GetComponent<Health>().health + "/" + GetComponent<Health>().maxHealth;
    }

    void OnMouseOver()
    {
        if (tag == "Enemy")
        {
            if (!GetComponent<Health>().IsDead())
            {
                enemyHealthFillImage.fillAmount = (float)GetComponent<Health>().health / (float)GetComponent<Health>().maxHealth;
                enemyHealthFillImage.transform.parent.gameObject.SetActive(true);
                enemyHealthText.text = GetComponent<Health>().health + "/" + GetComponent<Health>().maxHealth;
            }
            else
            {
                enemyHealthFillImage.transform.parent.gameObject.SetActive(false);
            }

        }
    }

    void OnMouseExit()
    {
        if (tag == "Enemy")
        {
            enemyHealthFillImage.transform.parent.gameObject.SetActive(false);
        }
    }
}
