using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// Notesの出現処理など、Notesの管理
/// </summary>
public class NotesManager : MonoBehaviour
{
    private readonly float justTimeRange = 0.04f;
    private readonly float greatTimeRange = 0.1f;
    private readonly float goodTimeRange = 0.2f;
    private readonly float badTimeRange = 0.3f;

    private readonly float minAnimationSpeedRate = 0.5f;
    private readonly float maxAnimationSpeedRate = 2f;

    /// <summary>
    /// 1回の釣り上げに、何個のタイミングアニメーションを再生するか
    /// </summary>
    private readonly int countOfInputSequenceInOneFishing = 3;

    /// <summary>
    /// タイミングアニメーションごとにひとつ。
    /// </summary>
    [SerializeField] private GameObject[] notePrefabs;
    [SerializeField][ReadOnly] private NotesObject[] notes;
    private int currentNoteIndex = 4;

    public NotesObject CurrentNotesObject => this.notes[this.currentNoteIndex];

    /// <summary>
    /// 現在の入力判定を取得
    /// </summary>
    public E_NotesInputAccuracy CurrentNotesInputAccuracy
    {
        get
        {
            float timeFromJustToCurrent = this.CurrentNotesObject.TimeFromJustToCurrent;
            if (timeFromJustToCurrent <= this.justTimeRange / 2) return E_NotesInputAccuracy.Just;
            else if (timeFromJustToCurrent <= this.greatTimeRange / 2) return E_NotesInputAccuracy.Great;
            else if (timeFromJustToCurrent <= this.goodTimeRange / 2) return E_NotesInputAccuracy.Good;
            else if (timeFromJustToCurrent <= this.badTimeRange / 2) return E_NotesInputAccuracy.Bad;
            else return E_NotesInputAccuracy.Miss;
        }
    }

    /// <summary>
    /// ひとつの釣り上げで、現在何回すでに入力しているか(何個目の入力か...最初は0個め)
    /// </summary>
    public int CountInputInOneFishing { get; private set; }

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
        this.notes[this.currentNoteIndex/*UnityEngine.Random.Range(0, this.notes.Length)*/].Generate(1f/*UnityEngine.Random.Range(0.5f, 2f)*/);
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

/// <summary>
/// 入力精度
/// </summary>
public enum E_NotesInputAccuracy
{
    Just,
    Great,
    Good,
    Bad,
    Miss
}