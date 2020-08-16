using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WaitEatFishMovement : MonoBehaviour
{
    private float defaultXPosition;

    private void Awake()
    {
        this.defaultXPosition = this.transform.position.x;
    }
    private void OnEnable()
    {
        this.transform.DOLocalMoveX(0.1f, 1f);
    }

    private void OnDisable()
    {
        this.transform.position = new Vector3(this.defaultXPosition, this.transform.position.y, this.transform.position.z);
    }
}
