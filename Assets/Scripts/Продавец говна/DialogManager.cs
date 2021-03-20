using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField]
    GlobalDialogContainer dialogContainer;
    [SerializeField]
    Text t1;
    [SerializeField]
    Text t2;
    [SerializeField]
    Text t3;
    [SerializeField]
    Image pic;
    [SerializeField]
    Text phrase;

    private int dialogId;
    private Dialog currentDialog;
    // Start is called before the first frame update
    void Start()
    {
        int temp = PlayerPrefs.GetInt("DialogToLoad", -228);
        if (temp == -228)
            Debug.Log("Попытка загрузить диалог, но ID не назначен. || DialogManager.cs Start");
        else
            dialogId = temp;

        if (setCurrentDialog(dialogId))
        {
            drawWord();
        }
        else
            new go_to_scene().ChangeScene("StartMenu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void drawWord()
    {
        Word current = currentDialog.getWord();
        phrase.text = current.phrase;
        pic.sprite = current.picture;
        t1.text = current.ans1;
        t2.text = current.ans2;
        t3.text = current.ans3;
    }

    bool setCurrentDialog(int id)
    {
        if (dialogContainer != null && dialogContainer.dialogs != null) { 
            for (int i = 0; i < dialogContainer.dialogs.Length; ++i)
                if (dialogContainer.dialogs[i] != null)
                    if (dialogContainer.dialogs[i].ID == id)
                    {
                        currentDialog = dialogContainer.dialogs[i];
                        return true;
                    }
            }
        if (dialogContainer != null)
            Debug.Log("В контейнере " + dialogContainer.name + " нет диалога с ID=" + id.ToString() + " || DialogManager.cs setCurrentDialog()");
        else
            Debug.Log("Фатальная ошибка в работе диалоговой системы. Dialog Container повреждён. || DialogManeger.cs setCurrentDialog()");
        return false;
    }

    public void onButtonClick(int id)
    {
        if (id < 1 || id > 3)
            Debug.Log("ID кнопки указан неверно. Допустимы значения [1,2,3]");
        else
        {
            if (currentDialog.goNext(currentDialog.getIds()[id-1]))
                drawWord();
            else
                new go_to_scene().ChangeScene("StartMenu");
        }
    }
}
