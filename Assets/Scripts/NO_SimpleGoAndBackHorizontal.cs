using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 真ん中にターゲット目盛。長方形のバーを左右に揺らし、真ん中に来た時にjust
/// </summary>
public class NO_SimpleGoAndBackHorizontal : NotesObject
{
    [SerializeField] private GameObject centerTarget;
    [SerializeField] private GameObject movingBar;

    protected override float JustTimeFromRoutineStartInDefaultTimeScale => 2f;

    protected override void SetNoteAnimationSequence()
    {
        this.noteAnimationSequence = DOTween.Sequence()
            .OnUpdate(() => this.CountTimeFromEveryRoutineStart += Time.deltaTime)
            .AppendCallback(() => this.CountTimeFromEveryRoutineStart = 0f)
            .Append(this.movingBar.transform.DOMoveX(0f, 2f))
            .Append(this.movingBar.transform.DOMoveX(-this.movingBar.transform.position.x, 2f))
            .AppendCallback(() => this.CountTimeFromEveryRoutineStart = 0f)
            .Append(this.movingBar.transform.DOMoveX(0f, 2f))
            .Append(this.movingBar.transform.DOMoveX(this.movingBar.transform.position.x, 2f))
            .SetLoops(-1);
    }
}
