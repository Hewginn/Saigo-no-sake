using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

//Játékkezelő
public class GameManager : MonoBehaviour
{
    // Pause menu 
    [SerializeField] GameObject pauseMenu;

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

    //Felhető tárgyakat létrehozó
    public GameObject powerUpSpawner;

    //A következő szint betöltése gomb
    public GameObject nextLevelButton;

    // a küldetés leírása és a küldetés teljeítésének szövege
    public TextMeshProUGUI description;

    //Főellenség asset
    public GameObject BossPrefab;

    //Főellenség objektum
    GameObject BossInstance;

    //A páya száma
    public int level;

    //Kiiktatást számláló UI szövege
    GameObject killsUITextGO;
    //Az időzítő értéke
    GameObject timeUITextGO;

    //Játék állapotok tipus
    public enum GameManagerState
    {

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

    string jsonFile;// a json fájl elérési útvonala

    Missions data; //a történetet tárolja a json fájlból

    bool pressed;// annak az ellenőrzése,hogy az escape billentyűt lenyomták-e már egyszer vagy sem

    //Első frame update előtt van meghívva
    void Start()
    {
        // a beolvasott fájl útvonala
        jsonFile = File.ReadAllText(Application.dataPath + "/Resources/story.json");
        // a beolvasott fájl adatait eltároló változó
        data = JsonUtility.FromJson<Missions>(jsonFile);
        pressed = false;
        MissionDescription();



        GMState = GameManagerState.Opening;

        //A elpusztított elenséget számoló UI megnevezése
        killsUITextGO = GameObject.FindGameObjectWithTag("DestroyedEnemies");
        //Az időt számoló UI megnevezése
        timeUITextGO = GameObject.FindGameObjectWithTag("TimerText");
    }

    void Update()
    {
        //ha lenyomjuk az escape billentyűt először és már játékban vagyunk
        if (Input.GetKey(KeyCode.Escape) && (GMState == GameManagerState.Gameplay) && (pressed == false))
        {
            // a pause menü megjelenítése
            pauseMenu.SetActive(true);
            // a játék megállítása
            Time.timeScale = 0;
            //megnyomták már az escape billenyűt
            pressed = true;

        }
        //ha lenyomjuk már az escape billentyűt egyszer és már játékban vagyunk, és a két billentyű leütés között eltelt ez bizonyos idő nagyság
        /*else if (Input.GetKey(KeyCode.Escape) && (GMState == GameManagerState.Gameplay) && (pressed == true))
        {
            // a pause menü megjelenítése
            pauseMenu.SetActive(false);
            // a játék megállítása
            Time.timeScale = 1;
            //megnyomták már az escape billenyűt
            StartCoroutine(Delay());
            pressed = false;
        }*/

    }

