using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour {

    public Image enemyHealthFillImage;
    public Text enemyHealthText;

    public Shader defaultShader;
    public Shader outlineShader;

    public int health;
    public int maxHealth;
    public bool isFirstHit = true;

    private PhotonView photonView;
    private Animator anim;
    private bool dead = false;
    private float navMeshSpeed;

    public bool isBleeding = false;
    public int maxBleedCount = 0;
    public int bleedDamage = 0;
    public bool isCriticalHit = false;
    public int criticalHitValue = 0;
    public float maxCriticalHitTime = 0f;
    public bool isChilled = false;
    public float maxChillTime = 0f;
    public bool isFrozen = false;
    public float maxFreezeTime = 0f;
    public bool isStunned = false;
    public int stunChance = 0;
    public float maxStunTime = 0f;
    public int increasedDamagePercentage = 0;
    public bool isDamageReduced = false;
    public int reducedDamagePercentage = 0;
    public bool immuneToBeControlled = false;
    public float maxImmuneToControlTime = 0f;


    private float criticalHitTimer = 0f;
    private float chillTimer = 0f;
    private float freezeTimer = 0f;
    private float stunTimer = 0f;
    private float bleedTimer = 0f;
    private float immuneToControlTimer = 0f;

    private bool stunActivate = true;
    private bool chillActivate = true;
    private bool freezeActivate = true;
    private bool critActivate = false;

    private int bleedCount = 1;
    private int counter = 0;

    private float sinkSpeed = 0.5f;
    private bool isPlayerRespawned = false;

    void Awake()
    {

        anim = GetComponent<Animator>();
        photonView = GetComponent<PhotonView>();
        navMeshSpeed = GetComponent<NavMeshAgent>().speed;
    }

    void OnEnable()
    {

        if(photonView.isMine)
        {
            if (CompareTag("Player"))
            {
                Debug.Log("Subscribed to the event: " + gameObject.tag);
                HUD_Manager.OnRespawn += RespawnPlayer;
            }
        }
        
        
    }

    void OnDestroy()
    {
        if (photonView.isMine)
        {
            if (CompareTag("Player"))
            {
                Debug.Log("UNSubscribed to the event on Destroy: " + gameObject.tag);
                HUD_Manager.OnRespawn -= RespawnPlayer;
            }
        }
    }

    void OnDisable()
    {
        if (photonView.isMine)
        {
            if (CompareTag("Player"))
            {
                Debug.Log("UNSubscribed to the event on Destroy: " + gameObject.tag);
                HUD_Manager.OnRespawn -= RespawnPlayer;
            }
        }
    }

    void Start()
    {
        enemyHealthFillImage = HUD_Manager.hudManager.enemyHealth;
        enemyHealthText = HUD_Manager.hudManager.enemyHealthText;


        Invoke("InitializeHealth", 1);
        StartCoroutine("HealthUpdate");
        if (CompareTag("Player"))
        {
            Invoke("StartHealthRegeneration", 5);
        }

        

    }

    public void InitializeHealth()
    {
        if (photonView.isMine)
        {
            if (CompareTag("Player"))
            {
                maxHealth = PlayFabDataStore.playerMaxHealth;
                if(isPlayerRespawned)
                {
                    health = maxHealth / 2;
                    PlayFabDataStore.playerCurrentResource = 0;
                }
                else
                {
                    health = maxHealth;
                } 
                PlayFabDataStore.playerCurrentHealth = health;
                GetComponent<PlayerCombatManager>().canAttack = true;
                UpdateHealth();
            }
        }
        if(PhotonNetwork.player.isMasterClient)
        {
            if (CompareTag("Enemy"))
            {
                if (GetComponent<EnemyCombatManager>() != null)
                {
                    health = maxHealth;
                    isFirstHit = true;
                    UpdateHealth();
                }
            }
        }
        
    }

    void StartHealthRegeneration()
    {
        if(photonView.isMine)
        {
            StartCoroutine("HealthRegeneration");
        }
        
    }

    void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Invoke("UpdateHealth", 1);
    }

    public void UpdateHealth()
    {
        HUD_Manager.hudManager.SetHealthAndResource();
        photonView.RPC("SetHealth", PhotonTargets.AllViaServer, health, maxHealth);
    }

    public void CharacterStatsUpdateHealth(int _maxHealth)
    {
        photonView.RPC("SetHealth", PhotonTargets.AllViaServer, health, _maxHealth);
    }

    [PunRPC]
    public void SetHealth(int _health, int _maxHealth)
    {
        health = _health;
        maxHealth = _maxHealth;
        if(health <= 0)
        {
            Dead();
        }
    }

    [PunRPC]
    public void SetDamageReduction(int viewId, bool _isDamageReduced, int _reducedDamagePercentage, float _immuneToControlTime)
    {
        isDamageReduced = _isDamageReduced;
        reducedDamagePercentage = _reducedDamagePercentage;
        immuneToBeControlled = true;
        maxImmuneToControlTime = _immuneToControlTime;
        immuneToControlTimer = 0f;

    }

    [PunRPC]
    public void SetBleeding(int viewId, bool _isBleeding, int _maxBleedCount, int _bleedDamage)
    {
        Debug.Log("I AM BLEEDINGGGG!!!!");
        isBleeding = _isBleeding;
        maxBleedCount = _maxBleedCount;
        bleedDamage = _bleedDamage / _maxBleedCount;
        bleedTimer = 0f;
        bleedCount = 1;
    }

    [PunRPC]
    public void SetFreeze(int viewId, bool _isFrozen, float _maxFreezeTime)
    {
        isFrozen = _isFrozen;
        maxFreezeTime = _maxFreezeTime;
    }

    [PunRPC]
    public void SetStun(int viewId, bool _isStunned, float _maxStunTime)
    {
        isStunned = _isStunned;
        maxStunTime = _maxStunTime;

    }

    [PunRPC]
    public void SetChill(int viewId, bool _isChilled, float _maxChillTime, int _increasedDamagePercentage)
    {
        isChilled = _isChilled;
        maxChillTime = _maxChillTime;
        increasedDamagePercentage = _increasedDamagePercentage;
    }

    IEnumerator HealthRegeneration()
    {
        if (PlayFabDataStore.playerCurrentHealth + Mathf.CeilToInt(PlayFabDataStore.playerSpirit / 5) <= PlayFabDataStore.playerMaxHealth)
        {
            Debug.Log(Mathf.CeilToInt(PlayFabDataStore.playerSpirit / 5));
            PlayFabDataStore.playerCurrentHealth += Mathf.CeilToInt(PlayFabDataStore.playerSpirit / 5);
            health = PlayFabDataStore.playerCurrentHealth;
            
        }
        else
        {
            health = PlayFabDataStore.playerMaxHealth;
        }
        
        UpdateHealth();

        yield return new WaitForSeconds(1);
        
        if(!IsDead())
        {
            StartCoroutine(HealthRegeneration());
        }
        

    }

    IEnumerator HealthUpdate()
    {
        if (isDamageReduced)
        {
            if (immuneToControlTimer >= maxImmuneToControlTime)
            {
                isDamageReduced = false;
            }
        }


        if (isBleeding)
        {
            if (bleedTimer >= bleedCount)
            {
                Debug.Log("Bleed count: " + bleedCount);
                Debug.Log("Bleed Damage: " + bleedDamage);
                bleedCount++;

                TakeDamage(gameObject, bleedDamage, 0, "Natural");
                if (bleedCount > maxBleedCount)
                {
                    isBleeding = false;
                }
            }
        }

        if (isFrozen && !immuneToBeControlled)
        {
            if (freezeActivate)
            {
                freezeActivate = false;
                freezeTimer = 0f;
                GetComponent<NavMeshAgent>().speed = 0;
                anim.SetTrigger("FIGHT IDLE");

            }
            GetComponent<NavMeshAgent>().ResetPath();
            if (freezeTimer >= maxFreezeTime)
            {
                GetComponent<NavMeshAgent>().speed = navMeshSpeed;
                isFrozen = false;
                freezeActivate = true;
                freezeTimer = 0f;
            }

        }

        if (isStunned && !immuneToBeControlled)
        {
            if (stunActivate)
            {
                stunActivate = false;
                stunTimer = 0f;
                GetComponent<NavMeshAgent>().speed = 0;
                anim.SetTrigger("STUN");

                if (CompareTag("Player"))
                {
                    if (!dead)
                    {
                        GetComponent<PlayerCombatManager>().canAttack = false;
                    }
                }
                if (CompareTag("Enemy"))
                {
                    if (!dead)
                    {
                        if (GetComponent<PlayerCombatManager>() != null)
                        {
                            GetComponent<PlayerCombatManager>().canAttack = false;
                        }
                        else
                        {
                            GetComponent<EnemyCombatManager>().canAttack = false;
                            GetComponent<EnemyMovement>().canMove = false;
                            //GetComponent<EnemyCombatManager>().enabled = false;
                            //GetComponent<EnemyMovement>().enabled = false;
                        }
                    }
                }
            }
            if(GetComponent<NavMeshAgent>().enabled)
            {
                GetComponent<NavMeshAgent>().ResetPath();
            }
            
            if (stunTimer >= maxStunTime)
            {
                if (CompareTag("Player"))
                {
                    if (!dead)
                    {
                        GetComponent<PlayerCombatManager>().canAttack = true;
                    }

                }
                if (CompareTag("Enemy"))
                {
                    if (!dead)
                    {
                        if (GetComponent<PlayerCombatManager>() != null)
                        {
                            GetComponent<PlayerCombatManager>().canAttack = true;
                        }
                        else
                        {
                            GetComponent<EnemyCombatManager>().canAttack = true;
                            GetComponent<EnemyMovement>().canMove = true;
                            //GetComponent<EnemyCombatManager>().enabled = true;
                            //GetComponent<EnemyMovement>().enabled = true;
                        }

                    }
                }
                GetComponent<NavMeshAgent>().speed = navMeshSpeed;
                anim.SetTrigger("FIGHT IDLE");
                isStunned = false;
                stunActivate = true;
            }
        }


        if (isChilled)
        {
            if (chillActivate)
            {
                chillActivate = false;
                chillTimer = 0f;
            }
            if (chillTimer >= maxChillTime)
            {
                isChilled = false;
            }
        }

        if (isCriticalHit)
        {
            if (critActivate)
            {
                critActivate = false;
                criticalHitTimer = 0f;
            }
            if (criticalHitTimer >= maxCriticalHitTime)
            {
                isCriticalHit = false;
                critActivate = true;
                criticalHitValue = 0;
            }
        }

        yield return new WaitForSeconds(0.1f);

        StartCoroutine("HealthUpdate");
    }

    void Update()
    {
        chillTimer += Time.deltaTime;
        criticalHitTimer += Time.deltaTime;
        freezeTimer += Time.deltaTime;
        stunTimer += Time.deltaTime;
        bleedTimer += Time.deltaTime;
        immuneToControlTimer += Time.deltaTime;

        anim.SetFloat("MOVE", GetComponent<NavMeshAgent>().velocity.magnitude / GetComponent<NavMeshAgent>().speed);
    }

    public void TakeDamage(GameObject source, int damageTaken, float criticalChance, string damageType)
    {
        if (!dead)
        {
            if (isCriticalHit)
            {
                criticalChance += criticalHitValue;
            }
            if (isChilled)
            {
                damageTaken += increasedDamagePercentage;
            }
            if (Random.Range(0f, 100f) <= criticalChance + criticalHitValue)
            {
                damageTaken *= 2; //if it's a critical, double the damage
            }
            if(isDamageReduced)
            {
                damageTaken -= damageTaken * reducedDamagePercentage / 100;
            }
        }

        photonView.RPC("ApplyDamageTaken", PhotonTargets.AllViaServer, photonView.viewID, damageTaken, source.GetComponent<PhotonView>().viewID);
    }

    [PunRPC]
    void ApplyDamageTaken(int sourceId, int damageTaken, int playerID)
    {
        if(!dead)
        {
            if (photonView.viewID == sourceId)
            {
                if (CompareTag("Enemy"))
                {
                    if (health > damageTaken)
                    {
                        //Debug.Log(gameObject + " takes " + damageTaken + " damage");
                        if(GetComponent<EnemyCombatManager>() != null)
                        {
                            if(isFirstHit && GetComponent<EnemyMovement>().isInCombat != true)
                            {
                                isFirstHit = false;
                                GameObject source = PhotonView.Find(playerID).gameObject;
                                if(source.CompareTag("Player"))
                                {
                                    Debug.Log("ApplyDamageTaken tag " + source.tag);
                                    GetComponent<EnemyMovement>().isInCombat = true;
                                    GetComponent<EnemyCombatManager>().playerAttackList.Add(source);
                                    GetComponent<EnemyMovement>().AlertNearbyEnemies(source);
                                }
                                
                            }     
                        }
                        health -= damageTaken;
                    }
                    else
                    {
                        health = 0;
                        Dead();
                    }
                }
                if (CompareTag("Player"))
                {
                    if (PlayFabDataStore.playerCurrentHealth > damageTaken)
                    {
                        health -= damageTaken;
                        PlayFabDataStore.playerCurrentHealth -= damageTaken;
                    }
                    else
                    {
                        health = 0;
                        PlayFabDataStore.playerCurrentHealth = 0;
                        Dead();
                    }
                }
            }
        }
        
        
    }
  
    void Dead()
    {
        dead = true;
        anim.SetTrigger("DIE");
        StopCoroutine("HealthUpdate");
        if (CompareTag("Player"))
        {
            GetComponent<PlayerCombatManager>().enabled = false;
            StopCoroutine("HealthRegeneration");
            Debug.Log("IM DEAAAADDDD");
            HUD_Manager.hudManager.ToggleRespawnWindow();

        }
        if(CompareTag("Enemy"))
        {
            if(GetComponent<PlayerCombatManager>() != null)
            {
                GetComponent<PlayerCombatManager>().enabled = false;
            }
            else
            {
                Invoke("SinkEnemy", 5);
                Leveling.GrantExperince();
                GetComponent<EnemyMovement>().enabled = false;
                GetComponent<EnemyCombatManager>().enabled = false;
                if(PhotonNetwork.isMasterClient)
                {
                    GetComponent<DropItem>().GetDropItemId();
                    Invoke("DestroyEnemy", 7);
                }
                
            } 
        }
        GetComponent<NavMeshAgent>().enabled = false; 
        //GetComponent<Health>().enabled = false;       
        GetComponent<CapsuleCollider>().enabled = false;
    }
    void OnMouseOver()
    {
        if (CompareTag("Enemy"))
        {
            if (!dead)
            {
                enemyHealthFillImage.fillAmount = (float)health / (float)maxHealth;
                enemyHealthFillImage.transform.parent.gameObject.SetActive(true);
                enemyHealthText.text = health + "/" + maxHealth;

                GetComponentInChildren<SkinnedMeshRenderer>().material.shader = outlineShader;
            }

        }
    }

    void OnMouseExit()
    {
        if (CompareTag("Enemy"))
        {
            enemyHealthFillImage.transform.parent.gameObject.SetActive(false);
            GetComponentInChildren<SkinnedMeshRenderer>().material.shader = defaultShader;
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public bool IsDead()
    {
        return dead;
    }

    void RespawnPlayer()
    {
        if(photonView.isMine)
        {
            StartCoroutine(Respawn());
        }
        
    }

    IEnumerator Respawn()
    {
        isPlayerRespawned = true;
        gameObject.transform.position = InitializerScript.initializer.respawnPoint.position;
        photonView.RPC("SendPosition", PhotonTargets.Others, photonView.viewID, gameObject.transform.position);
        photonView.RPC("SendTrigger", PhotonTargets.AllViaServer, photonView.viewID, "RESPAWN");
        InitializeHealth();

        yield return new WaitForSeconds(1);

        photonView.RPC("RespawnOverNetwork", PhotonTargets.All, photonView.viewID);
        GetComponent<PlayerCombatManager>().enabled = true;
        GetComponent<PlayerCombatManager>().canMove = true;
        GetComponent<PlayerCombatManager>().canAttack = true;

        StartCoroutine("HealthRegeneration", 3);

    }
    [PunRPC]
    void RespawnOverNetwork(int viewID)
    {
        if(photonView.viewID == viewID)
        {
            dead = false;
            GetComponent<CapsuleCollider>().enabled = true;
            GetComponent<NavMeshAgent>().enabled = true;
            StartCoroutine("HealthUpdate");
            //GetComponent<Health>().enabled = true;
        }

    }

    void SinkEnemy()
    {
        StartCoroutine(Sink());
    }
    IEnumerator Sink()
    {
        transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);

        yield return new WaitForSeconds(0);

        StartCoroutine(Sink());
    }

    void DestroyEnemy()
    {
        PhotonNetwork.Destroy(gameObject);

    }

    /*
    [PunRPC]
    void GrantExperience()
    {
       if(photonView.isMine)
        {
            if(CompareTag("Player"))
            {
                Leveling.GrantExperince();
            }
        }
    }*/
}
