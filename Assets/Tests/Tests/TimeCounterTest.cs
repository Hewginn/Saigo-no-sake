using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Időszámláló
public class TimeCounterTest : MonoBehaviour
{
    // UI szöveg a idő megjelenítésére
    private TextMeshProUGUI timeUI;

    // Kezdő idő, amikor a számláló elindul
    private float startTime;

    // Eltelt idő a számláló indítása óta
    private float elapsedTime;

    // Logikai változó, hogy a számláló fut-e
    private bool isCounterRunning;

    // Eltelt idő percei és másodpercei
    private int minutes;
    private int seconds;

    // Az első frame frissítése előtt hívódik meg
    void Start()
    {
        // Inicializálás
        isCounterRunning = false;
        timeUI = GetComponent<TextMeshProUGUI>();
        timeUI.text = "00:00"; // A szöveg alapértelmezett értéke
    }

    // A számláló elindítása
    public void StartTimeCounter()
    {
        if (!isCounterRunning) // Megakadályozza a többszöri indítást, ha már fut
        {
            startTime = Time.time;
            isCounterRunning = true;
        }
    }

    // A számláló leállítása
    public void StopTimeCounter()
    {
        isCounterRunning = false;
    }

    // Frissítés minden frame során
    void Update()
    {
        if (isCounterRunning)
        {
            // Eltelt idő kiszámítása
            elapsedTime = Time.time - startTime;

            // Eltelt idő percek és másodpercek átkonvertálása
            minutes = (int)(elapsedTime / 60);
            seconds = (int)(elapsedTime % 60);

            // UI szöveg frissítése
            timeUI.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
