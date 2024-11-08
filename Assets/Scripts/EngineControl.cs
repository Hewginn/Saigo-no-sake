using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EngineControl : MonoBehaviour
{
    //Aktuális élet
    int health;

    //Max élet
    int maxHealth = 1;

    //Legutóbb kapott sebzés
    float lastDamaged;

    //Legutóbb kapott gyógyítás
    float lastHeal;

    //Sugár lövés
    public GameObject Blast;

    //Gyógyulás effekt
    public GameObject Heal;
    
    //Inicializálás
    public void Init(){
        health = maxHealth;

        lastDamaged = Time.time;

        lastHeal = Time.time;

        Heal.SetActive(false);
    }
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    //Minden képkockánál meghívásra kerül
    void Update(){
        Regeneration();
    }

    //Sugárlövés
    public void engineBlast(){

        //Sugár létrehozása
        GameObject newBlast = (GameObject)Instantiate(Blast);

        //Sugár pozíciójának létrehozása
        newBlast.transform.position =
        new Vector2(
            transform.position.x,
            transform.position.y - newBlast.GetComponent<SpriteRenderer>().bounds.size.y / 2 - 0.064f
        );
        newBlast.transform.SetParent(gameObject.transform, true);
    }

    //Ütközések kezelése
    private void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "PlayerBulletTag"){
            health--;
        }else if(col.tag == "PlayerSpecialTag"){
            health -= 5;
        }
        if(health <= 0){
                Destroy(gameObject);
        }
        lastDamaged = Time.time;
    }

    //Élettöltés
    private void Regeneration(){
        if((Time.time - lastDamaged >= 5f) && (health < maxHealth) && (Time.time - lastHeal > 1f)){
            health = health + 1;
            Heal.SetActive(true);
            lastHeal = Time.time;
        }else if(lastHeal < lastDamaged || health == maxHealth){
            Heal.SetActive(false);
        }
    }

    private void OnDestroy() {
        transform.parent.gameObject.GetComponent<BomberBossControl>().NumberOfEngines -= 1;
    }
    
}
