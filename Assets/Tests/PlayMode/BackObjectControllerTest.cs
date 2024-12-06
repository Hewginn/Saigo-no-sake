using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TestTools;

public class BackObjectControllerTest
{
    
    private GameObject Controller;
    GameObject mainCamera;

    //Kamera beállítása
    [OneTimeSetUp]
    public void SetupCamera(){

        mainCamera = new GameObject("mainCamera");
        mainCamera.transform.position = new Vector2(0,0);
        mainCamera.AddComponent<Camera>();
        mainCamera.GetComponent<Camera>().orthographic = true;
        mainCamera.GetComponent<Camera>().orthographicSize = 5;
        mainCamera.tag = "MainCamera";

    }

    //Háttér objektum létrehozó beállítása
    [SetUp]
    public void BackObjectControllerTestSetUp()
    {
        Controller = new GameObject("ControllerGO");

        Controller.AddComponent<BackObjectController>();
        Controller.GetComponent<BackObjectController>().backObject = new GameObject("BackObjectPrefab");
        Controller.GetComponent<BackObjectController>().backObject.AddComponent<BackObject>();
        Controller.GetComponent<BackObjectController>().backObject.AddComponent<SpriteRenderer>();
    }

    //Szigetek tesztelése
    [UnityTest]
    public IEnumerator IslandTest()
    {
        //Arrange
        yield return new WaitForFixedUpdate();
        Controller.GetComponent<BackObjectController>().sprites = Resources.LoadAll<Sprite>("ForTests/BackgroundIslands");

        //Act
        yield return new WaitForSeconds(5);
        GameObject BackObject = GameObject.Find("BackObjectPrefab(Clone)");
        bool check = false;
        string spriteName = "BackgroundIslands_";
        for(int i = 0; i < 6; i++){
            if(BackObject.GetComponent<SpriteRenderer>().sprite.name == spriteName + i){
                check = true;
                break;
            }
        }

        //Assert
        Assert.IsTrue(check);
        GameObject.Destroy(BackObject);
    }

    //Csatahajók tesztelése
    [UnityTest]
    public IEnumerator ShipsTest()
    {
        //Arrange
        yield return new WaitForFixedUpdate();
        Controller.GetComponent<BackObjectController>().sprites = Resources.LoadAll<Sprite>("ForTests/BackgroundShips");
        
        //Act
        yield return new WaitForSeconds(5);
        GameObject BackObject = GameObject.Find("BackObjectPrefab(Clone)");
        bool check = false;
        string spriteName = "BackgroundShips_";
        for(int i = 0; i < 6; i++){
            if(BackObject.GetComponent<SpriteRenderer>().sprite.name == spriteName + i){
                check = true;
                break;
            }
        }

        //Assert
        Assert.IsTrue(check);
        GameObject.Destroy(BackObject);
    }

    //Háttérobjektumok minimális távolságának tesztelése
    [UnityTest]
    public IEnumerator DistanceTest(){

        //Arrange
        yield return new WaitForFixedUpdate();
        Controller.GetComponent<BackObjectController>().sprites = Resources.LoadAll<Sprite>("ForTests/BackgroundIslands");

        //Act
        yield return new WaitForSeconds(5);
        GameObject BackObject = GameObject.Find("BackObjectPrefab(Clone)");
        Vector2 firstPosition = BackObject.transform.position;
        GameObject.Destroy(BackObject);
        yield return new WaitForFixedUpdate();
        yield return new WaitUntil(() => GameObject.Find("BackObjectPrefab(Clone)") != null);

        //Assert
        BackObject = GameObject.Find("BackObjectPrefab(Clone)");
        double distance = Math.Abs(firstPosition.x - BackObject.transform.position.x);
        Assert.IsTrue(distance >= 4f);
        GameObject.Destroy(BackObject);
    }

    //Háttérobjektumok a kamera szélein belül vannak
    [UnityTest]
    public IEnumerator InCameraTest(){

        //Arrange
        yield return new WaitForFixedUpdate();
        Controller.GetComponent<BackObjectController>().sprites = Resources.LoadAll<Sprite>("ForTests/BackgroundIslands");

        //Act
        yield return new WaitForSeconds(5);
        
        //Assert
        GameObject BackObject = GameObject.Find("BackObjectPrefab(Clone)");
        Assert.GreaterOrEqual(BackObject.transform.position.x, Camera.main.ViewportToWorldPoint(new Vector2(0,0)).x);
        Assert.GreaterOrEqual(Camera.main.ViewportToWorldPoint(new Vector2(1,1)).x, BackObject.transform.position.x);
        GameObject.Destroy(BackObject);
    }

    //Szög be van állítva
    [UnityTest]
    public IEnumerator AngleIsSetTest(){

        //Arrange
        yield return new WaitForFixedUpdate();
        Controller.GetComponent<BackObjectController>().sprites = Resources.LoadAll<Sprite>("ForTests/BackgroundIslands");

        //Act
        yield return new WaitForSeconds(5);

        //Assert
        GameObject BackObject = GameObject.Find("BackObjectPrefab(Clone)");
        float [] angles = {45, 90, 135, 180, 225, 270};
        bool check = false;
        Debug.Log(BackObject.transform.eulerAngles.z);
        foreach(float angel in angles){
            if(Mathf.Approximately(angel, BackObject.transform.eulerAngles.z)){
                check = true;
                break;
            }
        }
        Assert.IsTrue(check);
        GameObject.Destroy(BackObject);
    }

    //Több objektum létrehozásának tesztelése
    [UnityTest]
    public IEnumerator MultipleSpawnTest(){

        //Arrange
        yield return new WaitForFixedUpdate();
        Controller.GetComponent<BackObjectController>().sprites = Resources.LoadAll<Sprite>("ForTests/BackgroundIslands");

        //Act
        yield return new WaitForSeconds(20);

        //Assert
        GameObject BackObject;
        int numberOfSpawns = 0;
        while(GameObject.Find("BackObjectPrefab(Clone)") != null){
            BackObject = GameObject.Find("BackObjectPrefab(Clone)");
            GameObject.Destroy(BackObject);
            numberOfSpawns++;
            yield return new WaitForFixedUpdate();
        }
        Assert.GreaterOrEqual(numberOfSpawns, 4);
    }

    //Tesztelt objektum törlése
    [TearDown]
    public void BackObjectControllerTestTearDown(){
        GameObject.Destroy(Controller.GetComponent<BackObjectController>().backObject);
        GameObject.Destroy(Controller);
    }

    //Kamera törlése
    [OneTimeTearDown]
    public void TearDownCamera(){
        GameObject.Destroy(mainCamera.gameObject);
    }
}
