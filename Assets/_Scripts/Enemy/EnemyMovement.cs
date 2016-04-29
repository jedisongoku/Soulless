using UnityEngine;
using System.Collections;

public class EnemyMovement :MonoBehaviour
{

    public float aggroRange = 15f;
    public float chasingRange = 50f;
    public float chaseStopDistance = 2f;
    public bool canMove = true;
    public bool isInCombat = false;
    public bool isFirstAggro = true;

    private NavMeshAgent controller;
    private NavMeshObstacle obstacle;
    private Animator enemyAnimation;
    private GameObject player;
    private Vector3 initialPosition;
    private EnemyCombatManager combatManager;
    private PhotonView photonView;


    private bool isChasing = false;
    private bool immuneToAggro = false;
    private float idleTimer = 0f;
    private bool isMoving_Animation = false;
    
    private bool isTargetDead = false;
    private bool isHealthRegenerating = false;
    


    void Start()
    {
        controller = GetComponent<NavMeshAgent>();
        enemyAnimation = GetComponent<Animator>();
        combatManager = GetComponent<EnemyCombatManager>();
        photonView = GetComponent<PhotonView>();
        obstacle = GetComponent<NavMeshObstacle>();
        controller.avoidancePriority = Random.Range(0, 99);
        
        //player = GameObject.FindGameObjectWithTag("Player");
        initialPosition = transform.position;
        controller.stoppingDistance = chaseStopDistance;

        if(PhotonNetwork.isMasterClient)
        {
            StartCoroutine(Movement());
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

    IEnumerator Movement()
    {
        Invoke("IsPlayersInAggroRange", 1f); //Checks if any player is in range, and add them to the attack list

        if (combatManager.playerAttackList.Count != 0 & canMove)
        {
            if (isInCombat && !InAttackingRange())
            {
                controller.stoppingDistance = chaseStopDistance - Mathf.RoundToInt(chaseStopDistance * 0.25f);
                MoveToPosition(combatManager.playerAttackList[0].transform.position);  
                if(isFirstAggro)
                {
                    isFirstAggro = false;
                    AlertNearbyEnemies(combatManager.playerAttackList[0]);
                }
            }
            if(Vector3.Distance(transform.position, combatManager.playerAttackList[0].transform.position) <= PlayFabDataStore.catalogRunes[combatManager.selectRune].attackRange)
            {
                transform.LookAt(combatManager.playerAttackList[0].transform.position);
                controller.enabled = false;
                obstacle.enabled = true;
            }
            else
            {
                obstacle.enabled = false;
                controller.enabled = true;
                
            }
            
        }

        if ((combatManager.playerAttackList.Count == 0) && isInCombat)
        {
            obstacle.enabled = false;
            controller.enabled = true;
            chaseStopDistance = 0;
            controller.stoppingDistance = 0;
            isInCombat = false;
            isFirstAggro = true;
            isHealthRegenerating = true;
            MoveToPosition(initialPosition);
        }

        if(Vector3.Distance(transform.position, initialPosition) < 1 && isHealthRegenerating)
        {
            isHealthRegenerating = false;
            GetComponent<Health>().InitializeHealth();
        }

        yield return new WaitForSeconds(0.1f);

        if(!GetComponent<Health>().IsDead())
        {
            StartCoroutine(Movement());
        }
        
    }

    void IsPlayersInAggroRange()  
    {
        for (int i = 0; i < GameManager.players.Count; i++)
        {
            //added this check to prevent null reference exceptions when player leaves room
            if (GameManager.players[i] != null)
            {
                if (Vector3.Distance(transform.position, GameManager.players[i].transform.position) <= aggroRange)
                {
                    if (!combatManager.playerAttackList.Contains(GameManager.players[i]) && !GameManager.players[i].GetComponent<Health>().IsDead())
                    {
                        combatManager.playerAttackList.Add(GameManager.players[i]);
                    }
                }
                if (Vector3.Distance(transform.position, GameManager.players[i].transform.position) > aggroRange && !isInCombat)
                {
                    if (combatManager.playerAttackList.Contains(GameManager.players[i]))
                    {
                        combatManager.playerAttackList.Remove(GameManager.players[i]);
                    }
                }
            }
        }
        if (combatManager.playerAttackList.Count > 0)
        {
            chaseStopDistance = PlayFabDataStore.catalogRunes[GetComponent<EnemyCombatManager>().selectRune.ToString()].attackRange;
            isInCombat = true;
        }
    }

    bool InAttackingRange()
    {
        if(combatManager.playerAttackList.Count != 0)
        {
            if (Vector3.Distance(transform.position, combatManager.playerAttackList[0].transform.position) <= chaseStopDistance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
        
    }

    void MoveToPosition(Vector3 position)
    {
        if(controller.enabled)
        {
            photonView.RPC("SendMoveDestination", PhotonTargets.Others, photonView.viewID, position, controller.stoppingDistance);
            //transform.LookAt(position);
            controller.SetDestination(position);
        }
        
    }

    //Call this when you transfer ownership of the room, so enemies still function
    void OnMasterClientChanged()
    {
        StartCoroutine(Movement());
    }

    public void AlertNearbyEnemies(GameObject target)
    {
        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, 5, LayerMask.GetMask("Enemy"));
        foreach(var enemy in nearbyEnemies)
        {
            if (enemy.GetComponent<EnemyMovement>().isInCombat != true)
            {
                enemy.GetComponent<EnemyMovement>().isFirstAggro = false;
                enemy.GetComponent<EnemyMovement>().isInCombat = true;
                enemy.GetComponent<EnemyCombatManager>().playerAttackList.Add(target);
            }
            
        }
    }

}
