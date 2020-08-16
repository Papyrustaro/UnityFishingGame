using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.Events;

public abstract class NotesObject : MonoBehaviour
{
    protected Sequence noteAnimationSequence;
    /// <summary>
    /// Sequence再生時にupdateで呼ばれる処理(継承先で代入)
    /// </summary>
    protected UnityEvent onUpdateInSequence = new UnityEvent();

    /// <summary>
    /// Sequence終了時(プレイヤー入力処理終了後)呼ばれる処理
    /// </summary>
    protected UnityEvent onFinishAnimation = new UnityEvent();

    /// <summary>
    /// アニメーションのワンルーチン(往復なら半周)開始からjustTimeまでの時間
    /// </summary>
    protected float JustTimeFromRoutineStart => this.JustTimeFromRoutineStartInDefaultTimeScale / this.noteAnimationSequence.timeScale;

    /// <summary>
    /// 入力ワンルーチン(往復で2回justなら半周)の時間
    /// </summary>
    protected float OneInputRoutineTime => this.JustTimeFromRoutineStart * 2f;

    /// <summary>
    /// timeScale==1fにおける、アニメーションのワンルーチン開始からjustTimeまでの時間(継承先で指定)。ワンルーチンの時間の半分。
    /// </summary>
    protected abstract float JustTimeFromRoutineStartInDefaultTimeScale { get; }

    /// <summary>
    /// 現在どのタイミングか記憶用のcountTime。アニメーションのワンルーチン(往復なら半周)ごとにリセットされる。
    /// </summary>
    public float CountTimeFromEveryRoutineStart { get; protected set; }

    /// <summary>
    /// justTimeのタイミングから今が何秒ズレているか。
    /// </summary>
    public float TimeFromJustToCurrent => Mathf.Abs(JustTimeFromRoutineStart - CountTimeFromEveryRoutineStart);

    private int _countRoutine;
    /// <summary>
    /// 開始から何ルーチン経ったか(往復の場合は片道で1ルーチン)
    /// </summary>
    public int CountRoutine
    {
        get { return this._countRoutine; }
        private set
        {
            this._countRoutine = value;
            FishingUIManager.Instance.RemainRoutineCountText.text = "残りルーチン: " + (FishingSceneManager.Instance.CountOfRoutineLimit - this._countRoutine);
        }
    }

    /// <summary>
    /// ワンルーチンのどのあたりまで進んでいるか(0~1)
    /// </summary>
    public float CurrentPositionOfRoutine => this.CountTimeFromEveryRoutineStart / this.OneInputRoutineTime;

    /// <summary>
    /// タイミングアニメーションの再生開始。
    /// </summary>
    /// <param name="playSpeed">再生速度</param>
    public void Generate(float playSpeed)
    {
        if (this.noteAnimationSequence == null)
        {
            SetNoteAnimationSequence();
            SetSequenceOnUpdateFunc();
        }
        FishingUIManager.Instance.RemainRoutineCountText.text = "残りルーチン: " + (FishingSceneManager.Instance.CountOfRoutineLimit - this._countRoutine);
        this.noteAnimationSequence.timeScale = playSpeed;
        this.gameObject.SetActive(this);
        if (!this.noteAnimationSequence.IsPlaying()) this.noteAnimationSequence.Play(); 
    }

    public void StopAnimation()
    {
        this.noteAnimationSequence.Pause();
    }

    /// <summary>
    /// アニメーション終了前(プレイヤーの入力処理を終えた後)の初期化処理
    /// </summary>
    public void ResetBeforeFinish()
    {
        this.CountTimeFromEveryRoutineStart = 0f;
        this.CountRoutine = 0;
        this.noteAnimationSequence.Goto(0f);
        this.noteAnimationSequence.Pause();
        this.onFinishAnimation.Invoke();
    }

    private void SetSequenceOnUpdateFunc()
    {
        this.noteAnimationSequence.OnUpdate(() =>
        {
            if (this.CountTimeFromEveryRoutineStart + Time.deltaTime >= this.OneInputRoutineTime) //次のルーチンへ行くとき
            {
                this.CountRoutine++;
                this.CountTimeFromEveryRoutineStart += Time.deltaTime - this.OneInputRoutineTime;
            }
            else
            {
                this.CountTimeFromEveryRoutineStart += Time.deltaTime;
            }

            //継承先で指定した処理
            this.onUpdateInSequence.Invoke();
        });
    }

    /// <summary>
    /// タイミングアニメーションの初期化。
    /// </summary>
    protected abstract void SetNoteAnimationSequence();
}
