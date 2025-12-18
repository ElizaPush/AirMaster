using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulletmove : MonoBehaviour
{
    public float speed = 60f;
    public float damage = 20f;
    public float lifetime = 10f;

    private Vector3 moveDirection = Vector3.zero;
    public void SetDirection(Vector3 dir)
    {
        moveDirection = dir.normalized;
        transform.rotation = Quaternion.LookRotation(moveDirection);
    }
    void Start()
    {
        //Destroy(gameObject, lifetime);
        Destroy(gameObject, 5f);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (moveDirection != Vector3.zero)
        {
            transform.position += moveDirection * speed * Time.deltaTime;
        }
        
        //transform.Translate(Vector3.forward * speed * Time.deltaTime); //was
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
            //Destroy(gameObject);
        }
        //Destroy(gameObject);
    }
}
