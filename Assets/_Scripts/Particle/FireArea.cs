using UnityEngine;
using System.Collections;

public class FireArea : MonoBehaviour {

    public int weaponDamage;
    public string damageType;
    public PhotonView photonView;

    void Awake()
    {
        //transform.Rotate(new Vector3(90, 0, 0));
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter from the fire ground");
        if(other.CompareTag("Enemy"))
        {
            Debug.Log("Trigger Enter from the fire ground in the enemy tag");
            other.gameObject.GetComponent<PhotonView>().RPC("SetBleeding", PhotonTargets.AllViaServer, photonView.viewID, true, (int)PlayFabDataStore.catalogRunes["Rune_RainOfFire"].effectTime, weaponDamage * PlayFabDataStore.catalogRunes["Rune_RainOfFire"].increasedDamage / 100);
        }
    }
}
