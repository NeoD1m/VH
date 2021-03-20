using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Video : MonoBehaviour
{
    public bool skip, GO;
    public VideoPlayer vp;
    private double time;
    public float timer;

    private AsyncOperation levelLoading;
    void Start()
    {
        time = vp.length;
        timer = (float)vp.length;
        PlayerPrefs.SetInt("MenuLoad", 1);
        levelLoading = SceneManager.LoadSceneAsync("AITesting");
        levelLoading.allowSceneActivation = false;
        //(float)time;
        //StartCoroutine(loadScene());
        //StartCoroutine(LoadYourAsyncScene());
        //if (skip == true) SceneManager.LoadScene(1);
    }

    void Update()
    {
        if (timer > 0) { timer -= Time.deltaTime; }
        else { levelLoading.allowSceneActivation = true; }
        
        //if (timer < 0 && GO == true) { SceneManager.LoadScene(1); }
    }
    /*
    IEnumerator loadScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("1.Menu", LoadSceneMode.Single);
        async.allowSceneActivation = false;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            //progressText.text = async.progress + "";
            yield return null;
        }

    }


    IEnumerator LoadYourAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);
        //if (timer > 0) { timer -= Time.deltaTime; }
        // Wait until the asynchronous scene fully loads
        while (timer < 0)
        {
            //Debug.Log
            yield return null;
        }
    }*/
}
