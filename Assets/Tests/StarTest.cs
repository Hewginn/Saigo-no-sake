using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarTest : MonoBehaviour
{
    // A csillag lefelé mozgásának sebessége
    public float speed;

    // Az első frame frissítése előtt hívódik meg
    void Start()
    {
        // Itt lehet kezdeti beállításokat elhelyezni, ha szükséges
    }

    // Frissítés minden frame során
    void Update()
    {
        // A csillag lefelé mozgatása
        MoveStarDownwards();
        
        // Ellenőrizzük, hogy a csillag kijutott-e a képernyőről, és szükség esetén visszaállítjuk a pozícióját
        CheckStarPosition();
    }

    // A csillag lefelé mozgatása a sebessége alapján
    private void MoveStarDownwards()
    {
        Vector2 position = transform.position;
        position.y -= speed * Time.deltaTime;  // Lefelé mozgás
        transform.position = position;           // A csillag pozíciójának frissítése
    }

    // A csillag pozíciójának visszaállítása, ha kijutott a képernyőről
    private void CheckStarPosition()
    {
        // A viewport minimum y értékének lekérése
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        // A viewport maximum y értékének lekérése
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        // Ha a csillag a képernyő alá kerül, állítsuk vissza a pozícióját
        if (transform.position.y < min.y)
        {
            // Visszaállítás egy véletlenszerű x pozícióra a képernyő szélességén belül, a képernyő tetején
            transform.position = new Vector2(Random.Range(min.x, max.x), max.y);
        }
    }
}
