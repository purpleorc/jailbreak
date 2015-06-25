using UnityEngine;
using System.Collections;


public class Enemy : MonoBehaviour {

	public bool CollidingWithSpotlight = false;
	public bool civilianNear = false;
	public bool chasing = false;
	public bool grounded;
	public float EnemyLife = 3.0f;
	public float TimerIncrementUp = 0.01f;
	public float TimerIncrementDown = 0.01f;
	public float xDir = Random.Range (-0.4f, 0.4f);
	public float zDir = Random.Range (-0.4f, 0.4f);
	public float turnTime = 5f;
	public float turnCount = 0f;
	public float chaseCount = 0f;
	public float chaseTime = 5f;
	private float minValue = 1000f;
	public float chaseDistance = 500f;
	public float chaseCooldown = 3f;
	public float normalizedX;
	public float normalizedZ;
	private float[] civilianDistance = new float[GameManager.civilianNum];
	public GameObject civilianTarget;
	public static Vector3 StartVectorEnemy;

	public float ENEMYMAXLIFE = 3.0f;
	public float moveSpeed = 100;
	public LayerMask groundedMask;



	//Function for changing the enemy's direction every X seconds.
	void Turn()
	{
		if (turnCount < turnTime) {
			turnCount += Time.deltaTime;
		} 
		
		if (turnCount >= turnTime) {
			turnCount = 0;
			turnTime = Random.Range (1f, 5f);
			transform.Rotate (Vector3.up * Random.Range (0,359));
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
			Turn ();
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
		ENEMYMAXLIFE += 0.5f;
		EnemyLife += ENEMYMAXLIFE / 4;
		if (EnemyLife > ENEMYMAXLIFE)
			EnemyLife = ENEMYMAXLIFE;
		chasing = false;
		turnCount = turnTime;
	}

	//Function for determining if a civilian is close to an enemy.
	void ProximityCheck(){
		minValue = chaseDistance + 1;
		for (int i = 0; i < GameManager.arrayOfCivilians.Count; i++) {
			civilianDistance[i] = Vector3.Distance(transform.position, GameManager.arrayOfCivilians[i].transform.position);
			//Debug.Log (civilianDistance[i]);
			if(minValue > civilianDistance[i]){
				minValue = civilianDistance[i];
				civilianTarget = GameManager.arrayOfCivilians[i];
			}
		}
		if (minValue < chaseDistance && chaseCooldown <= 0) {
			chasing = true;
			chaseCount = 0;
			Debug.Log (gameObject.name + " is chasing " + civilianTarget.name);
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
	void Chase(GameObject target)
	{
		if (target != null) {

			//float distanceX = target.transform.position.x - gameObject.transform.position.x;
			//float distanceZ = target.transform.position.z - gameObject.transform.position.z;
			//normalizedX = distanceX / (Mathf.Sqrt(Mathf.Pow(distanceX, 2) + Mathf.Pow (distanceZ, 2)));
			//normalizedZ = distanceZ / (Mathf.Sqrt(Mathf.Pow(distanceX, 2) + Mathf.Pow (distanceZ, 2)));
			//float chaseSpeed = .35f;
			//xDir = chaseSpeed * normalizedX;
			//zDir = chaseSpeed * normalizedZ;
			Vector3 targetVector = (target.transform.position - transform.position).normalized;
			//Quaternion chaseRotation = Quaternion.LookRotation (target.transform.position - transform.position, Vector3.up);
			transform.rotation = Quaternion.FromToRotation(transform.forward, targetVector) * transform.rotation;
			chaseCount += Time.deltaTime;

			if (chaseCount >= chaseTime) {
				chasing = false;
				chaseCooldown = 3f;
				chaseCount = 0;
				turnCount = turnTime;
				//Debug.Log (gameObject.name + " stopped chasing " + civilianTarget.name);
			}
		} else {
			chasing = false;
			chaseCooldown = 3f;
			chaseCount = 0;
			Debug.Log ("Target is dead");
		}
	}
	
	//Function that destroys enemy and removes it from the enemy list.
	void KillEnemy()
	{
		if(GameManager.arrayOfEnemies.Contains (gameObject)){
			GameManager.arrayOfEnemies.Remove(gameObject);
			PlayerSpotlight.InSpotlight = false;
			GameManager.livingEnemies -= 1;
			Destroy (gameObject);
		}
		else
		Debug.Log ("Error: Object not found in enemy list!");
	}
	
	// Update is called once per frame
	void Update () {
		DrainLife ();
		if (!chasing) {
			Turn ();
			StartCoroutine(DoProxiCheck ());
			if(chaseCooldown > 0){
				chaseCooldown -= Time.deltaTime;
			}
			if(chaseCooldown < 0){
				chaseCooldown = 0;
			}
		}

		grounded = false;
		Ray ray = new Ray (transform.position, -transform.up);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 1 + .1f, groundedMask)) {
			grounded = true;
		}

		if (grounded) {

		}
	}

	void FixedUpdate(){
		if (chasing)
			Chase (civilianTarget);
		GetComponent<Rigidbody>().MovePosition (GetComponent<Rigidbody>().position + transform.forward * moveSpeed * Time.deltaTime);
	}
}
