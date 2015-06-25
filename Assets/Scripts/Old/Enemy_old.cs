using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
public class Enemy_old : MonoBehaviour {

	public bool CollidingWithSpotlight = false;
	public bool civilianNear = false;
	public bool chasing = false;
	private int levelMax = GameManager.gameSizeMax;
	private int levelMin = GameManager.gameSizeMin;
	public float EnemyLife = 3.0f;
	public float TimerIncrementUp = 0.01f;
	public float TimerIncrementDown = 0.01f;
	public float xDir = Random.Range (-0.4f, 0.4f);
	public float zDir = Random.Range (-0.4f, 0.4f);
	public float turnTime = 5f;
	public float time = 0f;
	public float chaseTime = 5f;
	private float minValue = 1000f;
	public float chaseDistance = 40f;
	public float chaseCooldown = 3f;
	public float normalizedX;
	public float normalizedZ;
	private float[] civilianDistance = new float[GameManager.civilianNum];
	public GameObject civilianTarget;
	public static Vector3 StartVectorEnemy;
	public static List<GameObject> arrayOfEnemies = new List<GameObject>();
	public float ENEMYMAXLIFE = 3.0f;
	
	//Function for spawing enemies.  Takes number of enemies to spawn, and the min and max dimensions of the gameboard for where to spawn them.
	public static void spawnEnemy(int enemyNum, int gameSizeMin, int gameSizeMax)
	{
		for(int i=0; i<enemyNum; i++){
			StartVectorEnemy = new Vector3(Random.Range (gameSizeMin + 1,gameSizeMax - 1), 3, Random.Range (gameSizeMin + 1,gameSizeMax - 1));
			arrayOfEnemies.Add(Instantiate(Resources.Load ("Enemy") as GameObject, StartVectorEnemy, Quaternion.identity) as GameObject);
			arrayOfEnemies[i].name = "Enemy" + i;
			arrayOfEnemies[i].tag = "Enemy";
		}
	}

	//Function for changing the enemy's direction every X seconds.
	void Turn()
	{
		
		if (time < turnTime) {
			time += Time.deltaTime;
		} 
		
		if (time >= turnTime) {
			xDir = Random.Range (-0.4f, 0.4f);
			zDir = Random.Range (-0.4f, 0.4f);
			time = 0;
			turnTime = Random.Range (1f, 5f);
		}
	}

	//Function for changing enemy direction when they hit the bounds of the gameboard.
	void BoundaryCheck()
	{
		if (transform.position.x >= levelMax && xDir > 0) {
			xDir *= -1f;
		}
		
		if (transform.position.x <= levelMin && xDir < 0) {
			xDir *= -1f;
		}
		
		if (transform.position.z >= levelMax && zDir > 0) {
			zDir *= -1f;
		} 
		
		if (transform.position.z <= levelMin  && zDir < 0) {
			zDir *= -1f;
		}
		
	}

	//Functions for determining when an enemy is colliding with the spotlight.
	void OnTriggerEnter(Collider theCollision)
	{
		if (theCollision.gameObject.name == "SpotlightCollider") {
			CollidingWithSpotlight = true;
			PlayerSpotlight.InSpotlight = true;
			//Debug.Log ("Enemy max life is " + ENEMYMAXLIFE);
		}
		if (theCollision.gameObject.tag == "Civilian") {
			Invigorate ();
			chaseCooldown = 3f;
			chasing = false;
		}
	}

	void OnTriggerStay(Collider theCollision)
	{
		if (theCollision.gameObject.name == "SpotlightCollider") {
			PlayerSpotlight.InSpotlight = true;
		}
	}

	void OnTriggerExit(Collider theCollision)
	{
		if (theCollision.gameObject.name == "SpotlightCollider") {
			CollidingWithSpotlight = false;
			PlayerSpotlight.InSpotlight = false;
		}
	}

