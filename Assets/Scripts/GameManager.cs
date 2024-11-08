using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Játékkezelő
public class GameManager : MonoBehaviour
{
    //Játékot elindító gomb
    public GameObject playButton;

    //Menübe léptető gomb
    public GameObject menuButton;

    //Játékos repülő
    public GameObject playerPlane;

    //Ellenség létrehozó
    public GameObject enemySpawner;

    //Vereség pop-up
    public GameObject GameOverGO;

    //Pontszámláló
    public GameObject scoreUITextGO;

    //Kiiktatás számláló
    public GameObject destroyedUITextGO;

    //Időt számláló UI
    public GameObject TimerCounterGO;

    //Cím kiirás
    public GameObject GameTitleGO;

    //Megállító gomb
    public GameObject PauseButton;

    //Felhető tárgyakat létrehozó
    public GameObject powerUpSpawner;

    //Főellenség asset
    public GameObject BossPrefab;

    //Főellenség objektum
    GameObject BossInstance;

    //Játék állapotok tipus
    public enum GameManagerState{

        //Kezdő menü
        Opening,

        //Játék
        Gameplay,

        //Játék vége
        GameOver,

        //Fő ellenség
        Bossfight,
    }

    //Játék állapota
    GameManagerState GMState;

    //Első frame update előtt van meghívva
    void Start()
    {
        GMState = GameManagerState.Opening;
    }

    //A játék állapotának megváltoztatása
    void UpdateGameManagerState(){

        switch(GMState)
        {
            //Kezdeti állapot beállítása
            case GameManagerState.Opening:

                playButton.SetActive(true);

                menuButton.SetActive(true);

                GameTitleGO.SetActive(true);

                GameOverGO.SetActive(false);

                PauseButton.SetActive(false);

                break;

            //Játékmenet beállítása
            case GameManagerState.Gameplay:

                scoreUITextGO.GetComponent<GameScore>().Score = 0;
                destroyedUITextGO.GetComponent<DestroyedEnemy>().Kills = 0;

                playButton.SetActive(false);

                menuButton.SetActive(false);

                GameTitleGO.SetActive(false);

                PauseButton.SetActive(true);

                playerPlane.GetComponent<PlayerControl>().Init();

                enemySpawner.GetComponent<EnemySpawner>().ScheduleEnemySpawner();

                powerUpSpawner.GetComponent<PowerUpSpawner>().SchedulePowerUpSpawner();

                TimerCounterGO.GetComponent<TimeCounter>().StartTimeCounter();
    
                break;
            //Játékvége beállítása
            case GameManagerState.GameOver:

                TimerCounterGO.GetComponent<TimeCounter>().StopTimeCounter();

                powerUpSpawner.GetComponent<PowerUpSpawner>().UnSchedulePowerUpSpawner();

                if(BossInstance == null){
                    enemySpawner.GetComponent<EnemySpawner>().UnScheduleEnemySpawner();
                    
                }else{
                    BossInstance.GetComponent<BomberBossControl>().DestroyFinalBoss();
                }

                GameOverGO.SetActive(true);

                PauseButton.SetActive(false);

                playerPlane.SetActive(false);

                Invoke("ChangeToOpeningState",8f);
                
                break;

            //Utolsó küzdelem megkezdése
            case GameManagerState.Bossfight:

                //Ellenséges repülők létrehozásának megállítása
                enemySpawner.GetComponent<EnemySpawner>().UnScheduleEnemySpawner();
                
                //Főellenség létrehozása
                BossInstance = Instantiate(BossPrefab);

                Vector2 position = Camera.main.ViewportToWorldPoint(new Vector2((float)0.5,1));

                BossInstance.transform.position = new Vector2(position.x, position.y + 2f);

                break;
        }
    }

    //Játék állapot beállítása adott állapotra
    public void SetGameManagerState(GameManagerState state)
    {
        GMState = state;
        UpdateGameManagerState();
    }

    //Játék állapotának beállítása játékmenetre
    public void StartGamePlay(){
        GMState = GameManagerState.Gameplay;
        UpdateGameManagerState();
    }

    //Játék beállítása kezdeti állapotra
    public void ChangeToOpeningState(){
        SetGameManagerState(GameManagerState.Opening);
    }

    //Új szint feloldása
    /*void UnlockNewLevel(){
        if(SceneManager.GetActiveScene().buildIndex>=PlayerPrefs.GetInt("ReachedIndex")){
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex +1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel") +1);
            PlayerPrefs.Save();
        }
    }*/
}
