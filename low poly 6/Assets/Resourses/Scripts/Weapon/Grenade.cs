using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {

    public GameObject explosionPrefab;
    public float blastRadius = 5f;
    public float exsplosionTimer = 3f;

	// Use this for initialization
	IEnumerator Start () {
        yield return new WaitForSeconds(exsplosionTimer);

        Debug.Log("BOOM");

        GameObject exsplosionEffect = Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(exsplosionEffect, 5f);

        HitTargets();

        Destroy(gameObject);
	}

    void HitTargets()
    {
        Collider selfCollider = GetComponent<Collider>();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, blastRadius);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider == selfCollider) continue;

            if (hitCollider is CharacterController)
            {
                print("Player is in blast radius");

                RaycastHit hit;

                Vector3 directionToTarget = hitCollider.transform.position - transform.position;

                if (Physics.Raycast(transform.position, directionToTarget, out hit))
                {
                    if (hit.collider is CharacterController)
                    {
                        //IN RADIUS
                        //Apply damage to things and player
                        print("Got hit");
                    }
                    else
                    {
                        //OUT OF RADIUS
                        print("Didnt get hit");
                    }
                }
            }

        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, blastRadius);
    }

}
