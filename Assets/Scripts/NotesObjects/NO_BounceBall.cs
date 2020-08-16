using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 球が重力によって真ん中下にある板で跳ね返る。x軸方向は等速。
/// </summary>
public class NO_BounceBall : NotesObject
{
    [SerializeField] private GameObject bounceBall;
    [SerializeField] private GameObject ground;
    protected override float JustTimeFromRoutineStartInDefaultTimeScale => 2f;

    protected override void SetNoteAnimationSequence()
    {
        float moveXLength = -2f * this.bounceBall.transform.position.x;
        float moveYLength = this.bounceBall.transform.position.y - this.ground.transform.position.y - (this.bounceBall.transform.localScale.y / 2) - (this.ground.transform.localScale.y / 2);

        this.noteAnimationSequence = DOTween.Sequence()
            .Append(this.bounceBall.transform.DOBlendableLocalMoveBy(Vector3.right * (moveXLength / 2), this.JustTimeFromRoutineStartInDefaultTimeScale))
            .Join(this.bounceBall.transform.DOBlendableLocalMoveBy(Vector3.down * moveYLength, this.JustTimeFromRoutineStartInDefaultTimeScale).SetEase(Ease.InQuad))
            .Append(this.bounceBall.transform.DOBlendableLocalMoveBy(Vector3.right * (moveXLength / 2), this.JustTimeFromRoutineStartInDefaultTimeScale))
            .Join(this.bounceBall.transform.DOBlendableLocalMoveBy(Vector3.up * moveYLength, this.JustTimeFromRoutineStartInDefaultTimeScale).SetEase(Ease.OutQuad))
            .Append(this.bounceBall.transform.DOBlendableLocalMoveBy(Vector3.left * (moveXLength / 2), this.JustTimeFromRoutineStartInDefaultTimeScale))
            .Join(this.bounceBall.transform.DOBlendableLocalMoveBy(Vector3.down * moveYLength, this.JustTimeFromRoutineStartInDefaultTimeScale).SetEase(Ease.InQuad))
            .Append(this.bounceBall.transform.DOBlendableLocalMoveBy(Vector3.left * (moveXLength / 2), this.JustTimeFromRoutineStartInDefaultTimeScale))
            .Join(this.bounceBall.transform.DOBlendableLocalMoveBy(Vector3.up * moveYLength, this.JustTimeFromRoutineStartInDefaultTimeScale).SetEase(Ease.OutQuad))
            .SetLoops(-1)
            .SetLink(this.gameObject);
    }
}
