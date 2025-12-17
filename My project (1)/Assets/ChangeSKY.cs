using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSKY : MonoBehaviour
{
    public Material[] Skyboxes;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Changesky()
    {
        int randomNumber = Random.Range(0, Skyboxes.Length);
        RenderSettings.skybox = Skyboxes[randomNumber];
    }
}
