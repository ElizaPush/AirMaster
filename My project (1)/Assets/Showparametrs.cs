using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Showparametrs : MonoBehaviour
{
    [Header("Airplane")]
    public Rigidbody airplaneRigidbody;   // Rigidbody самолёта

    [Header("UI")]
    public TMP_Text speedText;
    public TMP_Text altitudeText;

    void Update()
    {
        UpdateSpeed();
        UpdateAltitude();
    }

    void UpdateSpeed()
    {
        // Скорость в км/ч
        float speed = airplaneRigidbody.velocity.magnitude * 3.6f;
        speedText.text = "Скорость: " + Mathf.Round(speed) + " km/h";

        // Вывод в консоль
        Debug.Log("Speed: " + Mathf.Round(speed) + " km/h");
    }

    void UpdateAltitude()
    {
        // Высота по Y
        //float altitude = transform.position.y;
       // altitudeText.text = "Altitude: " + Mathf.Round(altitude) + " m";
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            float altitude = hit.distance; //до столкновения с коллайдером
            altitudeText.text = "Altitude: " + Mathf.Round(altitude) + " m";
            // Вывод в консоль
            Debug.Log("Altitude: " + Mathf.Round(altitude) + " m");
        }
        else
        {
            altitudeText.text = "Altitude: ---";  //если луч не выстрелил землю, показываем данных нет
            Debug.Log("Altitude: ---");
        }
    }
}
