using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NCMB;
using System;
using UnityEngine.SceneManagement;

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

    [SerializeField] private GameObject announcePanel;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private Text rankingPlayerNameText;
    [SerializeField] private Text rankingScoreText;
    [SerializeField] private Text playerResultText;
    private int thisTimePlayerRank = -1;

    private bool sendPlayerData = false;
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

    public void OnGameOver()
    {
        this.ShowInputOccuracyText.text = "GameOver";
        this.playerResultText.text = "あなたのスコア: " + FishingSceneManager.Instance.CurrentScore + "点";
        StartCoroutine(SetHighRankingTextFromClearResult());
        StartCoroutine(CoroutineManager.DelayMethod(1f, () =>
        {
            this.announcePanel.SetActive(false);
            this.resultPanel.SetActive(true);
        }));
    }

    public void OnPressContinueButton()
    {
        this.SavePlayerResult();
        SceneManager.LoadScene("Fishing");
    }

    public void OnPressGoTitleButton()
    {
        this.SavePlayerResult();
        SceneManager.LoadScene("Title");
    }

    public void SavePlayerResult()
    {
        this.sendPlayerData = true;
        NCMBObject obj = new NCMBObject("Ranking");
        obj["PlayerName"] = StaticData.playerName;
        obj["Score"] = FishingSceneManager.Instance.CurrentScore;
        obj.SaveAsync();
    }

    public IEnumerator SetHighRankingTextFromClearResult()
    {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("Ranking");
        List<NCMBObject> result = null;
        NCMBException error = null;

        query.OrderByAscending("Score"); //昇順
        query.Limit = 100;

        query.FindAsync((List<NCMBObject> _result, NCMBException _error) =>
        {
            result = _result;
            error = _error;
        });

        //resultもしくはerrorが入るまで待機
        yield return new WaitWhile(() => result == null && error == null);

        //後続処理
        if (error == null)
        {
            this.SetHighRankingTextFromClearResult(result);
        }
        else
        {
            Debug.Log(error);
        }
    }

    public void SetHighRankingTextFromClearResult(List<NCMBObject> highRanks)
    {
        bool rankined = false;
        string playerName = "";
        string resultScore = "";
        int highRanksCount = highRanks.Count;
        int playerScore = FishingSceneManager.Instance.CurrentScore;

        List<float> resultScores = new List<float>();
        int thisTimeIndex = highRanksCount; //今回クリアしたプレイヤーの順位
        for (int i = 0; i < highRanksCount; i++)
        {
            if (float.Parse(highRanks[i]["Score"].ToString()) > playerScore)
            {
                thisTimeIndex = i;
                break;
            }
        }
        if (highRanksCount < 100) highRanksCount++;

        for (int i = 0; i < highRanksCount; i++)
        {
            if (i == thisTimeIndex) //playerの順位
            {
                playerName += (i + 1).ToString() + "." + StaticData.playerName + "\n";
                resultScore += playerScore + "\n";
                this.thisTimePlayerRank = i + 1;
                rankined = true;
            }
            else if (rankined) //playerがランクインしたあとの
            {
                playerName += (i + 1).ToString() + "." + highRanks[i - 1]["PlayerName"].ToString() + "\n";
                resultScore += float.Parse((highRanks[i - 1]["Score"]).ToString()) + "\n";
            }
            else //playerがランクインする前
            {
                playerName += (i + 1).ToString() + "." + highRanks[i]["PlayerName"].ToString() + "\n";
                resultScore += float.Parse((highRanks[i]["Score"]).ToString()) + "\n";
            }
        }

        this.rankingPlayerNameText.text = playerName;
        this.rankingScoreText.text = resultScore;
    }

    public void Tweeting()
    {
        string tweetText = "";
        if (this.thisTimePlayerRank != -1) tweetText = "【現在" + this.thisTimePlayerRank.ToString() + "位】";
        tweetText += StaticData.playerName + "さんのスコア: " + FishingSceneManager.Instance.CurrentScore + "点";

        string url = "https://twitter.com/intent/tweet?"
            + "text=" + tweetText
            + "&url=" + "https://unityroom.com/games/spacejumpgame"
            + "&hashtags=" + "エビ,unityroom";

#if UNITY_EDITOR
        Application.OpenURL(url);
#elif UNITY_WEBGL
            // WebGLの場合は、ゲームプレイ画面と同じウィンドウでツイート画面が開かないよう、処理を変える
            Application.ExternalEval(string.Format("window.open('{0}','_blank')", url));
#else
            Application.OpenURL(url);
#endif
    }
}
