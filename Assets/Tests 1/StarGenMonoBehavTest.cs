using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;


public abstract class StarGenMonoBehavTest{
    protected GameObject subject;

    [SetUp]
    public virtual void SetUp(){
        subject = new GameObject("Subject");
    }

    [TearDown]
    public void TearDown()
    {
        Object.FindObjectsOfType<GameObject>().ToList().ForEach(go=>Object.DestroyImmediate(go));
    }

}