    //A játék állapotának megváltoztatása
    void UpdateGameManagerState()
    {

        switch (GMState)
        {
            //Kezdeti állapot beállítása
            case GameManagerState.Opening:

                playButton.SetActive(true);

                menuButton.SetActive(true);

                GameOverGO.SetActive(false);

                // Kiírjuk a szöveget a Mission objektumba
                MissionDescription();

                break;

            //Játékmenet beállítása
            case GameManagerState.Gameplay:

                scoreUITextGO.GetComponent<GameScore>().Score = data.score;
                destroyedUITextGO.GetComponent<DestroyedEnemy>().Kills = 0;

                playButton.SetActive(false);

                menuButton.SetActive(false);

                // a küldetés szövegének eltüntetése
                description.enabled = false;

                playerPlane.GetComponent<PlayerControl>().Init();

                enemySpawner.GetComponent<EnemySpawner>().ScheduleEnemySpawner();

                powerUpSpawner.GetComponent<PowerUpSpawner>().SchedulePowerUpSpawner();

                TimerCounterGO.GetComponent<TimeCounter>().StartTimeCounter();

                break;
            //Játékvége beállítása
            case GameManagerState.GameOver:

                TimerCounterGO.GetComponent<TimeCounter>().StopTimeCounter();

                powerUpSpawner.GetComponent<PowerUpSpawner>().UnSchedulePowerUpSpawner();

                if (BossInstance == null)
                {
                    enemySpawner.GetComponent<EnemySpawner>().UnScheduleEnemySpawner();

                }
                else
                {
                    BossInstance.GetComponent<BomberBossControl>().DestroyFinalBoss();
                }

                GameOverGO.SetActive(true);

                playerPlane.SetActive(false);

                SaveToJson(scoreUITextGO.GetComponent<GameScore>().Score, false);

                Invoke("ChangeToOpeningState", 8f);

                break;

            //Utolsó küzdelem megkezdése
            case GameManagerState.Bossfight:

                //Ellenséges repülők létrehozásának megállítása
                enemySpawner.GetComponent<EnemySpawner>().UnScheduleEnemySpawner();

                //Főellenség létrehozása
                BossInstance = Instantiate(BossPrefab);

                Vector2 position = Camera.main.ViewportToWorldPoint(new Vector2((float)0.5, 1));

                BossInstance.transform.position = new Vector2(position.x, position.y + 2f);

                break;

            //Küldetés teljesítve beállítások
            case GameManagerState.Win:

                TimerCounterGO.GetComponent<TimeCounter>().StopTimeCounter();

                enemySpawner.GetComponent<EnemySpawner>().UnScheduleEnemySpawner();

                powerUpSpawner.GetComponent<PowerUpSpawner>().UnSchedulePowerUpSpawner();

                MissionSuccessed();

                menuButton.SetActive(true);

                nextLevelButton.SetActive(true);

                playerPlane.SetActive(false);

                SaveToJson(scoreUITextGO.GetComponent<GameScore>().Score, true);

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
    public void StartGamePlay()
    {
        GMState = GameManagerState.Gameplay;
        UpdateGameManagerState();
    }

    //Játék beállítása kezdeti állapotra
    public void ChangeToOpeningState()
    {
        SetGameManagerState(GameManagerState.Opening);
    }

    // a küldetés leírásának betöltése a Mission objektumba küldetésenként
    public void MissionDescription()
    {

        description.enabled = true;
        if (level == 1)
        {
            description.text = data.missions[0].description;
        }

        else if (level == 2)
        {
            description.text = data.missions[1].description;
        }
        else if (level == 3)
        {
            description.text = data.missions[2].description;
        }
    }
    // a küldetés sikerrel zárult történetének a betöltése a Mission objektumba küldetésenként
    public void MissionSuccessed()
    {

        description.enabled = true;
        if (level == 1)
            description.text = data.missions[0].successed;
        else if (level == 2)
        {
            description.text = data.missions[1].successed;
        }
        else if (level == 3)
        {
            description.text = data.missions[2].successed;
        }
    }


    //A pontszám mentése JSON fájlba
    public void SaveToJson(int score, bool success)
    {
        //Ha sikeres a küldetés, akkor elmentjük az összegyűjtött pontokat, különben lenullázuk
        Missions data_new = data;
        if (success)
        {
            data_new.score = score;
        }
        else { data_new.score = 0; }
        //A highscore menübe való elmentése, megdölt-e egy rekord
        for (int i = 0; i < data_new.highscores.Length; i++)
        {
            if (score > data_new.highscores[i])
            {
                data_new.highscores[i] = score;
                break;
            }
        }

        //A fájlba való kiírás
        string json = JsonUtility.ToJson(data_new, true);
        File.WriteAllText(Application.dataPath + "/Resources/story.json", json);
    }

    public void Missions()
    {
        int i; //a nehézségi szint száma a tömbben

        // a nehézségi szint kiválasztása
        for (i = 0; i < data.difficulty.Length; i++)
        {
            if (data.difficulty[i].type == data.choosed_difficulty)
            {
                break;
            }
        }

        //UI frissítés
        if (level == 1)
        {
            //killsUITextGO.GetComponent<DestroyedEnemy>().UpdateDestroyedTextUI();


            //Az első küldetés teljesítésének feltétele
            if (killsUITextGO.GetComponent<DestroyedEnemy>().Kills == data.difficulty[i].enemynumber_first_level)
            {

                //Játék győzelemmel zárult
                SetGameManagerState(GameManagerState.Win);
                //Player.SetActive(false);

                //Új szint feloldása
                //UnlockNewLevel();

            }

        }
        //A második Küldetés teljesítésének feltétele
        else if (level == 2)
        {

            if ((int)timeUITextGO.GetComponent<TimeCounter>().ellapsedTime % 60 == 2)
            {
                //Játék győzelemmel zárult
                SetGameManagerState(GameManagerState.Win);
            }


        }
        //A harmadik küldetés teljesítésének feltétele
        else if (level == 3)
        {


            if (killsUITextGO.GetComponent<DestroyedEnemy>().Kills == data.difficulty[i].enemynumber_last_level)
            {
                //Végső ellenség elleni csata kezdete
                SetGameManagerState(GameManagerState.Bossfight);


            }
        }
    }
}


