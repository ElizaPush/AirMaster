using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FromLostToMenu : MonoBehaviour
{
    public void Return()
    {
        SceneManager.LoadScene(0);
    }
}
