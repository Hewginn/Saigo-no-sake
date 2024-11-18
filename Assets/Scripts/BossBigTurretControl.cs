using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

//Nagy ágyú
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
        health = 50;
        isDestroyedByPlayer = false;
        InvokeRepeating("FireBomb", 5f, Random.Range(3f, 7f));
        scoreUITextGO = GameObject.FindGameObjectWithTag("ScoreTextTag");
    }


    //Folyamatos körbe-körbe lövés
    private IEnumerator HellFire()
    {
        isHellFireOn = true;

        //Lövések közötti elforgatási szög
        float delta = Mathf.Deg2Rad * 10;

        //Első lövés iránya (egyenesen lefele)
        Vector2 direction = new Vector2(0,-1);

        //Lövés 30x
        for(int i = 0; i<30; i++){

            //Ha az ágyú felrobbant lövés megszakítása
            if(gameObject == null){
                break;
            }

            //Lövedék inicializálása
            GameObject bullet = (GameObject) Instantiate(BulletGO);
            bullet.GetComponent<EnemyBullet>().SetDirection(direction);
            bullet.transform.position = gun.transform.position;

            //Ha az irány már 45 fokkal eltér akkor vissza fele forgatás
            if(Vector2.Angle(direction, new Vector2(0,-1)) > 45){
                delta *= -1;
            }

            //Következő irány beállítása
            direction = new Vector2(
                direction.x * Mathf.Cos(delta) - direction.y * Mathf.Sin(delta),
                direction.x * Mathf.Sin(delta) + direction.y * Mathf.Cos(delta)
                );
            
            //Ágyú elforgatása a lövés irányába
            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction * -1);

            //Várakozás a következő lövéssel
            yield return new WaitForSeconds((float)0.25);
        }
        yield return new WaitForSeconds(3);

        //Következő támadás meghívása
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

        //Ha a HellFire támadás éppen fut és felrobban az ágyú
        // a coroutine befejezése és a következő támadás megkezdése (ha még él a játékos)
        if(isHellFireOn){
            StopCoroutine(HellFire());
            if(GameObject.Find("PlayerGO") != null){
                gameObject.transform.GetComponentInParent<BomberBossControl>().Invoke("actions", 3f);
            }
        }
        

        //Pontszámolás
        if(isDestroyedByPlayer){

            scoreUITextGO.GetComponent<GameScore>().Score += 500;

        }
    }

    //A nagy ágyú saját támadása -> felrobbanó bomba lövése
    private void FireBomb(){
        GameObject playerPlane = GameObject.Find("PlayerGO");
        if(playerPlane != null){

            //Bomba létrehozása
            GameObject bomb = (GameObject)Instantiate(BombGO);
            bomb.GetComponent<BossBomb>().Init(gun.transform.position, playerPlane.transform.position - gun.transform.position, 2f);

            //Ágyú elforgatása az irányba
            transform.rotation = Quaternion.LookRotation(Vector3.forward, (playerPlane.transform.position - gun.transform.position) * -1);
        }
    }
}
