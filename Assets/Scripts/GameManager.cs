using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager: MonoBehaviour {
	
	public static int enemyNum = 10;
	public static int livingEnemies;
	public static int civilianNum = 10;
	public static int livingCivilians;
	public static int enemyIndex = 0;

	public static List<GameObject> arrayOfEnemies = new List<GameObject>();
	public static List<GameObject> arrayOfCivilians = new List<GameObject>();


	//Function for spawing enemies.  Takes number of enemies to spawn, and the min and max dimensions of the gameboard for where to spawn them.
	public static void spawnEnemy(int enemyNum)
	{
		for(int i=0; i<enemyNum; i++){
			arrayOfEnemies.Add(Instantiate(Resources.Load ("Enemy") as GameObject,(Random.onUnitSphere *620), Quaternion.identity) as GameObject);
			arrayOfEnemies[i].name = "Enemy" + i;
			arrayOfEnemies[i].tag = "Enemy";
		}
	}

	//Function for spawing civilians.  Takes number of civilians to spawn, and the min and max dimensions of the gameboard for where to spawn them.
	public static void spawnCivilians(int civilianNum)
	{
		for (int i=0; i<civilianNum; i++) {
			arrayOfCivilians.Add(Instantiate(Resources.Load ("Civilian") as GameObject, Vector3.zero, Quaternion.identity) as GameObject);
			arrayOfCivilians[i].transform.position = Random.onUnitSphere * 620;
			arrayOfCivilians[i].name = "Civilian" + i;
			arrayOfCivilians[i].tag = "Civilian";
		}
	}

	// Use this for initialization
	void Start () {
		spawnEnemy (enemyNum);
		spawnCivilians (civilianNum);
		livingEnemies = enemyNum;
		livingCivilians = civilianNum;
	}

	void OnGUI(){
		GUILayout.Label ("Enemies left: " + livingEnemies);
		GUILayout.Label ("Civilians left: " + livingCivilians);
	}
}