using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 釣りゲーム中のManager
/// </summary>
public class FishingSceneManager : MonoBehaviour
{
    private E_FishingSceneState currentState = E_FishingSceneState.TryInputJustTime;
    private readonly int scoreOfOneFish = 10;

    /// <summary>
    /// 何ルーチン以内の入力でボーナススコアが入るか。
    /// </summary>
    private readonly int borderOfFastInputScoreBonus = 2;

    /// <summary>
    /// 今回釣った本数
    /// </summary>
    public int CountFishedNum { get; private set; } = 0;

    /// <summary>
    /// 現在のレベル(釣った本数で決まる)
    /// </summary>
    public int CurrentLevel
    {
        get
        {
            if (this.CountFishedNum < 5) return 1;       //badまでセーフ
            else if (this.CountFishedNum < 10) return 2; //goodまでセーフ
            else if (this.CountFishedNum < 20) return 3; //greatまでセーフ
            else return 4;                               //justのみセーフ
        }
    }

    /// <summary>
    /// 何ルーチン目まで待つことができるか。(レベル1で20。レベル1上がるごとに5減少。最終的に5)
    /// </summary>
    public int CountOfRoutineLimit => 20 - ((this.CurrentLevel - 1) * 5);

    /// <summary>
    /// 入力精度によるスコアボーナス。greatならレベル分、justならレベルの2倍ボーナス。
    /// </summary>
    public int CurrentInputAccuracyScoreBonus
    {
        get
        {
            switch (NotesManager.Instance.CurrentNotesInputAccuracy)
            {
                case E_NotesInputAccuracy.Just:
                    return this.CurrentLevel * 2;
                case E_NotesInputAccuracy.Great:
                    return this.CurrentLevel;
                default:
                    return 0;
            }
        }
    }

    /// <summary>
    /// 入力速度によるスコアボーナス。現在2周以内でレベル分ボーナス。(変更するかも)
    /// </summary>
    public int CurrentFastInputScoreBonus
    {
        get
        {
            if (NotesManager.Instance.CurrentNotesObject.CountRoutine < this.borderOfFastInputScoreBonus) return this.CurrentLevel;
            else return 0;
        }
    }

    /// <summary>
    /// 入力が成功したかどうか。(入力精度が、下限より上かどうか)
    /// </summary>
    public bool IsSuccessInput
    {
        get
        {
            E_NotesInputAccuracy inputAccuracy = NotesManager.Instance.CurrentNotesInputAccuracy;
            return (inputAccuracy == E_NotesInputAccuracy.Just) ||
                (inputAccuracy == E_NotesInputAccuracy.Great && this.CurrentLevel <= 3) ||
                (inputAccuracy == E_NotesInputAccuracy.Good && this.CurrentLevel <= 2) ||
                (inputAccuracy == E_NotesInputAccuracy.Bad && this.CurrentLevel == 1);
        }
    }

    /// <summary>
    /// 現在の獲得スコア
    /// </summary>
    public int CurrentScore { get; private set; } = 0;


    public static FishingSceneManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            throw new Exception();
        }
    }

    private void Update()
    {
        if(this.currentState == E_FishingSceneState.TryInputJustTime && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(NotesManager.Instance.CurrentNotesObject.CountRoutine);
            Debug.Log(NotesManager.Instance.CurrentNotesInputAccuracy);
        }
    }

    private void OnPlayerInput()
    {

    }

    public enum E_FishingSceneState
    {
        TryInputJustTime,
        ShowFishedThing,

    }
}
