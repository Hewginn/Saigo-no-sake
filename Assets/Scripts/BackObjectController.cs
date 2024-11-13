using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Háttér objektum létrehozó
public class BackObjectController : MonoBehaviour
{
    //Háttér objektum prefab
    public GameObject backObject;

    //Háttérobjektum képek
    public Sprite [] sprites;

    //Pozíciója a legutóbbi objektumnak
    Vector2 lastPosition;



    // Háttér objektum spawner inicializálása
    void Start()
    {
        lastPosition = Camera.main.ViewportToWorldPoint(new Vector2((float)0.5,1));

        Invoke("makeNewObject", 5f);
    }

    //Új háttér objektum létrehozása
    void makeNewObject(){

        //Határok meghatározása
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1,1));
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0,0));

        //Pozíció X értének meghatározása úgy hogy semmikép se fedje az előző objektumot
        float makeX;
        do{
            makeX = Random.Range(min.x, max.x);
        }while((lastPosition.x - 4f < makeX) && (lastPosition.x + 4f > makeX));

        //Elforgatás értékei
        float [] angles = {45, 90, 135, 180, 225, 270};

        //Pozíció meghatározása
        Vector2 position = new Vector2(makeX, max.y + 2f);

        //Új objektum létrehozása
        GameObject newObject = (GameObject)Instantiate(backObject);
        newObject.GetComponent<BackObject>().Init(
            position,
            sprites[Random.Range(0, sprites.Length)],
            angles[Random.Range(0, angles.Length)]);

        //Pozíció meghatározása
        lastPosition = position;

        //Következő objektum létrehozása
        Invoke("makeNewObject", Random.Range(2f, 5f));
    }
}
