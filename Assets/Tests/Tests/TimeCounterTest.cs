using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeCounterTest : MonoBehaviour
{
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

        // Ha nincs UI, akkor logoljunk
        Debug.Log("Számláló indítva: 00:00");
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

            // Kiírás a konzolra (UI helyett)
            Debug.Log(string.Format("Eltelt idő: {0:00}:{1:00}", minutes, seconds));
        }
    }
}
