using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int currentHealth = 100;
    public int healPlayerAmount = 5;

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            HealPlayer();
            Destroy(this.gameObject);
        }
    }

    void HealPlayer()
    {
        AirplaneHealth airplaneHealth = FindObjectOfType<AirplaneHealth>();

        if (airplaneHealth != null)
        {
            airplaneHealth.Heal(healPlayerAmount);
        }
    }
}
