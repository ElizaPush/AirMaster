using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damagescript : MonoBehaviour
{
    public int damageAmount = 25;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "house") {
            other.GetComponent<Health>().TakeDamage(damageAmount);
        }
    }
}
