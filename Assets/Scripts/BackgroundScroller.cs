using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroller : MonoBehaviour
{
    /*
    [SerializeField] private RawImage _img; // görgetendő kép
    [SerializeField] private float _x, _y; // a görgetés iránya
    // Update is called once per frame
    void Update()
    {
        // a háttér görgetése
        _img.uvRect = new Rect(_img.uvRect.position + new Vector2(_x,_y)* Time.deltaTime,_img.uvRect.size);
    }*/
    public float speed; // a sebbeség
    [SerializeField] 
    private Renderer bgRenderer;// a kép amit görgetünk

    void Update(){
        //a háttér görgetésének iránya, sebesség szerint
        bgRenderer.material.mainTextureOffset+= new Vector2(0,speed*Time.deltaTime);
    }
}
