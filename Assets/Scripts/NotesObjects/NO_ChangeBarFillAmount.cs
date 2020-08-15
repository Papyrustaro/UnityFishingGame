using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

/// <summary>
/// バーを上下に揺らす。バーが最大のときjust
/// </summary>
public class NO_ChangeBarFillAmount : NotesObject
{
    [SerializeField] private Image changeFillAmountBar;
    protected override float JustTimeFromRoutineStartInDefaultTimeScale => 1f;

    protected override void SetNoteAnimationSequence()
    {
        this.noteAnimationSequence = DOTween.Sequence()
            .Append(this.changeFillAmountBar.DOFillAmount(1f, this.JustTimeFromRoutineStartInDefaultTimeScale))
            .Append(this.changeFillAmountBar.DOFillAmount(0f, this.JustTimeFromRoutineStartInDefaultTimeScale))
            .SetLoops(-1);
    }
}
