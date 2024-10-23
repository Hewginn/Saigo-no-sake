using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    //Egyszeri animációk pl: robbanás törlése
    void DestroyGameObject(){
        Destroy(gameObject);
    }
}
