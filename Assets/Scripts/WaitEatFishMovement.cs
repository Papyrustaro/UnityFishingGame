using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WaitEatFishMovement : MonoBehaviour
{
    private float defaultXPosition;
    Tweener _tweener;

    private void Awake()
    {
        this.defaultXPosition = this.transform.position.x;
        //this._tweener = this.transform.DOLocalMoveX(0.1f, 0.9f);
    }
    private void OnEnable()
    {
        this.transform.DOLocalMoveX(0.1f, 0.9f);
    }

    private void OnDisable()
    {
        this.transform.position = new Vector3(this.defaultXPosition, this.transform.position.y, this.transform.position.z);
    }
}
