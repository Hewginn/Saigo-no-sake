using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using System;

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

    //Megállító gomb
    public GameObject PauseButton;

    //Felhető tárgyakat létrehozó
    public GameObject powerUpSpawner;

    //A következő szint betöltése gomb
    public GameObject nextLevelButton;

    //A pálya száma történetmesélés és küldetés szempontjából
    public int id; 

    // a küldetés leírása és a küldetés teljeítésének szövege
    public TextMeshProUGUI description; 

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
        //Küldetés teljesítve
        Win,

        //Fő ellenség
        Bossfight,
    }

    //Játék állapota
    GameManagerState GMState;

    Missions story; //a történetet tárolja a json fájlból

    //Első frame update előtt van meghívva
    void Start()
    {
        // A fájl betöltése a Resources mappából
        TextAsset jsonFile = Resources.Load<TextAsset>("story");
        if (jsonFile != null)
        {
        // Deszerializáljuk a JSON-t a C# osztályba
        story = JsonUtility.FromJson<Missions>(jsonFile.text); 
        // Kiírjuk a szöveget a Mission objektumba
        MissionDescription();   
         
        }
    else
        {
            Debug.LogError("Nem található a JSON fájl!");
        }

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

                GameOverGO.SetActive(false);

                PauseButton.SetActive(false);

                // Kiírjuk a szöveget a Mission objektumba
                MissionDescription();

                break;

            //Játékmenet beállítása
            case GameManagerState.Gameplay:

                scoreUITextGO.GetComponent<GameScore>().Score = 0;
                destroyedUITextGO.GetComponent<DestroyedEnemy>().Kills = 0;

                playButton.SetActive(false);

                menuButton.SetActive(false);

                PauseButton.SetActive(true);

                // a küldetés szövegének eltüntetése
                description.enabled= false;

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
            
            //Küldetés teljesítve beállítások
             case GameManagerState.Win:

                TimerCounterGO.GetComponent<TimeCounter>().StopTimeCounter();

                enemySpawner.GetComponent<EnemySpawner>().UnScheduleEnemySpawner();

                PauseButton.SetActive(false);
                
                MissionSuccessed();

                menuButton.SetActive(true);

                nextLevelButton.SetActive(true);

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

     // a küldetés leírásának betöltése a Mission objektumba küldetésenként
    public void MissionDescription(){
        description.enabled= true;
        if(id==1){
            description.text = story.missions[0].description;
        }

        else if(id==2){
            description.text = story.missions[1].description;
        }
        else if(id==3){
            description.text = story.missions[2].description;
        }
    }
    // a küldetés sikerrel zárult történetének a betöltése a Mission objektumba küldetésenként
     public void MissionSuccessed(){
        description.enabled= true;
        if(id==1)
          description.text = story.missions[0].successed;
        else if(id==2){
            description.text = story.missions[1].successed;
        }
        else if(id==3){
            description.text = story.missions[2].successed;
        }
    }
}
