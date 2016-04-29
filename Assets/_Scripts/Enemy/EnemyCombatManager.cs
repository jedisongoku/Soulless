using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyCombatManager : MonoBehaviour
{
    public string selectRune;
    public int weaponDamage = 1;
    public float attackSpeed = 2;
    public Transform spellStartLocation;
    public Transform spellTargetLocation;
    public bool canAttack = true;

    public List<GameObject> playerAttackList;

    private Animator enemyAnimation;
    private float criticalChance = 0;
    private PhotonView photonView;
    private GameObject targetPlayer;

    void Start()
    {
        enemyAnimation = GetComponent<Animator>();
        playerAttackList = new List<GameObject>();
        photonView = GetComponent<PhotonView>();

        if (PhotonNetwork.isMasterClient)
        {
            StartCoroutine(EnemyAttack());
        }
    }

    IPunCallbacks OnMasterClientSwitched()
    {
        if (PhotonNetwork.isMasterClient)
        {
            OnMasterClientChanged();
        }
        return null;
    }

    IEnumerator EnemyAttack()
    {
        if (playerAttackList.Count != 0)
        {
            if (!playerAttackList[0].GetComponent<Health>().IsDead() && InAttackingRange() && canAttack)
            {
                targetPlayer = playerAttackList[0];
                Invoke(selectRune, 0);
            }

            if (playerAttackList[0].GetComponent<Health>().IsDead())
            {
                playerAttackList.RemoveAt(0);
            }
        }

        yield return new WaitForSeconds(attackSpeed);

        if (!GetComponent<Health>().IsDead())
        {
            StartCoroutine(EnemyAttack());
        }
        
    }

    bool InAttackingRange()
    {
        if (Vector3.Distance(transform.position, playerAttackList[0].transform.position) <= PlayFabDataStore.catalogRunes[selectRune].attackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Call this when you transfer ownership of the room, so enemies still function
    void OnMasterClientChanged()
    {
        StartCoroutine(EnemyAttack());
    }

    /// <summary>
    /// Hit an enemy for 320% physical damage.
    /// </summary>
    public void Rune_Slam()
    {
        
        photonView.RPC("SendTrigger", PhotonTargets.All, photonView.viewID, "ATTACK 1");
        targetPlayer.GetComponent<Health>().TakeDamage(gameObject, weaponDamage * PlayFabDataStore.catalogRunes[selectRune].attackPercentage / 100, criticalChance, PlayFabDataStore.catalogRunes[selectRune].damageType);
    }

    /// <summary>
    /// Hit an enemy for 250% physical damage.
    /// </summary>
    public void Rune_MagicBolt()
    {
        photonView.RPC("SendTrigger", PhotonTargets.All, photonView.viewID, "ATTACK SPELL");
        targetPlayer.GetComponent<Health>().TakeDamage(gameObject, weaponDamage * PlayFabDataStore.catalogRunes[selectRune.ToString()].attackPercentage / 100, criticalChance, PlayFabDataStore.catalogRunes[selectRune].damageType);
        photonView.RPC("InstantiateParticleEffects", PhotonTargets.All, photonView.viewID, "MagicBolt", spellStartLocation.position, Quaternion.identity, targetPlayer.GetComponent<PhotonView>().viewID, false);
    }
}
