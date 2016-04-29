using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawnManager : MonoBehaviour
{

    public enum enemyList { Orc_Melee, Orc_Ranged, Goblin_Melee, Goblin_Ranged, DarkElf_Melee, DarkElf_Ranged, Skeleton_Melee, Skeleton_Ranged };
    public enum runeList { Rune_Slam, Rune_MagicBolt };
    public List<enemyList> enemies;
    public List<runeList> runes;

    private BoxCollider spawnArea;
    private Vector3 spawnAreaSize;
    private Vector3 spawnLocation;
    private int selector = 0;

    void Awake()
    {
        spawnArea = GetComponent<BoxCollider>();
        spawnAreaSize = spawnArea.size;
        if (PhotonNetwork.isMasterClient)
        {
            StartCoroutine("Spawn");
        }
    }

    IEnumerator Spawn()
    {

        spawnLocation = new Vector3(Random.Range(spawnArea.transform.position.x - spawnArea.size.x / 2, spawnArea.transform.position.x + spawnArea.size.x / 2), spawnArea.transform.position.y,
            Random.Range(spawnArea.transform.position.z - spawnArea.size.z / 2, spawnArea.transform.position.z + spawnArea.size.z / 2));
        GameObject spawnEnemy = PhotonNetwork.InstantiateSceneObject(enemies[selector].ToString(), spawnLocation, Random.rotation, 0, null);
        spawnEnemy.GetComponent<EnemyCombatManager>().selectRune = runes[selector].ToString();
                

        yield return new WaitForSeconds(Random.Range(0f, 1.5f));

        selector++;

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