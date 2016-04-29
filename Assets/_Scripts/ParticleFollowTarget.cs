using UnityEngine;
using System.Collections;

public class ParticleFollowTarget : MonoBehaviour {

    private GameObject target;
    Transform targetTrans;
    float speed = 2;
	
	void Update ()
    {
        transform.LookAt(targetTrans);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void SetTarget(GameObject _target)
    {
        target = _target;
        if(_target.GetComponent<EnemyCombatManager>() != null)
        {
            targetTrans = _target.GetComponent<EnemyCombatManager>().spellTargetLocation;
        }
        else
        {
            targetTrans = _target.GetComponent<PlayerCombatManager>().spellTargetLocation;
        }
    }

    public GameObject GetTarget()
    {
        return target;
    }
}
