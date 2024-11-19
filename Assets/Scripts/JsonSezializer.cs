[System.Serializable]
// a json fájlban eltárolt tömb objektumok változói
public class JsonSezializer
{
  public int id;
  public string description;
  public string successed;

}

[System.Serializable]
public class Difficulty
{
  public string type;
  public int maxspawntime;
  public int enemynumber_first_level;
  public int enemynumber_last_level;
  public int min_number_of_enemy;

  public int engine_health;
  public int big_turret_health;
  public int small_turret_health;

}
// a json fájlban eltárolt adatok változói
[System.Serializable]
public class Missions
{
  public string title;
  public JsonSezializer[] missions;

  public Difficulty[] difficulty;
  public int score;

  public int[] highscores;

  public string choosed_difficulty;

}

