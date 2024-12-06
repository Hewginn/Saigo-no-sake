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

    [OneTimeSetUp]
    public void SetupCamera(){

        mainCamera = new GameObject("mainCamera");
        mainCamera.transform.position = new Vector2(0,0);
        mainCamera.AddComponent<Camera>();
        mainCamera.GetComponent<Camera>().orthographic = true;
        mainCamera.GetComponent<Camera>().orthographicSize = 5;
        mainCamera.tag = "MainCamera";

    }

    [SetUp]
    public void BackObjectControllerTestSetUp()
    {
        Controller = new GameObject("ControllerGO");

        Controller.AddComponent<BackObjectController>();
        Controller.GetComponent<BackObjectController>().backObject = new GameObject("BackObjectPrefab");
        Controller.GetComponent<BackObjectController>().backObject.AddComponent<BackObject>();
        Controller.GetComponent<BackObjectController>().backObject.AddComponent<SpriteRenderer>();
    }

    [UnityTest]
    public IEnumerator IslandTest()
    {
        yield return new WaitForFixedUpdate();

        Controller.GetComponent<BackObjectController>().sprites = Resources.LoadAll<Sprite>("ForTests/BackgroundIslands");

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

        Assert.IsTrue(check);
        GameObject.Destroy(BackObject);
    }

    [UnityTest]
    public IEnumerator ShipsTest()
    {
        yield return new WaitForFixedUpdate();

        Controller.GetComponent<BackObjectController>().sprites = Resources.LoadAll<Sprite>("ForTests/BackgroundShips");

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

        Assert.IsTrue(check);
        GameObject.Destroy(BackObject);
    }

    [UnityTest]
    public IEnumerator DistanceTest(){

        yield return new WaitForFixedUpdate();

        Controller.GetComponent<BackObjectController>().sprites = Resources.LoadAll<Sprite>("ForTests/BackgroundIslands");

        yield return new WaitForSeconds(5);

        GameObject BackObject = GameObject.Find("BackObjectPrefab(Clone)");
        Vector2 firstPosition = BackObject.transform.position;
        GameObject.Destroy(BackObject);
        yield return new WaitForFixedUpdate();
        yield return new WaitUntil(() => GameObject.Find("BackObjectPrefab(Clone)") != null);
        BackObject = GameObject.Find("BackObjectPrefab(Clone)");
        double distance = Math.Abs(firstPosition.x - BackObject.transform.position.x);
        Assert.IsTrue(distance >= 4f);
        GameObject.Destroy(BackObject);
    }

    [UnityTest]
    public IEnumerator InCameraTest(){
        yield return new WaitForFixedUpdate();

        Controller.GetComponent<BackObjectController>().sprites = Resources.LoadAll<Sprite>("ForTests/BackgroundIslands");

        yield return new WaitForSeconds(5);

        GameObject BackObject = GameObject.Find("BackObjectPrefab(Clone)");

        Assert.GreaterOrEqual(BackObject.transform.position.x, Camera.main.ViewportToWorldPoint(new Vector2(0,0)).x);
        Assert.GreaterOrEqual(Camera.main.ViewportToWorldPoint(new Vector2(1,1)).x, BackObject.transform.position.x);
        GameObject.Destroy(BackObject);
    }

    [UnityTest]
    public IEnumerator AngleIsSetTest(){
        yield return new WaitForFixedUpdate();

        Controller.GetComponent<BackObjectController>().sprites = Resources.LoadAll<Sprite>("ForTests/BackgroundIslands");

        yield return new WaitForSeconds(5);

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

    [UnityTest]
    public IEnumerator MultipleSpawnTest(){

        yield return new WaitForFixedUpdate();

        Controller.GetComponent<BackObjectController>().sprites = Resources.LoadAll<Sprite>("ForTests/BackgroundIslands");

        yield return new WaitForSeconds(20);

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

    [TearDown]
    public void BackObjectControllerTestTearDown(){
        GameObject.Destroy(Controller.GetComponent<BackObjectController>().backObject);
        GameObject.Destroy(Controller);
    }

    [OneTimeTearDown]
    public void TearDownCamera(){
        GameObject.Destroy(mainCamera.gameObject);
    }
}
