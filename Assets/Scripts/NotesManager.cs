using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using KanKikuchi.AudioManager;

/// <summary>
/// Notesの出現処理など、Notesの管理
/// </summary>
public class NotesManager : MonoBehaviour
{
    private readonly float justTimeRange = 0.04f;
    private readonly float greatTimeRange = 0.1f;
    private readonly float goodTimeRange = 0.2f;
    private readonly float badTimeRange = 0.5f;

    private readonly float minAnimationSpeedRate = 0.5f;
    private readonly float maxAnimationSpeedRate = 2f;

    

    /// <summary>
    /// タイミングアニメーションごとにひとつ。
    /// </summary>
    [SerializeField] private GameObject[] notePrefabs;
    [SerializeField][ReadOnly] private NotesObject[] notes;
    private int currentNoteIndex = 0;

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

    /*private void Start()
    {
        GenerateNote();
    }*/

    /// <summary>
    /// タイミングを計るためのnotesObject生成。生成する種類と、再生速度はランダム。
    /// </summary>
    public void GenerateNote()
    {
        //とりあえずSetActiveで。gameObjectとしてsetActiveしなくても、描画処理だけ変えればいいかも？
        this.currentNoteIndex = UnityEngine.Random.Range(0, this.notePrefabs.Length);
        this.notes[this.currentNoteIndex].Generate(UnityEngine.Random.Range(1f, 5f));
    }

    /// <summary>
    /// プレイヤーが入力した際のnotesの処理。(全体の処理はFishingSceneManagerでおこなう)
    /// </summary>
    public void OnPlayerInput()
    {
        switch (this.CurrentNotesInputAccuracy)
        {
            case E_NotesInputAccuracy.Just:
                SEManager.Instance.Play(SEPath.JUST);
                break;
            case E_NotesInputAccuracy.Great:
                SEManager.Instance.Play(SEPath.GREAT);
                break;
            case E_NotesInputAccuracy.Good:
                SEManager.Instance.Play(SEPath.GOOD);
                break;
            case E_NotesInputAccuracy.Bad:
                SEManager.Instance.Play(SEPath.BAD);
                break;
            case E_NotesInputAccuracy.Miss:
                SEManager.Instance.Play(SEPath.MISS);
                break;
        }
        this.CurrentNotesObject.StopAnimation();
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