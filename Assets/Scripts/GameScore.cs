using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;
using System.Runtime.CompilerServices;

//Pontszámlálóstatic 
public static class GameScore
{

    //Pontok
    static int currentScore = 0;
    static int sumScore = 0;

    //Pont hozzáadása
    public static void AddScore(int value){
        currentScore += value;
    }

    //Az éppenleges küldetésben szerzett pontok nullázása
    public static void NullCurrentScore(){
        currentScore = 0;
    }

    //Összpont nullázása
    public static void NullSumScore(){
        SaveScoreToJson();
        sumScore = 0;
    }

    //Eddig szerzett pontok
    public static int getSumScore(){
        return sumScore;
    }

    //
    public static void addCurrentToSum(){
        sumScore += currentScore;
        NullCurrentScore();
    }

    public static int getAll(){
        return currentScore + sumScore;
    }

    //A pontszám mentése JSON fájlba
    static private void SaveScoreToJson()
    {
        // a beolvasott fájl útvonala
        string jsonFile = File.ReadAllText(Application.dataPath + "/StreamingAssets/story.json");
        // a beolvasott fájl adatait eltároló változó
        Missions data = JsonUtility.FromJson<Missions>(jsonFile);

        int scoreHigh = sumScore;
        int switcher;
        //A highscore menübe való elmentése, megdölt-e egy rekord
        for (int i = 0; i < data.highscores.Length; i++)
        {
            if (scoreHigh > data.highscores[i])
            {
                switcher = data.highscores[i];
                data.highscores[i] = scoreHigh;
                scoreHigh = switcher;
            }
        }
        //A fájlba való kiírás
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/StreamingAssets/story.json", json);
    }
}
