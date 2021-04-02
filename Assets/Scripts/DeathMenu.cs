using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public GameObject deathObject;
    public PlayerControll player;

    // Start is called before the first frame update
    void Start()
    {
        deathObject.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.HP <= 0)
        {
            deathObject.gameObject.SetActive(true);
        }


    }
    public void ReloadLeavel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
