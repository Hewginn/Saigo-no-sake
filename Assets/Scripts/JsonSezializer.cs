[System.Serializable]
// a json fájlban eltárolt tömb objektumok változói
public class JsonSezializer
{
 public int id;
 public string description;
 public string successed;
  
}
// a json fájlban eltárolt adatok változói
[System.Serializable]
public class Missions{
  public string title;
  public JsonSezializer[] missions;
}
