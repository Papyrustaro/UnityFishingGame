using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public abstract class NotesObject : MonoBehaviour
{
    protected Sequence noteAnimationSequence;

    public void Generate(float playSpeed)
    {
        if(this.noteAnimationSequence == null) SetNoteAnimationSequence();
        this.noteAnimationSequence.timeScale = playSpeed;
        this.gameObject.SetActive(this);
    }

    /// <summary>
    /// タイミングアニメーションの初期化
    /// </summary>
    protected abstract void SetNoteAnimationSequence();
}
