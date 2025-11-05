using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bull : MonoBehaviour
{
   //public int speed = 50;

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector3.forward * speed * Time.deltaTime); //перемещение
        Destroy(gameObject, 0.5f);
    }
}
