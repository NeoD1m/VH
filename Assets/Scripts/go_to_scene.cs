using UnityEngine;
using UnityEngine.SceneManagement;

public class go_to_scene : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
