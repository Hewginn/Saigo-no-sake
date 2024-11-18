using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Sugát lövés
public class BossBlastControl : MonoBehaviour
{

    //Tag hozzáadása az objektumhoz
    public void blastStart(){
        gameObject.tag = "BossBlastTag";
    }

    //Objektum törlése
    public void destroyBlast(){
        gameObject.tag = "Untagged";
        Destroy(gameObject);
    }
}
