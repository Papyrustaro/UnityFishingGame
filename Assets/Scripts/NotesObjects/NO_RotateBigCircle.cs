using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 画面端(外)を中心とする大きな円上に複数の図形が回転。シルエットと図形が一致するところで押す。
/// </summary>
public class NO_RotateBigCircle : NotesObject
{
    [SerializeField] private GameObject rotateObjectsCenter;
    [SerializeField] private GameObject[] rotateObjects;

    protected override float JustTimeFromRoutineStartInDefaultTimeScale => 3f;

    protected override void SetNoteAnimationSequence()
    {
        this.noteAnimationSequence = DOTween.Sequence()
            .Append(this.rotateObjectsCenter.transform.DOLocalRotate(Vector3.forward * -360f, this.JustTimeFromRoutineStartInDefaultTimeScale * 2f, RotateMode.FastBeyond360))
            .SetLoops(-1)
            .SetLink(this.gameObject);
        foreach(GameObject obj in this.rotateObjects)
        {
            this.noteAnimationSequence
                .Join(obj.transform.DOLocalRotate(Vector3.forward * 360f, this.JustTimeFromRoutineStartInDefaultTimeScale * 2f, RotateMode.FastBeyond360));
        }
    }
}
