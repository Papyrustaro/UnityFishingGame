using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 真ん中にターゲット目盛。長方形のバーを左右に揺らし、真ん中に来た時にperfect
/// </summary>
public class NO_SimpleGoAndBackHorizontal : NotesObject
{
    [SerializeField] private GameObject centerTarget;
    [SerializeField] private GameObject movingBar;

    protected override void SetNoteAnimationSequence()
    {
        this.noteAnimationSequence = DOTween.Sequence()
            .Append(this.movingBar.transform.DOMoveX(0f, 2f))
            .Append(this.movingBar.transform.DOMoveX(-this.movingBar.transform.position.x, 2f))
            .Append(this.movingBar.transform.DOMoveX(0f, 2f))
            .Append(this.movingBar.transform.DOMoveX(this.movingBar.transform.position.x, 2f))
            .SetLoops(-1);
    }
}
