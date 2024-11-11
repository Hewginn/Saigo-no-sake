using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackObjectController : MonoBehaviour
{
    //Háttér objektum prefab
    public GameObject backObject;

    //Háttérobjektum képek
    public Sprite [] sprites;



    // Start is called before the first frame update
    void Start()
    {
        Invoke("makeNewObject", 5f);
    }

    void makeNewObject(){

        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1,1));
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0,0));
        float makeX = Random.Range(min.x, max.x);

        float [] angles = {45, 90, 135, 180, 225, 270};

        GameObject newObject = (GameObject)Instantiate(backObject);
        newObject.GetComponent<BackObject>().Init(
            new Vector2(makeX, max.y + 2f),
            sprites[Random.Range(0, sprites.Length)],
            angles[Random.Range(0, angles.Length)]);
        Invoke("makeNewObject", Random.Range(2f, 5f));
    }
}
