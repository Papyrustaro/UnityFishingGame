using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NotesObject : MonoBehaviour
{
    /// <summary>
    /// アニメーション再生速度(ノーツ速度)
    /// </summary>
    public float PlaySpeed { get; private set; } = 1f;

    public void Generate(float playSpeed)
    {
        this.PlaySpeed = playSpeed;
        this.gameObject.SetActive(this);
    }
}
