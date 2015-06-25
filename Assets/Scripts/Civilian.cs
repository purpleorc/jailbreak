using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Civilian : MonoBehaviour {
	
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
	public float dangerDistance = 100f;
	public float flashTime = 1f;
	private float minValue = 1000f;
	private float[] enemyDistance = new float[GameManager.enemyNum];
	private int livingCivilians = GameManager.livingCivilians;
	public float moveSpeed = 80;
	public static List<GameObject> enemyTarget = new List<GameObject>();



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
		for (int i = 0; i < GameManager.arrayOfEnemies.Count; i++) {
			enemyDistance[i] = Vector3.Distance(transform.position, GameManager.arrayOfEnemies[i].transform.position);
			if(minValue > enemyDistance[i])
				minValue = enemyDistance[i];
			if (enemyDistance[i] <= dangerDistance && !enemyTarget.Contains (GameManager.arrayOfEnemies[i]))
				enemyTarget.Add(GameManager.arrayOfEnemies[i]);
			if(enemyDistance[i] > dangerDistance && enemyTarget.Contains (GameManager.arrayOfEnemies[i]))
				enemyTarget.Remove (GameManager.arrayOfEnemies[i]);
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

	void Flee()
	{
		if (enemyTarget.Count > 0) {
			Vector3 totalTargetVectors = Vector3.zero;
			Vector3 averageTargetVectors = Vector3.zero;
			foreach (GameObject o in enemyTarget) {
				if(o != null){
					Debug.Log (gameObject + " is fleeing from " + o);
					totalTargetVectors += transform.position - o.transform.position;
				}
			}
			averageTargetVectors = (totalTargetVectors / enemyTarget.Count).normalized;
			transform.rotation = Quaternion.FromToRotation(transform.forward, averageTargetVectors) * transform.rotation;
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
		if(GameManager.arrayOfCivilians.Contains (gameObject)){
			GameManager.arrayOfCivilians.Remove(gameObject);
			Destroy (gameObject);
		}
		else
			Debug.Log ("Error: Object not found in enemy list!");

		deadBody = Instantiate(Resources.Load("Body"), gameObject.transform.position, Quaternion.identity) as GameObject;
		GameManager.livingCivilians -= 1;
		Destroy (gameObject);
	}

	// Update is called once per frame
	void Update () {
		StartCoroutine (DoProxiCheck());
		if (enemyNear) {
			Flee ();
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
