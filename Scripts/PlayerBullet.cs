using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{

    float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = 8f;
    }

    // Update is called once per frame
    void Update()
    {
        //Lövedék aktuális pozíciójának megadása
        Vector2 position = transform.position;

        //Új pozíció megadása
        position.y += speed * Time.deltaTime;
        transform.position = position;

        //Lövedék törlése a memóriából, ha képernyőn kívülre kerül
        Vector2 limit = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        if(transform.position.y > limit.y){
            Destroy(gameObject);
        }
    }
}
