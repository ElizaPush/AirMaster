using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int currentHealth = 100;
    public int healAmount = 3;
   
   
    void Update()
    {

    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            HealAirplane();
            Destroy(this.gameObject);
        }
    }

    void HealAirplane()
    {
        AirplaneHealth airplaneHealth = FindObjectOfType<AirplaneHealth>();

        if (airplaneHealth != null)
        {
            airplaneHealth.Heal(healAmount);
        }
    }
}
