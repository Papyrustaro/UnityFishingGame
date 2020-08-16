using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SelectButtonOnEnable : MonoBehaviour
{
    private Button _button;
    private void Awake()
    {
        this._button = GetComponent<Button>();
    }
    private void OnEnable()
    {
        StartCoroutine(CoroutineManager.DelayMethodRealTime(1, () => this._button.Select()));
    }
}
