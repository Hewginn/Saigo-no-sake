using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class BossBigTurretControl : MonoBehaviour
{
//Pontokat számláló UI szöveg
    GameObject scoreUITextGO;
    
    //Fegyver által ki lőtt lövedék
    public GameObject BulletGO;

    //Fegyver által ki lőtt bomba
    public GameObject BombGO;

    //A torony fegyvere
    public GameObject gun;

    //A torony életereje
    int health;

    //A játékos győzte le
    bool isDestroyedByPlayer;

    //Robbanás
    public GameObject ExplosionGO;

    //HellFire akció fut-e
    bool isHellFireOn;

    //Éledéskor inicializálás beállítása
    private void Start() {
        isHellFireOn = false;
        health = 1;
        isDestroyedByPlayer = false;
        InvokeRepeating("FireBomb", 5f, Random.Range(3f, 7f));
        scoreUITextGO = GameObject.FindGameObjectWithTag("ScoreTextTag");
    }


    //Folyamatos körbe-körbe lövés
    private IEnumerator HellFire()
    {
        isHellFireOn = true;
        float delta = Mathf.Deg2Rad * 10;
        Vector2 direction = new Vector2(0,-1);
        for(int i = 0; i<30; i++){
            if(gameObject == null){
                break;
            }
            GameObject bullet = (GameObject) Instantiate(BulletGO);
            bullet.GetComponent<EnemyBullet>().SetDirection(direction);
            bullet.transform.position = gun.transform.position;
            if(Vector2.Angle(direction, new Vector2(0,-1)) > 45){
                delta *= -1;
            }
            direction = new Vector2
                (direction.x * Mathf.Cos(delta) - direction.y * Mathf.Sin(delta),
                direction.x * Mathf.Sin(delta) + direction.y * Mathf.Cos(delta));
            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction * -1);
            yield return new WaitForSeconds((float)0.25);
        }
        yield return new WaitForSeconds(3);
        gameObject.transform.GetComponentInParent<BomberBossControl>().Invoke("actions", 3f);
        isHellFireOn = false;
    }

    public void HellFireTrigger(){
        StartCoroutine(HellFire());
    }

    //Találatok ellenőrzése
    private void OnTriggerEnter2D(Collider2D col) {
        switch(col.tag){

            //Sima lövés ellenőrzése
            case "PlayerBulletTag":
                health--;
                break;

            //Különleges lövés ellenőrzése
            case "PlayerSpecialTag":
                health-=5;
                break;
        }
        
        if(health <= 0){

            //Robbanás lejátszása
            PlayerExplosion();

            //Játékos robbantotta fel
            isDestroyedByPlayer = true;

            //Törlés
            Destroy(gameObject);
        }
    }

    //Robbanást inicializáló kódrész
    void PlayerExplosion()
    {
        //Példányosítás
        GameObject explosion = (GameObject)Instantiate(ExplosionGO);

        //Robbanás helyének meghatározása (objektum helye)
        explosion.transform.position = transform.position;
    }

    private void OnDestroy() {

        if(isHellFireOn){
            StopCoroutine(HellFire());
            if(GameObject.Find("PlayerGO") != null){
                gameObject.transform.GetComponentInParent<BomberBossControl>().Invoke("actions", 3f);
            }
        }
        

        //Elpusztított repülők számolása
        if(isDestroyedByPlayer){

            scoreUITextGO.GetComponent<GameScore>().Score += 500;

        }
    }

    private void FireBomb(){
        GameObject playerPlane = GameObject.Find("PlayerGO");
        if(playerPlane != null){
            GameObject bomb = (GameObject)Instantiate(BombGO);
            bomb.GetComponent<BossBomb>().Init(gun.transform.position, playerPlane.transform.position - gun.transform.position, 2f);
        }
    }
}
