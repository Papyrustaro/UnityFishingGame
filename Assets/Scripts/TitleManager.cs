using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using KanKikuchi.AudioManager;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private InputField playerNameInputField;

    private void Start()
    {
        BGMManager.Instance.Play(BGMPath.BGM, isLoop: true, volumeRate: 0.8f);
        this.playerNameInputField.Select();
    }

    public void OnEndEditPlayerName()
    {
        if(this.playerNameInputField.text != "")
        {
            StaticData.playerName = this.playerNameInputField.text;
            SceneManager.LoadScene("Fishing");
        }
    }
}
