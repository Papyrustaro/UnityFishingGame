using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using KanKikuchi.AudioManager;

public class TitleManager : MonoBehaviour
{
    private void Start()
    {
        BGMManager.Instance.Play(BGMPath.BGM, isLoop: true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("Fishing");
        }
    }
}
