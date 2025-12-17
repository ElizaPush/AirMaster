using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulletmove : MonoBehaviour
{
    public float speed = 60f;
    public float damage = 20f;
    public float lifetime = 10f;

   
    void Start()
    {
        //Destroy(gameObject, lifetime);
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        //Destroy(gameObject, 5f);
       // transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) //пуля врага
    {
        Debug.Log("Пуля столкнулась с: " + other.name);
        AirplaneHealth airplane = other.GetComponent<AirplaneHealth>();

        if (airplane != null)
        {
            airplane.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
