using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;
using System.Timers;
using Unity.VisualScripting;
using System.IO;

//Játékos irányítása, kezelése
public class PlayerControl : MonoBehaviour
{
    //Játék kezelő
    public GameObject GameManagerGO;

    //ez a játékos előgyártott lövedéke
    public GameObject PlayerBulletGO;

    //Játékos fegyvereinek pozíciója
    public GameObject bulletPosition01;
    public GameObject bulletPosition02;
    public GameObject bulletPosition03;
    public GameObject bulletPosition04;
    public GameObject bulletPosition05;
    public GameObject bulletPosition06;

    //Fegyverek fejlesztési szintje
    int upgradeLevel;

    //Speciális lövés kezdeti pozíciója
    public GameObject specialPosition;

    //Robbanás
    public GameObject ExpolsionGO;

    //Speciális lövedék
    public GameObject SpecialGO;

    //Élet szám UI szöveg
    public TextMeshProUGUI LivesUIText;

    //Különleges lövedékek száma UI szöveg
    public TextMeshProUGUI SpecialsUIText;

    //Pontszámláló UI szöveg
    public GameObject scoreUITextGO;

    //Maximum élet
    const int maxLives = 3;

    //Aktuális élet
    int lives;

    //Repülő gyorsasága
    public float speed;

    //A repülő éppen halhatatlan-e
    bool isInvincible = false;

    //Maximális tárolható különleges lövedék
    int maxSpecial = 5;

    //Aktuális különleges lövedék
    int specials;

    //Legutóbbi lövés időpontja
    float lastShoot;

    //A játékos tudja irányítani a gépet
    bool hasControl;

    //Repülő inicializálása
    public void Init(){

        //Élet
        lives = maxLives;
        LivesUIText.text=lives.ToString() + " X";

        //Helyzet
        transform.position = new Vector2(0,0);

        //Aktiválás
        gameObject.SetActive(true);

        //Fejlesztési szint
        upgradeLevel = 0;

        //Különleges lövedékek száma
        specials = 0;
        SpecialsUIText.text = specials.ToString() + " X";

        //Irányítás megkapása
        hasControl = true;

    }

    // Update is called once per frame
    void Update()
    {
        // a szóköz billentyű lenyomására lő az űrhajó 0.25 másodpercenként
        if(Input.GetKey("space") && Time.time - lastShoot > 0.25f && hasControl){
            GetComponent<AudioSource>().Play();
            GameObject bullet01= (GameObject)Instantiate(PlayerBulletGO);
            bullet01.transform.position = bulletPosition01.transform.position;
            GameObject bullet02= (GameObject)Instantiate(PlayerBulletGO);
            bullet02.transform.position = bulletPosition02.transform.position;

            //1. szintű fejlesztés búnusz lövedékei
            if(upgradeLevel > 0){
                GameObject bullet03 = (GameObject)Instantiate(PlayerBulletGO);
                bullet03.transform.position = bulletPosition03.transform.position;
                GameObject bullet04= (GameObject)Instantiate(PlayerBulletGO);
                bullet04.transform.position = bulletPosition04.transform.position;
            }

            //2. szintű fejlesztés bónusz lövedékei
            if(upgradeLevel > 1){
                GameObject bullet05 = (GameObject)Instantiate(PlayerBulletGO);
                bullet05.transform.position = bulletPosition05.transform.position;
                GameObject bullet06 = (GameObject)Instantiate(PlayerBulletGO);
                bullet06.transform.position = bulletPosition06.transform.position;
            }

            //Lövés időpontjának megadása
            lastShoot = Time.time;
        }

        //Különleges lövedék lövése 'E' billentyűvel
        if(Input.GetKeyDown("e") && specials > 0 && hasControl){
            GameObject bomb = (GameObject) Instantiate(SpecialGO);
            bomb.GetComponent<PlayerSpecial>().Init(specialPosition.transform.position, new Vector2(0,1), 6f);
            specials--;
            SpecialsUIText.text = specials.ToString() + " X";
        }

        // az ertek -1 (balra nyil), 0 (nincs gomb megnyomva) vagy 1 (jobbra nyil) lesz 
        float x = Input.GetAxisRaw("Horizontal");
        // az ertek -1 (le nyil), 0 (nincs gomb megnyomva) vagy 1 (fel nyil) lesz
        float y = Input.GetAxisRaw("Vertical"); 

        // a bekert adatok szerint kiszamolunk egy egyseg es egy irany vektort
        Vector2 direction = new Vector2 (x,y).normalized;

        // ez szamolja ki a karakter mozgasat
        Move(direction);
    }

