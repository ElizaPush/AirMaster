using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damagescript : MonoBehaviour
{
    public int damageAmount = 25;

    public int enemyDamage = 50;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "house") {
            other.GetComponent<Health>().TakeDamage(damageAmount);
        }

        else if (other.CompareTag("opponent"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(enemyDamage);
            }
        }
    }
}
