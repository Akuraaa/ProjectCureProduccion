using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    public bool isActive;
    public int damage;

	[Header("Impact Effect Prefabs")]
	public Transform[] bloodImpactPrefabs;
	public Transform[] metalImpactPrefabs;
	public Transform[] dirtImpactPrefabs;
	public Transform[] concreteImpactPrefabs;

    private void Start()
    {
		this.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
	{
		//if (isActive)
  //      {

		if (collision.gameObject.name =="Pito")
        {
			Debug.Log("toque un pitoxd");
		}
		if (collision.gameObject.tag == "Player")
		{
			Physics.IgnoreCollision(collision.collider, GetComponent<BoxCollider>(), true);
		}

		if (collision.gameObject.tag == "Enemy")
		{
			Instantiate(bloodImpactPrefabs[Random.Range(0, bloodImpactPrefabs.Length)], transform.position, Quaternion.LookRotation(collision.contacts[0].normal));

			if (collision.gameObject.GetComponent<Target>().invulnerabilityTime <= 0)
			{
				collision.transform.GetComponent<Target>().TakeDamage(damage);
				//collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll | RigidbodyConstraints.FreezeRotation;
				//StartCoroutine(ChangeConstrains());
				//collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

			}
		}

		if (collision.transform.tag == "Metal")
		{
			//Instantiate random impact prefab from array
			Instantiate(metalImpactPrefabs[Random.Range
				(0, bloodImpactPrefabs.Length)], transform.position,
				Quaternion.LookRotation(collision.contacts[0].normal));
		}

		if (collision.transform.tag == "Dirt")
		{
			//Instantiate random impact prefab from array
			Instantiate(dirtImpactPrefabs[Random.Range
				(0, bloodImpactPrefabs.Length)], transform.position,
				Quaternion.LookRotation(collision.contacts[0].normal));
		}

		if (collision.transform.tag == "Concrete")
		{
			//Instantiate random impact prefab from array
			Instantiate(concreteImpactPrefabs[Random.Range
				(0, bloodImpactPrefabs.Length)], transform.position,
				Quaternion.LookRotation(collision.contacts[0].normal));

		}

		if (collision.transform.tag == "ExplosiveBarrel")
		{
			//Toggle "explode" on explosive barrel object
			collision.transform.gameObject.GetComponent
				<ExplosiveBarrelScript>().explode = true;
		}
       // }
	}
}
