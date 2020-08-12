using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public abstract class NotesObject : MonoBehaviour
{
    protected Sequence noteAnimationSequence;

    /// <summary>
    /// アニメーションのワンルーチン(往復なら半周)開始からjustTimeまでの時間
    /// </summary>
    protected float JustTimeFromRoutineStart => this.JustTimeFromRoutineStartInDefaultTimeScale / this.noteAnimationSequence.timeScale;

    /// <summary>
    /// timeScale==1fにおける、アニメーションのワンルーチン開始からjustTimeまでの時間
    /// </summary>
    protected abstract float JustTimeFromRoutineStartInDefaultTimeScale { get; }

    /// <summary>
    /// 現在どのタイミングか記憶用のcountTime。アニメーションのワンルーチン(往復なら半周)ごとにリセットされる。
    /// </summary>
    protected float CountTimeFromEveryRoutineStart { get; set; }

    /// <summary>
    /// justTimeのタイミングから今が何秒ズレているか。
    /// </summary>
    public float TimeFromJustToCurrent => Mathf.Abs(JustTimeFromRoutineStart - CountTimeFromEveryRoutineStart);

    public void Generate(float playSpeed)
    {
        if (this.noteAnimationSequence == null)
        {
            SetNoteAnimationSequence();
        }
        this.noteAnimationSequence.timeScale = playSpeed;
        this.gameObject.SetActive(this);
    }

    /// <summary>
    /// タイミングアニメーションの初期化
    /// </summary>
    protected abstract void SetNoteAnimationSequence();
}
