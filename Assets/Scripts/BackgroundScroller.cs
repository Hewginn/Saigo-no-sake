using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Háttér görgető
public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private RawImage img;// a kép amit görgetünk

    [SerializeField] private float x, y; //A görgetés sebessége x és y irányban

    void Update(){
        img.uvRect = new Rect(img.uvRect.position + new Vector2(x, y) * Time.deltaTime, img.uvRect.size); //Görgetés
    }
}
