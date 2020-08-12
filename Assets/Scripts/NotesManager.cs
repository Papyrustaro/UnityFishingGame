using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// Notesの出現処理など、Notesの管理
/// </summary>
public class NotesManager : MonoBehaviour
{
    /// <summary>
    /// タイミングアニメーションごとにひとつ。
    /// </summary>
    [SerializeField] private GameObject[] notePrefabs;
    [SerializeField][ReadOnly] private NotesObject[] notes;

    public static NotesManager Instance { private set; get; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            throw new System.Exception();
        }

    }

    private void Start()
    {
        GenerateNote();
    }

    /// <summary>
    /// タイミングを計るためのnotesObject生成。生成する種類と、再生速度はランダム。
    /// </summary>
    public void GenerateNote()
    {
        //とりあえずSetActiveで。gameObjectとしてsetActiveしなくても、描画処理だけ変えればいいかも？
        this.notes[UnityEngine.Random.Range(0, this.notes.Length)].Generate(10f/*UnityEngine.Random.Range(0.5f, 2f)*/);
    }

    [Button(enabledMode: EButtonEnableMode.Editor)]
    public void InitSetNotes()
    {
        this.notes = new NotesObject[this.notePrefabs.Length];
        for(int i = 0; i < this.notePrefabs.Length; i++)
        {
            this.notes[i] = this.notePrefabs[i].GetComponent<NotesObject>();
        }
    }
}
