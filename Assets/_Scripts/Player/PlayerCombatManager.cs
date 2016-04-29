using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerCombatManager : Runes
{

    public bool canMove = true; // UI clicks prevent player from moving
    public bool isInCombat = false;
    public bool canAttack = true;
    
    //sets by PlayerAttack script


    //private int skillAttackRange;
    private string actionBarSkillId;


    private float skill_1_Timer = 0f;
    private int resource = 0;
    private float idleTimer = 0f;
    private bool isMoving = false;
    private int skillSlot = 0;
    private bool autoAttack = false;


    void OnEnable()
    {
        targetEnemy = null;
    }

    void Update()
    {
        //Primary Skill
        if ((Input.GetMouseButtonDown(0) || actionBarSkillId == "LC") && canMove && canAttack)
        {
            skillSlot = 5;

            locatePosition(); //Find the clicked position and check if enemy clicked

            if (PlayFabDataStore.playerActiveSkillRunes.ContainsKey(skillSlot))
            {
                stopDistanceForAttack = PlayFabDataStore.catalogRunes[PlayFabDataStore.playerActiveSkillRunes[5]].attackRange;
                Invoke(PlayFabDataStore.playerActiveSkillRunes[skillSlot], 0);
                actionBarSkillId = null;
            }

        }

        //Secondary Skill
        if ((Input.GetMouseButtonDown(1) || actionBarSkillId == "RC") && canMove && canAttack)
        {
            skillSlot = 6;

            locatePosition();

            if (PlayFabDataStore.playerActiveSkillRunes.ContainsKey(skillSlot))
            {
                Invoke(PlayFabDataStore.playerActiveSkillRunes[skillSlot], 0);
                actionBarSkillId = null;
            }          
            
        }
        //Action Bar #1
        if (Input.GetKeyDown("1") || actionBarSkillId == "1")
        {
            skillSlot = 1;

            if (PlayFabDataStore.playerActiveSkillRunes.ContainsKey(skillSlot))
            {
                Invoke(PlayFabDataStore.playerActiveSkillRunes[skillSlot], 0);
                actionBarSkillId = null;
            }


        }

        if(Input.GetKeyDown("2") || actionBarSkillId == "2")
        {
            skillSlot = 2;

            if (PlayFabDataStore.playerActiveSkillRunes.ContainsKey(skillSlot))
            {
                Invoke(PlayFabDataStore.playerActiveSkillRunes[skillSlot], 0);
                actionBarSkillId = null;
            }
        }
        if(Input.GetKeyDown("3") || actionBarSkillId == "3")
        {
            foreach(var player in GameManager.players)
            {
                Debug.Log("Player : " + player.name);
            }
            skillSlot = 3;

            if (PlayFabDataStore.playerActiveSkillRunes.ContainsKey(skillSlot))
            {
                Invoke(PlayFabDataStore.playerActiveSkillRunes[skillSlot], 0);
                actionBarSkillId = null;
            }
        }

        if (Input.GetKeyDown("4") || actionBarSkillId == "4")
        {
            skillSlot = 4;

            if (PlayFabDataStore.playerActiveSkillRunes.ContainsKey(skillSlot))
            {
                Invoke(PlayFabDataStore.playerActiveSkillRunes[skillSlot], 0);
                actionBarSkillId = null;
            }
        }

        //playerAnimation.SetFloat("MOVE", controller.velocity.magnitude / controller.speed);

        if (targetEnemy != null && Vector3.Distance(targetEnemy.transform.position, transform.position) > stopDistanceForAttack)
        {
            if (targetEnemy.CompareTag("Enemy"))
            {
                position = targetEnemy.transform.position;
                MoveToPosition();
            }
        }
    }
    
    void locatePosition()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000))
        {
            if (hit.transform.tag == "Enemy")
            {
                targetEnemy = hit.transform.gameObject;
                position = hit.transform.position;
                controller.stoppingDistance = stopDistanceForAttack;
                isInCombat = true;
            }
           
            else
            {
                targetEnemy = null;
                position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                controller.stoppingDistance = 0f;
            }
        }

        if(targetEnemy == null || Vector3.Distance(targetEnemy.transform.position, transform.position) > PlayFabDataStore.catalogRunes[PlayFabDataStore.playerActiveSkillRunes[skillSlot]].attackRange)
        {
            MoveToPosition();
        }
    }


    void MoveToPosition()
    {
        photonView.RPC("SendMoveDestination", PhotonTargets.Others, photonView.viewID, position, controller.stoppingDistance);
        transform.LookAt(position);
        if(controller.enabled == true)
        {
            Debug.Log("MOVE TO POSITION");
            controller.SetDestination(position);
        }
        
    }


    public void OnButtonClick(string id)
    {
        actionBarSkillId = id;
    }

    [PunRPC]
    void AddPlayer(int viewID)
    {
        if (photonView.isMine)
        {
            GameObject player = PhotonView.Find(viewID).gameObject;
            GameManager.players.Add(player);
            Debug.Log("PlayerAdded for GameManager");
            Debug.Log(GameManager.players.Count);
        }
    }

}
