using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUIHandeler : MonoBehaviour
{

    TextMeshProUGUI ScoreUIGO;

    // Start is called before the first frame update
    void Start()
    {
        ScoreUIGO = GetComponent<TextMeshProUGUI>();
        UpdateScoreTextUI();
    }

    public void AddtoScore(int value){
        GameScore.AddScore(value);
        UpdateScoreTextUI();
    }

    public void ResetCurrent(){
        GameScore.NullCurrentScore();
        UpdateScoreTextUI();
    }
    void UpdateScoreTextUI()
    {
        string scoreStr = string.Format("{0:000000}", GameScore.getAll());
        ScoreUIGO.text = scoreStr;
    }
}
