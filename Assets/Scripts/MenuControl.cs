using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    // Start is called before the first frame update
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Выход из игры");
    }
    public void StartPlay()
    {
        SceneManager.LoadScene("AITesting");
        
    }
}
