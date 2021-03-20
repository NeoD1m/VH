using SQLite4Unity3d;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog2
{
    [PrimaryKey, AutoIncrement]
    public int id { get; set; }
    public string pic { get; set; }
    public string text { get; set; }
    public string btn_text1 { get; set; }
    public string btn_text2 { get; set; }
    public string btn_text3 { get; set; }
    public int btn_id1 { get; set; }
    public int btn_id2 { get; set; }
    public int btn_id3 { get; set; }

    public string getBtnText(int id)
    {
        switch (id)
        {
            case 1: return btn_text1;
            case 2: return btn_text2;
            case 3: return btn_text3;
        }
        Debug.Log("Error! Dialog_model. getBtnText(). Button id is out of range.");
        return "error";
    }

    public int getNextId(int id)
    {
        switch (id)
        {
            case 1: return btn_id1;
            case 2: return btn_id2;
            case 3: return btn_id3;
        }
        Debug.Log("Error! Dialog_model. getNextId(). Button id is out of range.");
        return -228;
    }

    public override string ToString()
    {
        return string.Format("Dialog model object. id={0}, pic={1}, text={2}, btn1={3}, btn2={4}, btn3={5}, idToGo={6},{7},{8}", id, pic, text, btn_text1, btn_text2, btn_text3, btn_id1, btn_id2, btn_id3);
    }
}
