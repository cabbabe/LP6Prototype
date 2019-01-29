using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour {

    public GameObject grenadePrefab;

    public float longThrowForce = 15f;
    public float shortThrowForce = 10f;

    // Update is called once per frame
    void Update () {
		if (Input.GetButtonDown("Fire1"))
        {
            LongThrowGranade();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            ShortThrowGranade();
        }
    }

    void LongThrowGranade()
    {
        GameObject grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
        grenade.GetComponent<Rigidbody>().AddForce(transform.forward * longThrowForce, ForceMode.VelocityChange);
    }

    void ShortThrowGranade()
    {
        GameObject grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
        grenade.GetComponent<Rigidbody>().AddForce(transform.forward * shortThrowForce, ForceMode.VelocityChange);
    }
}
