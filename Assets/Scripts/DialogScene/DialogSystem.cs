using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    const bool DEBUG_OUTPUT = true;

    DataService db;
    Dialog2 currentDialog;

    [SerializeField]
    Image DPicture;
    [SerializeField]
    Text DMainText;
    [SerializeField]
    Text[] BtnTexts;

    void Start()
    {
        PlayerPrefs.SetInt("DialogToLoad", 1);
        Debug.Log("Start Dialog");
        db = new DataService("data.db");
        MakeDialog();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MakeDialog()
    {
        int dialog_id = getIntPrefsSafe("DialogToLoad", "DialogSystem. MakeDialog()");
        if (dialog_id > 0)
        {
            currentDialog = db.GetDialogById(dialog_id);
            Debug.Log(currentDialog.ToString());
            trySetPicture();

            DMainText.text = currentDialog.text;
            for (int i = 0; i < BtnTexts.Length; ++i)
                BtnTexts[i].text = currentDialog.getBtnText(i + 1);
        }
        else
            if (dialog_id == 0)
            {
                finish();
            }
            else
            {
                Debug.Log("Dialog can NOT be loaded.");
                finish();
            }
    }

    void finish()
    {
        Debug.Log("Finish dialog");
        new go_to_scene().ChangeScene("2dMovement");
    }

    public void onClick(int num)
    {
        PlayerPrefs.SetInt("DialogToLoad", currentDialog.getNextId(num));
        MakeDialog();
    }

    public void trySetPicture()
    {
        string pathToPicture = "dialog_arts\\" + currentDialog.pic;
        try
        {
            DPicture.sprite = Resources.Load(pathToPicture, typeof(Sprite)) as Sprite;
        }
        catch (System.Exception e)
        {
            Debug.Log("Error! Fuck you! Can't get image for path=" + pathToPicture+ " "+e.ToString());
        }
    }

    public int getIntPrefsSafe(string key, string error_msg)
    {
        int tmp = PlayerPrefs.GetInt(key, -228);
        if (tmp == -228)
            Debug.Log("Bad get Int for key=" + key + " " + error_msg);
        return tmp;
    }
}
