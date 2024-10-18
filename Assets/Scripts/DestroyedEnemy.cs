using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DestroyedEnemy : MonoBehaviour
{
       public GameObject GameManagerGO;
       public GameObject Player;
       

    TextMeshProUGUI scoreTextUI;

    int score;

    public int Kills{

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
        string scoreStr = string.Format("{0:0}",score);
        scoreTextUI.text = scoreStr;
        if(scoreStr=="100" ){
                GameManagerGO.GetComponent<GameManager>().SetGameManagerState(GameManager.GameManagerState.GameOver);
                Player.SetActive(false);
                UnlockNewLevel();
            }
    }

    void UnlockNewLevel(){
        if(SceneManager.GetActiveScene().buildIndex>= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex +1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnclokedLevel", 1)+1);
            PlayerPrefs.Save();
        }
    }



}