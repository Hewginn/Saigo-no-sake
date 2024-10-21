using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playButton;
    public GameObject menuButton;
    public GameObject playerShip;
    public GameObject enemySpawner;
    public GameObject GameOverGO;
    public GameObject scoreUITextGO;
    public GameObject destroyedUITextGO;
    public GameObject TimerCounterGO;
    public GameObject GameTitleGO;
    public GameObject PauseButton;
    public GameObject powerUpSpawner;

    public enum GameManagerState{
        Opening,
        Gameplay,
        GameOver,
    }
    GameManagerState GMState;
    // Start is called before the first frame update
    void Start()
    {
        GMState = GameManagerState.Opening;
    }

    void UpdateGameManagerState(){

        switch(GMState)
        {
            case GameManagerState.Opening:

                playButton.SetActive(true);

                menuButton.SetActive(true);

                GameTitleGO.SetActive(true);

                GameOverGO.SetActive(false);

                PauseButton.SetActive(false);

                break;

            case GameManagerState.Gameplay:

                scoreUITextGO.GetComponent<GameScore>().Score = 0;
                destroyedUITextGO.GetComponent<DestroyedEnemy>().Kills = 0;

                playButton.SetActive(false);

                menuButton.SetActive(false);

                GameTitleGO.SetActive(false);
                PauseButton.SetActive(true);

                playerShip.GetComponent<PlayerControl>().Init();

                enemySpawner.GetComponent<EnemySpawner>().ScheduleEnemySpawner();

                powerUpSpawner.GetComponent<PowerUpSpawner>().SchedulePowerUpSpawner();

                TimerCounterGO.GetComponent<TimeCounter>().StartTimeCounter();
    
                break;

            case GameManagerState.GameOver:

                TimerCounterGO.GetComponent<TimeCounter>().StopTimeCounter();

                enemySpawner.GetComponent<EnemySpawner>().UnScheduleEnemySpawner();

                powerUpSpawner.GetComponent<PowerUpSpawner>().UnSchedulePowerUpSpawner();

                GameOverGO.SetActive(true);

                PauseButton.SetActive(false);

                Invoke("ChangeToOpeningState",8f);
                

                break;
        }
    }

    public void SetGameManagerState(GameManagerState state)
    {
        GMState = state;
        UpdateGameManagerState();
    }

    public void StartGamePlay(){
        GMState = GameManagerState.Gameplay;
        UpdateGameManagerState();
    }

    public void ChangeToOpeningState(){
        SetGameManagerState(GameManagerState.Opening);
    }
    /*void UnlockNewLevel(){
        if(SceneManager.GetActiveScene().buildIndex>=PlayerPrefs.GetInt("ReachedIndex")){
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex +1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel") +1);
            PlayerPrefs.Save();
        }
    }*/
}
