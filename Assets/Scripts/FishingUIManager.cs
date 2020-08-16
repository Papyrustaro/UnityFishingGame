using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishingUIManager : MonoBehaviour
{
    [field: SerializeField]
    [field: RenameField("CurrentScoreText")]
    public Text CurrentScoreText { get; private set; }

    [field: SerializeField]
    [field: RenameField("FastInputScoreBonusText")]
    public Text FastInputScoreBonusText { get; private set; }

    [field: SerializeField]
    [field: RenameField("InputOccuracyScoreBonusText")]
    public Text InputOccuracyScoreBonusText { get; private set; }

    [field: SerializeField]
    [field: RenameField("RemainRoutineCountText")]
    public Text RemainRoutineCountText { get; private set; }


    [field: SerializeField]
    [field: RenameField("LimitInputOccuracyText")]
    public Text LimitInputOccuracyText { get; private set; }

    [field: SerializeField]
    [field: RenameField("ShowInputOccuracyText")]
    public Text ShowInputOccuracyText { get; private set; }




    public static FishingUIManager Instance { get; private set; }

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
}
