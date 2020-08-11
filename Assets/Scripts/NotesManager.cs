using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Notesの出現処理など、Notesの管理
/// </summary>
public class NotesManager : MonoBehaviour
{
    /// <summary>
    /// タイミングアニメーションごとにひとつ。
    /// </summary>
    [SerializeField] private GameObject[] notePrefabs;
    private NotesObject[] notes;

    public static NotesManager Instance { private set; get; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            throw new System.Exception();
        }
    }

    /// <summary>
    /// タイミングを計るためのnotesObject生成。生成する種類と、再生速度はランダム。
    /// </summary>
    public void GenerateNote()
    {
        //とりあえずSetActiveで。Instantiateにするかも
        this.notes[UnityEngine.Random.Range(0, this.notes.Length)].Generate(UnityEngine.Random.Range(0.5f, 2f));
    }
}
