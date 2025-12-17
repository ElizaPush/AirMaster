using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class AirplaneHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public float collisiondamage = 5f;

    public TMP_Text healthText;
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    void Update()
    {
        // Постоянно выводим здоровье в консоль
        Debug.Log("Текущее здоровье: " + currentHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("получен урон" + damage + "текущее здоровье" +  currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount) //увеличение здоровья после уничтожения здания
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    private void OnCollisionEnter(Collision collision) //уменьшение на 5 при столкновении со зданием
    {
        if (collision.gameObject.CompareTag("house"))
        {
            TakeDamage(collisiondamage);
        }
    }

    void UpdateHealthUI()
    {
        healthText.text = Mathf.Ceil(currentHealth) + "/" + maxHealth;  // 75 / 100
    }

    void Die()
    {
        Debug.Log("Самолет уничтожен");
        SceneManager.LoadScene(2); //сцена смерти
    }
}
