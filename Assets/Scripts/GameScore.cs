using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Pontszámláló
public class GameScore : MonoBehaviour
{
    //Pontszámláló UI szöveg
    TextMeshProUGUI scoreTextUI;

    //Pontok
    int score;

    //Pontok setter, getter
    public int Score{

        get{
            return this.score;
        }
        set{
            this.score = value;
            UpdateScoreTextUI();
        }

    }

    //Első frame update előtt van meghívva
    void Start()
    {
        scoreTextUI = GetComponent<TextMeshProUGUI> ();
    }

    //Pontszámláló UI átírása
    void UpdateScoreTextUI(){
        string scoreStr = string.Format("{0:000000}",score);
        scoreTextUI.text = scoreStr;
    }

}
