using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 2つの丸がそれぞれ円状に時計回り、合わせて∞のように。真ん中で重なる。2つの初期位置は、∞の左右両端。
/// </summary>
public class NO_TwinCircleMove : NotesObject
{
    [SerializeField] private GameObject leftCircle;
    [SerializeField] private GameObject rightCircle;
    private Vector3 centerPositionOfLeftCircle;
    private Vector3 centerPositionOfRightCircle;
    private float radius;


    protected override float JustTimeFromRoutineStartInDefaultTimeScale => 2f;

    protected override void SetNoteAnimationSequence()
    {
        this.InitSet();
        this.noteAnimationSequence = DOTween.Sequence()
            .SetLoops(-1);
        this.onUpdateInSequence.AddListener(() =>
        {
            this.leftCircle.transform.position = new Vector3(this.radius * Mathf.Cos(-2 * Mathf.PI * this.CurrentPositionOfRoutine + Mathf.PI), this.radius * Mathf.Sin(-2 * Mathf.PI * this.CurrentPositionOfRoutine + Mathf.PI), 0f)
             + this.centerPositionOfLeftCircle;
            this.rightCircle.transform.position = new Vector3(this.radius * Mathf.Cos(-2 * Mathf.PI * this.CurrentPositionOfRoutine), this.radius * Mathf.Sin(-2 * Mathf.PI * this.CurrentPositionOfRoutine), 0f)
             + this.centerPositionOfRightCircle;
        });
    }

    private void InitSet()
    {
        this.radius = (this.rightCircle.transform.position.x - this.leftCircle.transform.position.x) / 4f;
        this.centerPositionOfLeftCircle = this.leftCircle.transform.position + Vector3.right * this.radius;
        this.centerPositionOfRightCircle = this.rightCircle.transform.position + Vector3.left * this.radius;
    }
}
