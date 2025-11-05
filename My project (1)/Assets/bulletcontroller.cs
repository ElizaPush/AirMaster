using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletcontroller : MonoBehaviour
{
   
    
    
   

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "enemy")
        {
            Destroy(gameObject, 0.1f);
        }
       
    }


}
