using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

/// <summary>
/// 釣りゲーム中のManager
/// </summary>
public class FishingSceneManager : MonoBehaviour
{
    private E_FishingSceneState currentState = E_FishingSceneState.TryInputJustTime;
    private readonly int scoreOfOneFish = 10;

    /// <summary>
    /// 1回の釣り上げに、何個のタイミングアニメーションを再生するか
    /// </summary>
    private readonly int countOfInputSequenceInOneFishing = 3;

    /// <summary>
    /// 何ルーチン以内の入力でボーナススコアが入るか。
    /// </summary>
    private readonly int borderOfFastInputScoreBonus = 2;

    private int _countFishedNum;
    /// <summary>
    /// 今回釣った本数
    /// </summary>
    public int CountFishedNum
    {
        get { return this._countFishedNum; }
        private set
        {
            this._countFishedNum = value;
            FishingUIManager.Instance.LimitInputOccuracyText.text = "最低精度: " + this.CurrentLimitInputAccuracy.ToString();
        }
    }

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
    /// 現在のミスにならない最低入力精度
    /// </summary>
    public E_NotesInputAccuracy CurrentLimitInputAccuracy
    {
        get
        {
            if (this.CurrentLevel == 1) return E_NotesInputAccuracy.Bad;
            else if (this.CurrentLevel == 2) return E_NotesInputAccuracy.Good;
            else if (this.CurrentLevel == 3) return E_NotesInputAccuracy.Great;
            else return E_NotesInputAccuracy.Just;
        }
    }

    /// <summary>
    /// 何ルーチン目まで待つことができるか。(レベル1で20。レベル1上がるごとに5減少。最終的に5)
    /// </summary>
    public int CountOfRoutineLimit => 20 - ((this.CurrentLevel - 1) * 5);

    private int _inputAccuracyScoreBonusInOneFishing;
    /// <summary>
    /// 1回のキャスティングにおける入力精度によるスコアボーナス記憶。釣り上げたら初期化。greatならレベル分、justならレベルの2倍ボーナス。
    /// </summary>
    public int InputAccuracyScoreBonusInOneFishing
    {
        get { return this._inputAccuracyScoreBonusInOneFishing; }
        private set
        {
            this._inputAccuracyScoreBonusInOneFishing = value;
            FishingUIManager.Instance.InputOccuracyScoreBonusText.text = "精度ボーナス: +" + this._inputAccuracyScoreBonusInOneFishing;
        }
    }

    private int _fastInputScoreBonusInOneFishing;
    /// <summary>
    /// 1回のキャスティングにおける、入力速度によるスコアボーナス記憶。釣り上げたら初期化。現在2周以内でレベル分ボーナス。
    /// </summary>
    public int FastInputScoreBonusInOneFishing
    {
        get { return this._fastInputScoreBonusInOneFishing; }
        private set
        {
            this._fastInputScoreBonusInOneFishing = value;
            FishingUIManager.Instance.FastInputScoreBonusText.text = "速度ボーナス: +" + this._fastInputScoreBonusInOneFishing;
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

    private int _currentScore;
    /// <summary>
    /// 現在の獲得スコア
    /// </summary>
    public int CurrentScore
    {
        get { return this._currentScore; }
        private set
        {
            this._currentScore = value;
            FishingUIManager.Instance.CurrentScoreText.text = "獲得スコア: " + this._currentScore;
        }
    }

    /// <summary>
    /// ひとつの釣り上げで、現在何回すでに入力しているか(何個目の入力か...最初は0個め)
    /// </summary>
    public int CountInputInOneFishing { get; private set; } = 0;


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
            this.OnPlayerInput();
        }else if(this.currentState == E_FishingSceneState.ShowFishedThing && Input.GetKeyDown(KeyCode.Return))
        {
            this.currentState = E_FishingSceneState.TryInputJustTime;
        }
    }

    /// <summary>
    /// プレイヤーが入力した際の総合処理
    /// </summary>
    private void OnPlayerInput()
    {
        NotesManager.Instance.OnPlayerInput();
        //精度表示などの共通処理
        Debug.Log(NotesManager.Instance.CurrentNotesInputAccuracy);
        Debug.Log("かかったルーチン: " + NotesManager.Instance.CurrentNotesObject.CountRoutine);


        CoroutineManager.DelayMethod(1f, () =>
        {
            NotesManager.Instance.CurrentNotesObject.ResetBeforeFinish();
            if (this.IsSuccessInput)
            {
                this.CountInputInOneFishing++;
                if (this.CountInputInOneFishing >= this.countOfInputSequenceInOneFishing)
                {
                    //釣り上げ処理
                    this.CountInputInOneFishing = 0;
                    this.OnGetFish();
                }
                else
                {
                    //次のタイミングアニメーション再生処理
                }
            }
            else
            {
                //gameOver処理
                this.OnGameOver();
            }
        });
    }

    public void OnGameOver()
    {
        Debug.Log("GameOver(最終スコア: " + this.CurrentScore + ")");
        SceneManager.LoadScene("Title");
    }

    public void OnGetFish()
    {
        Debug.Log("釣り上げた");
        FishingUIManager.Instance.CurrentScoreText.text = "獲得スコア: " + this.CurrentScore;
        this.currentState = E_FishingSceneState.ShowFishedThing;
    }

    /// <summary>
    /// 入力精度によるスコアボーナス。greatならレベル分、justならレベルの2倍ボーナス。
    /// </summary>
    public int GetThisInputAccuracyScoreBonus()
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

    /// <summary>
    /// 入力速度によるスコアボーナス。現在2周以内でレベル分ボーナス。
    /// </summary>
    public int GetThisInputFastScoreBonus()
    {
        if (NotesManager.Instance.CurrentNotesObject.CountRoutine < this.borderOfFastInputScoreBonus) return this.CurrentLevel;
        else return 0;
    }

    /// <summary>
    /// 次のタイミングアニメーションの再生
    /// </summary>
    public void PlayNewAnimation()
    {
        NotesManager.Instance.GenerateNote();
    }

    public enum E_FishingSceneState
    {
        TryInputJustTime,
        ShowFishedThing,

    }
}
