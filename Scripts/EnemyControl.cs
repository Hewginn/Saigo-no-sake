using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed  = 2f;    
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMoveForward();
    }

    void EnemyMoveForward(){

        //Ellenfél új pozíciójának meghatározása
        Vector2 position = transform.position;
        position.y -= speed * Time.deltaTime;
        transform.position = position;

        //Ellenfél törlése ha elérte a képernyő szélét
        Vector2 limit = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        limit.y = limit.y - 0.225f;
        if(position.y < limit.y){
            Destroy(gameObject);
        }

    }
}
