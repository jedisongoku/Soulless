using UnityEngine;
using System.Collections;

namespace MagicalFX
{
	public class FX_HitSpawner : MonoBehaviour
	{


		public GameObject FXSpawn;
		public bool DestoyOnHit = false;
		public bool FixRotation = false;
		public float LifeTimeAfterHit = 1;
		public float LifeTime = 0;
        private PhotonView photonview;
	
		void Start ()
		{
            photonview = GetComponent<PhotonView>();
		}
	
		void Spawn ()
		{
			if (FXSpawn != null) {
				Quaternion rotate = this.transform.rotation;
				if (!FixRotation)
					rotate = FXSpawn.transform.rotation;
				GameObject fx = (GameObject)GameObject.Instantiate (FXSpawn, this.transform.position, rotate);
				if (LifeTime > 0)
					Destroy (fx.gameObject, LifeTime);
			}
			if (DestoyOnHit) {

				Destroy (this.gameObject, LifeTimeAfterHit);
				if (this.gameObject.GetComponent<Collider>())
					this.gameObject.GetComponent<Collider>().enabled = false;

			}
		}
	
		void OnTriggerEnter (Collider other)
		{
            if(GetComponent<ParticleFollowTarget>() != null)
            {
                if (other.transform.gameObject == GetComponent<ParticleFollowTarget>().GetTarget())
                {
                    Spawn();
                }
            }
            else
            {
                Spawn();
            }
            

		}
	
		void OnCollisionEnter (Collision collision)
		{
            if(GetComponent<ParticleFollowTarget>() != null)
            {
                if (collision.transform.gameObject == GetComponent<ParticleFollowTarget>().GetTarget())
                {
                    Spawn();
                }
            }
            else
            {
                Spawn();
            }
            
        }
	}
}