using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    public Button restartSceneButton,quitGameButton,toggleBossBrain,toggleBossMovement;
    public GameObject menu,Boss;
    public PlayerControll playerControll;
    public Text fpsCounter,bossBrainText,bossMovementText;
    public Toggle godModeToggle,staminaModeToggle;
    void Start()
    {
        restartSceneButton.onClick.AddListener(restartScene);
        quitGameButton.onClick.AddListener(quitGame);
        toggleBossBrain.onClick.AddListener(toggleOnOffBossBrain);
        toggleBossMovement.onClick.AddListener(toggleOnOffBossMovement);
    }
    void Update()
    {
        int fps = (int)(1f / Time.unscaledDeltaTime);
        fpsCounter.text = fps.ToString() + " FPS";

        if (Input.GetKeyDown(KeyCode.E))
            if (menu.activeSelf == false) menu.SetActive(true); else menu.SetActive(false);
        if (Boss != null)
        {
            bossBrainText.text = "Boss AI: " + Boss.GetComponent<Brain>().enabled.ToString();
            bossMovementText.text = "Boss Movement: " + Boss.GetComponent<EnemyMovement>().enabled.ToString();
        }
        if (playerControll.HP < 3 && godModeToggle.isOn == true) playerControll.HP = 3;
        if (playerControll.stamina < playerControll.staminaSlider.maxValue && staminaModeToggle.isOn == true) playerControll.stamina = playerControll.staminaSlider.maxValue;
    }
    void toggleOnOffBossBrain()
    {
        if (Boss.GetComponent<Brain>().enabled == true) Boss.GetComponent<Brain>().enabled = false; else Boss.GetComponent<Brain>().enabled = true;
    }
    void toggleOnOffBossMovement()
    {
        if (Boss.GetComponent<EnemyMovement>().enabled == true) Boss.GetComponent<EnemyMovement>().enabled = false; else Boss.GetComponent<EnemyMovement>().enabled = true;
    }
    void quitGame()
    {
        Application.Quit();
    }
    void restartScene()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
