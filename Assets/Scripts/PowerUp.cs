using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        MoveForward();
    }

    //Az objektum folyamatos lefele mozgását leíró függvény
    void MoveForward(){

        Vector2 position = transform.position;

        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0,0));

        position.y -= speed * Time.deltaTime;
        transform.position = position;
        
        if(transform.position.y < min.y){
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.tag == "PlayerShipTag"){
            Destroy(gameObject);
        }
    }
}
