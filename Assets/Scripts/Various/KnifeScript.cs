using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeScript : MonoBehaviour
{
	[Tooltip("Damage of the bullet to gameObjects")]
	public int damage;
	public bool isActive;

	private Vector3 contactPos;
	[Header("Impact Effect Prefabs")]
	public Transform[] bloodImpactPrefabs;
	public Transform[] metalImpactPrefabs;
	public Transform[] dirtImpactPrefabs;
	public Transform[] concreteImpactPrefabs;

    private void OnTriggerEnter(Collider other)
    {
		if (isActive)
        {
			//Ignore collision if bullet collides with "Player" tag
			if (other.gameObject.tag == "Player")
			{
				//Physics.IgnoreCollision (collision.collider);
				Debug.LogWarning("Collides with player");
				//Physics.IgnoreCollision(GetComponent<Collider>(), GetComponent<Collider>());


				Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), GetComponent<Collider>());

			}

			if (other.gameObject.tag == "Enemy")
			{
				contactPos = other.ClosestPointOnBounds(new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z));
				//Instantiate random impact prefab from array
				Instantiate(bloodImpactPrefabs[Random.Range(0, bloodImpactPrefabs.Length)], transform.position, Quaternion.LookRotation(contactPos));
				if (other.gameObject.GetComponent<Target>().invulnerabilityTime <= 0)
				{
					other.transform.GetComponent<Target>().TakeDamage(damage);
					//Physics.IgnoreCollision(collision.collider, GetComponent<Collider>(), true);

				}
			}

			if (other.transform.tag == "Blood")
			{
				//Instantiate random impact prefab from array
				Instantiate(bloodImpactPrefabs[Random.Range
					(0, bloodImpactPrefabs.Length)], transform.position,
					other.gameObject.transform.rotation);
			}

			//If bullet collides with "Metal" tag
			if (other.transform.tag == "Metal")
			{
				contactPos = other.ClosestPointOnBounds(new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z));
				//Instantiate random impact prefab from array
				Instantiate(metalImpactPrefabs[Random.Range
					(0, bloodImpactPrefabs.Length)], transform.position,
					Quaternion.LookRotation(contactPos));
			}

			//If bullet collides with "Dirt" tag
			if (other.transform.tag == "Dirt")
			{
				contactPos = other.ClosestPointOnBounds(new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z));
				//Instantiate random impact prefab from array
				Instantiate(dirtImpactPrefabs[Random.Range
					(0, bloodImpactPrefabs.Length)], transform.position,
					Quaternion.LookRotation(contactPos));
			}

			//If bullet collides with "Concrete" tag
			if (other.transform.tag == "Concrete")
			{
				contactPos = other.ClosestPointOnBounds(new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z));
				//Instantiate random impact prefab from array
				Instantiate(concreteImpactPrefabs[Random.Range
					(0, bloodImpactPrefabs.Length)], transform.position,
					Quaternion.LookRotation(contactPos));
			}
        }
	}
}