    //Mozgás irány vektorral
    void Move(Vector2 direction){
        
        //Képernyő határainak meghatározása
        Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2(0,0));
        Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2(1,1));
        max.x = max.x -0.225f; 
        min.x = min.x +0.225f;
        max.y = max.y - 0.285f;
        min.y = min.y + 0.285f;

        //Aktuális pozíció
        Vector2 pos = transform.position; 

        //Új pozíció
        pos += direction *speed * Time.deltaTime; 
        
        //Határok alkalmazása
        pos.x= Mathf.Clamp (pos.x,min.x,max.x);
        pos.y= Mathf.Clamp (pos.y,min.y,max.y);

        //Új pozíció átadása az objektumnak
        transform.position = pos; 
    }

    //Ütközési esemény kezelő
    void OnTriggerEnter2D(Collider2D col){

        //Sebződés kezelése
        if(((col.tag == "EnemyShipTag") || (col.tag == "EnemyBulletTag") || (col.tag == "BossBlastTag") || col.tag == "BossEngines") && !isInvincible){

            //Robbanás lejátszása
            PlayerExplosion();

            //Élet csökkentése
            lives = lives > 0 ? lives - 1 : 0;
            LivesUIText.text = lives.ToString() + " X";

            //Fejlesztés nullázása
            upgradeLevel = 0;

            if(lives == 0){

                //0 élettel játék elvesztése
                GameManagerGO.GetComponent<GameManager>().SetGameManagerState(GameManager.GameManagerState.GameOver);
                gameObject.SetActive(false);
                

            }else{

                //Sebződés után játékos 2mp-ig sebezhetetlen
                InvincibleModeOn();
                Invoke("InvincibleModeOff", 2f);

            }

        //Tárgyak felvevése (ha maxon van +500 pont)
        }else switch(col.tag){

            //Fejlesztés felvevésével fejlesztés növelése
            case "UpgradePU":
                if(upgradeLevel < 2){
                    upgradeLevel++;
                }else{
                    scoreUITextGO.GetComponent<GameScore>().Score += 500;
                }
                break;
            
            //Gyógyítás felvevésével életnövelése
            case "HealPU":
                if(lives < maxLives){
                    lives++;
                    LivesUIText.text = lives.ToString() + " X";
                }else{
                    scoreUITextGO.GetComponent<GameScore>().Score += 500;
                }
                break;

            //Különleges lövedék növelése
            case "SpecialPU":
                if(specials < maxSpecial){
                    specials++;
                    SpecialsUIText.text = specials.ToString() + " X";
                }else{
                    scoreUITextGO.GetComponent<GameScore>().Score += 500;
                }
                break;          
        }
    }

    //Robbanás lejátszása
    void PlayerExplosion(){
        GameObject explosion = (GameObject)Instantiate(ExpolsionGO);

        explosion.transform.position = transform.position;
    }

    //Halhatatlanság be
    void InvincibleModeOn(){

        //Halhatatlanság funkció be
        isInvincible = true;
        gameObject.tag = "PlayerUndamagable";

        //Vizuális effekt be
        InvokeRepeating("Flash", 0f, 0.25f);

    }

    //Halhatatlanság ki
    void InvincibleModeOff(){

        //Halhatatlanság funkció ki
        gameObject.tag = "PlayerShipTag";
        isInvincible = false;

        //Vizuális effekt ki
        CancelInvoke("Flash");
        GetComponent<Renderer>().enabled = true;
    }

    //Repölő villantása
    void Flash(){
        GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
    }

    public void loseControl(){
        hasControl = false;
        transform.position = Camera.main.ViewportToWorldPoint(new Vector2((float)0.5,0));
        transform.position = new Vector2 (transform.position.x, transform.position.y + GetComponent<SpriteRenderer>().bounds.size.y / 2);
        if(isInvincible){
            CancelInvoke("InvincibleModeOff");
            InvincibleModeOff();
        }
        gameObject.tag = "PlayerUndamagable";
        isInvincible = true;
    }
}
