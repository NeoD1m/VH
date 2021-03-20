using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog", order = 51)]
public class Dialog : ScriptableObject
{
    [SerializeField]
    public int ID;
    [SerializeField]
    private Word[] words;

    private Word current;
    private void OnEnable()
    {
        if (words.Length > 0)
            current = words[0];
        else
            Debug.Log("Вы ебанутые? Диалог без реплик загружаете. || Dialog.cs onEnable()");
    }
    public Word getWord()
    {
        return current;
    }
    public bool goNext(int id)
    {
        for (int i = 0; i < words.Length; ++i)
        {
            if (words[i].myId == id)
            {
                current = words[i];
                return true;
            }
        }
        Debug.Log("Finish dialog || DialogStuff.cs");
        return false;
    }
    public int[] getIds()
    {
        return new int[] { current.id1, current.id2, current.id3 };
    }
}

[CreateAssetMenu(fileName = "New Word", menuName = "Word", order = 52)]
public class Word : ScriptableObject
{
    [SerializeField]
    public int myId;
    [SerializeField]
    public Sprite picture;
    [SerializeField]
    public string phrase;
    [SerializeField]
    public string ans1;
    [SerializeField]
    public string ans2;
    [SerializeField]
    public string ans3;
    [SerializeField]
    public int id1;
    [SerializeField]
    public int id2;
    [SerializeField]
    public int id3;
}
[CreateAssetMenu(fileName = "New Dialog Container", menuName = "Dialog Container", order = 50)] 
public class GlobalDialogContainer : ScriptableObject
{
    [SerializeField]
    public Dialog[] dialogs;
}
