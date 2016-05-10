using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawnManager : MonoBehaviour
{

    public enum enemyList { Orc_Melee, Orc_Ranged, Goblin_Melee, Goblin_Ranged, DarkElf_Melee, DarkElf_Ranged, Skeleton_Melee, Skeleton_Ranged}; //List of Enemies
    public enum runeList { Rune_Slam, Rune_MagicBolt}; // List of enemy skills
    public List<enemyList> enemies; // Enemies selected in the editor for spawning
    public List<runeList> runes; // Skills for the selected enemies

    private BoxCollider spawnArea; // Enemy Spawn area defined by a collider
    private Vector3 spawnAreaSize;
    private Vector3 spawnLocation;
    private int selector = 0;

    void Awake()
    {
        spawnArea = GetComponent<BoxCollider>();
        spawnAreaSize = spawnArea.size;
        if (PhotonNetwork.isMasterClient)
        {
            StartCoroutine("Spawn"); // Enemy spawn only called by the master
        }
    }

    // Enemies defined in the editor being spawned one by one
    IEnumerator Spawn()
    {

        spawnLocation = new Vector3(Random.Range(spawnArea.transform.position.x - spawnArea.size.x / 2, spawnArea.transform.position.x + spawnArea.size.x / 2), spawnArea.transform.position.y,
            Random.Range(spawnArea.transform.position.z - spawnArea.size.z / 2, spawnArea.transform.position.z + spawnArea.size.z / 2));
        GameObject spawnEnemy = PhotonNetwork.InstantiateSceneObject(enemies[selector].ToString(), spawnLocation, Random.rotation, 0, null);
        spawnEnemy.GetComponent<EnemyCombatManager>().selectRune = runes[selector].ToString();
                

        yield return new WaitForSeconds(Random.Range(0f, 1.5f));

        selector++;

        // Run the coroutine until all the enemies are spawned for the specific area
        if(selector < enemies.Count)
        {
            StartCoroutine("Spawn");
        }
        else
        {
            StopCoroutine("Spawn");
        }

        
    }
}