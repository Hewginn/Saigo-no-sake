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
    }

    //Játék állapota
    GameManagerState GMState;



    string jsonFile;// a json fájl elérési útvonala
    Missions data; //a történetet, és más adatokat tartalmazó osztály

    //Első frame update előtt van meghívva
    void Start()
    {
        /*
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
        }*/

        // a beolvasndó fájl
        jsonFile = File.ReadAllText(Application.dataPath + "/Resources/story.json");
        // a beolvasott fájl adatait eltároló változó
        data = JsonUtility.FromJson<Missions>(jsonFile);
        MissionDescription();



        GMState = GameManagerState.Opening;
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

                PauseButton.SetActive(false);

                // Kiírjuk a szöveget a Mission objektumba
                MissionDescription();

                break;

            //Játékmenet beállítása
            case GameManagerState.Gameplay:

                scoreUITextGO.GetComponent<GameScore>().Score = data.score;
                destroyedUITextGO.GetComponent<DestroyedEnemy>().Kills = 0;

                playButton.SetActive(false);

                menuButton.SetActive(false);

                PauseButton.SetActive(true);

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

                enemySpawner.GetComponent<EnemySpawner>().UnScheduleEnemySpawner();

                powerUpSpawner.GetComponent<PowerUpSpawner>().UnSchedulePowerUpSpawner();

                GameOverGO.SetActive(true);

                PauseButton.SetActive(false);

                Invoke("ChangeToOpeningState", 8f);

                SaveToJson(scoreUITextGO.GetComponent<GameScore>().Score, false);

                break;

            //Küldetés teljesítve beállítások
            case GameManagerState.Win:

                TimerCounterGO.GetComponent<TimeCounter>().StopTimeCounter();

                enemySpawner.GetComponent<EnemySpawner>().UnScheduleEnemySpawner();

                PauseButton.SetActive(false);

                MissionSuccessed();

                menuButton.SetActive(true);

                nextLevelButton.SetActive(true);

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
        if (id == 1)
        {
            description.text = data.missions[0].description;
        }

        else if (id == 2)
        {
            description.text = data.missions[1].description;
        }
        else if (id == 3)
        {
            description.text = data.missions[2].description;
        }
    }
    // a küldetés sikerrel zárult történetének a betöltése a Mission objektumba küldetésenként
    public void MissionSuccessed()
    {

        description.enabled = true;
        if (id == 1)
            description.text = data.missions[0].successed;
        else if (id == 2)
        {
            description.text = data.missions[1].successed;
        }
        else if (id == 3)
        {
            description.text = data.missions[2].successed;
        }
    }



    public void SaveToJson(int score, bool success)
    {
        Missions data1 = data;
        if (success)
        {
            data1.score += score;
        }
        else { data1.score = 0; }
        for (int i = 0; i < data.highscores.Length; i++)
        {
            if (score > data1.highscores[i])
            {
                data1.highscores[i] = score;
                break;
            }
        }

        string json = JsonUtility.ToJson(data1, true);
        File.WriteAllText(Application.dataPath + "/Resources/story.json", json);
    }
}

