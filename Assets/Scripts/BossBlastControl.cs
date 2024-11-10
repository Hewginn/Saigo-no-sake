using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBlastControl : MonoBehaviour
{
    public void blastStart(){
        gameObject.tag = "BossBlastTag";
    }

    public void destroyBlast(){
        gameObject.tag = "Untagged";
        Destroy(gameObject);
    }
}
