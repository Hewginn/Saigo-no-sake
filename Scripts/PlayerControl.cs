using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject PlayerBulletGO; //Lövedék objektuma
    public GameObject PlayerGun01; //Jobb oldali fegyver pozicióját megadó objektum
    public GameObject PlayerGun02; //Bal oldali fegyver pozicióját megadó objektum
    public float speed;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Mozgás (inputok: WASD vagy Nyilak)
        float x = Input.GetAxisRaw("Horizontal"); //Horizontális mozgás iránya {1 = jobbra, 0 = nem mozog, -1 = balra}
        float y = Input.GetAxisRaw("Vertical"); //Vertikális mozgás iránya {1 = felfele, 0 = nem mozog, -1 = lefele}
        Vector2 movementDirection = new Vector2 (x, y).normalized;
        Move(movementDirection);

        //Sima Tüzelés (input: 'Space')
        if(Input.GetKeyDown("space")){
            //Lövés a jobb oldali fegyverből
            GameObject firstBullet = (GameObject)Instantiate (PlayerBulletGO);
            firstBullet.transform.position = PlayerGun01.transform.position;
            //Lövés a bal oldali fegyverből
            GameObject secondBullet = (GameObject)Instantiate (PlayerBulletGO);
            secondBullet.transform.position = PlayerGun02.transform.position;
        }
    }

    void Move(Vector2 direction){

        //Képernyő határainak meghatározása
        Vector2 borderMin = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)); //Bal alsó sarok
        Vector2 borderMax = Camera.main.ViewportToWorldPoint(new Vector2(1, 1)); //Jobb felső sarok

        //Határok megadása úgy hogy a repülő fele ne lógjon ki a képernyőből
        borderMin.x = borderMin.x + 0.225f;
        borderMax.x = borderMax.x - 0.225f;
        borderMin.y = borderMin.y + 0.225f;
        borderMax.y = borderMax.y - 0.225f;

        //Új pozició meghatározása
        Vector2 position = transform.position; //Régi pozició
        position += direction * this.speed * Time.deltaTime; //Régi pozició + Elmozdulés irány * Sebesség * Eltelt idő = Új pozició

        //Határok alkamazása
        if(position.x < borderMin.x){
            position.x = borderMin.x;
        }else if(position.x > borderMax.x){
            position.x = borderMax.x;
        }
        if(position.y < borderMin.y){
            position.y = borderMin.y;
        }else if(position.y > borderMax.y){
            position.y = borderMax.y;
        }

        //Játékos poziciójának megváltoztatása
        transform.position = position;
    } 

}
