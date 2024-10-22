using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;
using System.Timers;

public class PlayerControl : MonoBehaviour
{
    public GameObject GameManagerGO;
    public GameObject PlayerBulletGO; //ez a játékos előgyártott lövedéke
    public GameObject bulletPosition01;
    public GameObject bulletPosition02;
    int upgradeLevel;
    public GameObject bulletPosition03;
    public GameObject bulletPosition04;
    public GameObject bulletPosition05;
    public GameObject bulletPosition06;
    public GameObject specialPosition;
    public GameObject ExpolsionGO;
    public GameObject SpecialGO;
    public TextMeshProUGUI LivesUIText;
    public TextMeshProUGUI SpecialsUIText;
    public GameObject scoreUITextGO;
    const int maxLives = 3;
    int lives;
    public float speed;
    bool isInvincible = false;
    int maxSpecial = 5;
    int specials;

    public void Init(){
        lives = maxLives;

        LivesUIText.text=lives.ToString();

        transform.position = new Vector2(0,0);

        gameObject.SetActive(true);

        upgradeLevel = 0;

        specials = 0;

        SpecialsUIText.text = "X " + specials.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // a szóköz billentyű lenyomására lő az űrhajó
        if(Input.GetKeyDown("space")){
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

        }

        if(Input.GetKeyDown("e") && specials > 0){
            GameObject bomb = (GameObject) Instantiate(SpecialGO);
            bomb.transform.position = specialPosition.transform.position;
            specials--;
            SpecialsUIText.text = "X " + specials.ToString();
        }

        float x = Input.GetAxisRaw("Horizontal");// az ertek -1 (balra nyil), 0 (nincs gomb megnyomva) vagy 1 (jobbra nyil) lesz 
        float y = Input.GetAxisRaw("Vertical"); // az ertek -1 (le nyil), 0 (nincs gomb megnyomva) vagy 1 (fel nyil) lesz

        Vector2 direction = new Vector2 (x,y).normalized; // a bekert adatok szerint kiszamolunk egy egyseg es egy irany vektort

        Move(direction); // ez szamolja ki a karakter mozgasat
    }

    void Move(Vector2 direction){
        
        Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2(0,0));
        Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2(1,1));
        max.x = max.x -0.225f; 
        min.x = min.x +0.225f;

        max.y = max.y - 0.285f;
        min.y = min.y + 0.285f;

        Vector2 pos = transform.position; 

        pos += direction *speed * Time.deltaTime; 
        
        pos.x= Mathf.Clamp (pos.x,min.x,max.x);
        pos.y= Mathf.Clamp (pos.y,min.y,max.y);

        transform.position = pos; 
    }

    void OnTriggerEnter2D(Collider2D col){

        if(((col.tag == "EnemyShipTag") || (col.tag == "EnemyBulletTag")) && !isInvincible){

            PlayerExplosion();
            lives = lives > 0 ? lives - 1 : 0;
            LivesUIText.text = lives.ToString();
            upgradeLevel = 0;
            if(lives ==0){
                GameManagerGO.GetComponent<GameManager>().SetGameManagerState(GameManager.GameManagerState.GameOver);
                gameObject.SetActive(false);
            }else{
                InvincibleModeOn();
                Invoke("InvincibleModeOff", 2f);
            }

            
        }else switch(col.tag){
            case "UpgradePU":
                if(upgradeLevel < 2){
                    upgradeLevel++;
                }else{
                    scoreUITextGO.GetComponent<GameScore>().Score += 500;
                }
                break;
            case "HealPU":
                if(lives < maxLives){
                    lives++;
                    LivesUIText.text = lives.ToString();
                }else{
                    scoreUITextGO.GetComponent<GameScore>().Score += 500;
                }
                break;
            case "SpecialPU":
                if(specials < maxSpecial){
                    specials++;
                    SpecialsUIText.text = "X " + specials.ToString();
                }else{
                    scoreUITextGO.GetComponent<GameScore>().Score += 500;
                }
                break;          
        }
    }

    void PlayerExplosion(){
        GameObject explosion = (GameObject)Instantiate(ExpolsionGO);

        explosion.transform.position = transform.position;
    }

    void InvincibleModeOn(){
        isInvincible = true;
        gameObject.tag = "PlayerUndamagable";
        InvokeRepeating("Flash", 0f, 0.25f);
    }

    void InvincibleModeOff(){
        isInvincible = false;
        CancelInvoke("Flash");
        GetComponent<Renderer>().enabled = true;
        gameObject.tag = "PlayerShipTag";
    }

    void Flash(){
        GetComponent<Renderer>().enabled = !(GetComponent<Renderer>().enabled);
    }
}
