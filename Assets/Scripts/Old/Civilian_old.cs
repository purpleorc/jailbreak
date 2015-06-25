using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
public class Civilian_old : MonoBehaviour {
	
	public bool enemyCollision = false;
	public bool enemyNear = false;
	public bool warning = false;
	public Color baseColor = Color.blue;
	public Color dangerColor = Color.red;
	public GameObject deadBody;
	public float xDir = Random.Range (-0.4f, 0.4f);
	public float zDir = Random.Range (-0.4f, 0.4f);
	public float turnTime = 5f;
	public float time = 0f;
	public float dangerDistance = 40f;
	public float flashTime = 1f;
	private float minValue = 1000f;
	private float[] enemyDistance = new float[GameManager.enemyNum];
	public static int civilianNum = 10;
	public float moveSpeed = 10;
	public static List<GameObject> arrayOfCivilians = new List<GameObject>();
	public static List<GameObject> enemyTarget = new List<GameObject>();

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

	//Function for changing the enemy's direction every X seconds.
	void Turn()
	{
		if (time < turnTime) {
			time += Time.deltaTime;
		} 
		
		if (time >= turnTime) {
			time = 0;
			turnTime = Random.Range (1f, 5f);
			transform.Rotate (Vector3.up * Random.Range (0,359));
		}
	}

	//Functions for determining if an enemy is close to a civilian.
	void ProximityCheck(){
		minValue = 1000f;
		for (int i = 0; i < Enemy.arrayOfEnemies.Count; i++) {
			enemyDistance[i] = Vector3.Distance(transform.position, Enemy.arrayOfEnemies[i].transform.position);
			if(minValue > enemyDistance[i])
				minValue = enemyDistance[i];
			if (enemyDistance[i] <= dangerDistance && !enemyTarget.Contains (Enemy.arrayOfEnemies[i]))
				enemyTarget.Add(Enemy.arrayOfEnemies[i]);
			if(enemyDistance[i] > dangerDistance && enemyTarget.Contains (Enemy.arrayOfEnemies[i]))
				enemyTarget.Remove (Enemy.arrayOfEnemies[i]);
		}
		if (minValue < dangerDistance) {
			enemyNear = true;
		} 
		else { 
			enemyNear = false;
			warning = false;
		}
		//Debug.Log ("Closest enemy to player is " + minValue + " units away!");
	}

	//Function to delay the proximity check so it doesn't run every frame.
	IEnumerator DoProxiCheck(){
		ProximityCheck ();
		yield return new WaitForSeconds (.1f);
	}

	//Function to make the civilians warn the player that there is an enemy nearby.
	IEnumerator Warn()
	{
		while(enemyNear){
			warning = true;
			baseColor = Color.red;
			gameObject.GetComponent<Renderer>().material.SetColor ("_EmissionColor", baseColor);
			yield return new WaitForSeconds ((minValue / dangerDistance) * .5f);
			
			baseColor = Color.blue;
			gameObject.GetComponent<Renderer>().material.SetColor ("_EmissionColor", baseColor);
			yield return new WaitForSeconds ((minValue / dangerDistance) * .5f);
			
		}
		baseColor = Color.blue;
		warning = false;
		time = turnTime;
	}
/*
	void Flee()
	{
		float normalizedX = 0f;
		float totalNormX = 0;
		float normalizedZ = 0f;
		float totalNormZ = 0;
		if (enemyTarget.Count > 0) {
			foreach (GameObject o in enemyTarget) {
				if(o != null){
					float distanceX = gameObject.transform.position.x - o.transform.position.x;
					float distanceZ = gameObject.transform.position.z - o.transform.position.z;
					normalizedX += distanceX / (Mathf.Sqrt (Mathf.Pow (distanceX, 2) + Mathf.Pow (distanceZ, 2)));
					normalizedZ += distanceZ / (Mathf.Sqrt (Mathf.Pow (distanceX, 2) + Mathf.Pow (distanceZ, 2)));
				}
			}
			totalNormX = normalizedX / (Mathf.Sqrt (Mathf.Pow (normalizedX, 2) + Mathf.Pow (normalizedZ, 2)));
			totalNormZ = normalizedZ / (Mathf.Sqrt (Mathf.Pow (normalizedX, 2) + Mathf.Pow (normalizedZ, 2)));
			float chaseSpeed = .3f;

			if(transform.position.x < levelMax && transform.position.x > levelMin)
				xDir = chaseSpeed * totalNormX;

			if(transform.position.x >= levelMax - 4 || transform.position.x <= levelMin + 4){
				xDir = 0;
				if(zDir > 0)
					zDir = chaseSpeed * 1;
				if(zDir < 0)
					zDir = chaseSpeed * -1;
				//zDir = (chaseSpeed * totalNormX) + (chaseSpeed * totalNormZ);
				//Debug.Log (gameObject.name + " is stopping in X!");
			}
			if(transform.position.z < levelMax && transform.position.z > levelMin)
				zDir = chaseSpeed * totalNormZ;

			if(transform.position.z >= levelMax - 4 || transform.position.z <= levelMin + 4){
				zDir = 0;
				if(xDir > 0)
					xDir = chaseSpeed * 1;
				if(xDir < 0)
					xDir = chaseSpeed * -1;
				//xDir = (chaseSpeed * totalNormX) + (chaseSpeed * totalNormZ);
				//Debug.Log (gameObject.name + " is stopping in Z!");
			}
		}

		else {
			Debug.Log ("No enemies are chasing the target!");
		}
	}

	//Function to check if the civilian has collided with an enemy.
	void OnTriggerEnter(Collider theCollision)
	{
		if (theCollision.gameObject.tag == "Enemy") {
			KillCivilian();
		}
	}

	//Function to destroy the civilian when it hits an enemy.
	void KillCivilian(){
		if(arrayOfCivilians.Contains (gameObject)){
			arrayOfCivilians.Remove(gameObject);
			Destroy (gameObject);
		}
		else
			Debug.Log ("Error: Object not found in enemy list!");

		deadBody = Instantiate(Resources.Load("Body"), gameObject.transform.position, Quaternion.identity) as GameObject;
		Destroy (gameObject);
	}

	// Update is called once per frame
	void Update () {
		StartCoroutine (DoProxiCheck());
		if (enemyNear) {
			//Flee ();
			if(!warning){
				StartCoroutine(Warn ());
			}
		}
		if (!enemyNear) {
			Turn ();
		}
	}

	void FixedUpdate(){
		GetComponent<Rigidbody>().MovePosition (GetComponent<Rigidbody>().position + transform.forward * moveSpeed * Time.deltaTime);
	}
}
*/