	//Function that changes the enemy's color when it is inside the spotlight. 
	void ChangeEnemyColor()
	{
		float newColorN = (EnemyLife/ENEMYMAXLIFE) *0.5f;
		Color EnemyCurrentColor = new Vector4(newColorN + 0.5f, 0.5f - newColorN, 0.5f - newColorN, 1f);
		gameObject.GetComponent<Renderer>().material.color = EnemyCurrentColor;
	}

	//Function that determines if enemy life is going up or down.
	void DrainLife()
	{
		if (CollidingWithSpotlight) {
			if (EnemyLife > 0){
				EnemyLife -= TimerIncrementDown;
				ChangeEnemyColor ();
			}
			else{
				KillEnemy ();
			}
		} 
		else {
			if(EnemyLife<ENEMYMAXLIFE){
				EnemyLife += TimerIncrementUp;
				ChangeEnemyColor ();
			}
		}
	}

	//Function that gives enemy full life and higher max health when he kills a civilian.
	void Invigorate(){
		ENEMYMAXLIFE += 1f;
		EnemyLife += ENEMYMAXLIFE / 4;
		if (EnemyLife > ENEMYMAXLIFE)
			EnemyLife = ENEMYMAXLIFE;
		chasing = false;
		time = turnTime;
	}

	//Function for determining if a civilian is close to an enemy.
	void ProximityCheck(){
		minValue = 1000f;
		for (int i = 0; i < Civilian.arrayOfCivilians.Count; i++) {
			civilianDistance[i] = Vector3.Distance(transform.position, Civilian.arrayOfCivilians[i].transform.position);
			if(minValue > civilianDistance[i]){
				minValue = civilianDistance[i];
				civilianTarget = Civilian.arrayOfCivilians[i];
			}
		}
		if (minValue < chaseDistance && chaseCooldown < 0) {
			chasing = true;
			time = 0;
			//Debug.Log (gameObject.name + " is chasing " + civilianTarget.name);
		} 
		else { 
			chasing = false;
		}
	}

	//Function to delay the proximity check so it doesn't run every frame.
	IEnumerator DoProxiCheck(){
		ProximityCheck ();
		yield return new WaitForSeconds (.1f);
	}

	//Function to make enemies chase nearby enemies.
	void Chase()
	{
		if (civilianTarget != null) {

			float distanceX = civilianTarget.transform.position.x - gameObject.transform.position.x;
			float distanceZ = civilianTarget.transform.position.z - gameObject.transform.position.z;
			normalizedX = distanceX / (Mathf.Sqrt(Mathf.Pow(distanceX, 2) + Mathf.Pow (distanceZ, 2)));
			normalizedZ = distanceZ / (Mathf.Sqrt(Mathf.Pow(distanceX, 2) + Mathf.Pow (distanceZ, 2)));
			float chaseSpeed = .35f;
			xDir = chaseSpeed * normalizedX;
			zDir = chaseSpeed * normalizedZ;
			time += Time.deltaTime;

			if (time >= chaseTime) {
				chasing = false;
				chaseCooldown = 3f;
				time = 0;
				//Debug.Log (gameObject.name + " stopped chasing " + civilianTarget.name);
			}
		} else {
			chasing = false;
			chaseCooldown = 3f;
			time = 0;
			Debug.Log ("Target is dead");
		}
	}
	
	//Function that destroys enemy and removes it from the enemy list.
	void KillEnemy()
	{
		if(arrayOfEnemies.Contains (gameObject)){
			arrayOfEnemies.Remove(gameObject);
			PlayerSpotlight.InSpotlight = false;
			Destroy (gameObject);
		}
		else
		Debug.Log ("Error: Object not found in enemy list!");
	}
	
	// Update is called once per frame
	void Update () {
		DrainLife ();
		if (chasing)
			Chase ();
		if (!chasing) {
			Turn ();
			StartCoroutine(DoProxiCheck ());
			if(chaseCooldown > 0)
			chaseCooldown -= Time.deltaTime;
		}
		BoundaryCheck ();
		transform.position += new Vector3 (xDir, 0, zDir);
	}
}
*/
