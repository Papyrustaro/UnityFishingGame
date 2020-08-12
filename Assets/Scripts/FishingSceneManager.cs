using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 釣りゲーム中のManager
/// </summary>
public class FishingSceneManager : MonoBehaviour
{
    private E_FishingSceneState currentState = E_FishingSceneState.TryInputJustTime;

    public static FishingSceneManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            throw new Exception();
        }
    }

    private void Update()
    {
        if(this.currentState == E_FishingSceneState.TryInputJustTime && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(NotesManager.Instance.CurrentNotesObject.TimeFromJustToCurrent);
        }
    }
    public enum E_FishingSceneState
    {
        TryInputJustTime,
        ShowFishedThing,

    }
}
