using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameScore : MonoBehaviour
{
       
    TextMeshProUGUI scoreTextUI;

    int score;

    public int Score{

        get{
            return this.score;
        }
        set{
            this.score = value;
            UpdateScoreTextUI();
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        scoreTextUI = GetComponent<TextMeshProUGUI> ();
    }
    void UpdateScoreTextUI(){
        string scoreStr = string.Format("{0:000000}",score);
        scoreTextUI.text = scoreStr;
        
    }

}
