using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSpotlight : MonoBehaviour {

	public bool LightRadiusGrow = false;
	public bool MovingInX = false;
	public bool MovingInY = false;
	public static bool InSpotlight = false;



	public float friction = 0.95f;
	public float moveSpeed = 100;
	public float FrictionValue = 0.98f;
	public float lightRadiusMin = 12.0f;
	public float lightRadiusMax = 40.0f;
	public float LightRadius = 12.0f;
	public float XDirection = 0f;
	public float YDirection = 0f;
	public float rotateSpeed = .1f;

	//private Vector3 moveDir;

	public  List<GameObject> enemiesinlight = new List<GameObject>();
	public  List<GameObject> civiliansinlight = new List<GameObject>();
	public  List<GameObject> bodiesinlight = new List<GameObject>();
	public  List<GameObject> powerupsinlight = new List<GameObject>();


	void MoveLight ()
	{
		if (Input.GetKey ("left") || Input.GetKey ("right")) {
			MovingInX = true;
			if (XDirection < 2 && XDirection > -2) {
				XDirection += Input.GetAxis ("Horizontal") * rotateSpeed * -1;
			}
		}

		if (Input.GetKey ("up") || Input.GetKey ("down")) {
			MovingInY = true;
			if (YDirection < 2 && YDirection > -2) {
				YDirection += Input.GetAxis ("Vertical") * rotateSpeed;
			}
		}

		if (Input.GetKeyUp ("down") || Input.GetKeyUp ("up"))
			MovingInY = false;
		if (Input.GetKeyUp ("left") || Input.GetKeyUp ("right"))
			MovingInX = false;
		/*
		if (MovingInX || MovingInY){
			if (moveSpeed < 5000) {
				moveSpeed *= 1.1f;
			}
		}

*/
		if (!MovingInX){
			XDirection *= .97f;
		}

		if(!MovingInY) {
			YDirection *= .97f;
		}

		transform.Rotate(YDirection, XDirection, 0);
	}


	void OnTriggerEnter(Collider theCollision)
	{
		if (theCollision.gameObject.tag == "Enemy") {
			// Store in enemy list
			enemiesinlight.Add(theCollision.gameObject);
		}
		if (theCollision.gameObject.tag == "Civilian") {
			// Store in enemy list
			civiliansinlight.Add(theCollision.gameObject);
		}
		if (theCollision.gameObject.tag == "Body") {
			// Store in body list
			bodiesinlight.Add(theCollision.gameObject);
		}
		if (theCollision.gameObject.tag == "Powerup") {
			// Store in powerup list
			powerupsinlight.Add(theCollision.gameObject);
		}
	}

	void OnTriggerExit(Collider theCollision)
	{
		if (theCollision.gameObject.tag == "Enemy") {
			// Remove from enemy list
			if(enemiesinlight.Contains (theCollision.gameObject)){
				enemiesinlight.Remove(theCollision.gameObject);
			}
		}
		if (theCollision.gameObject.tag == "Civilian") {
			// Remove from enemy list
			if(civiliansinlight.Contains (theCollision.gameObject)){
				civiliansinlight.Remove(theCollision.gameObject);
			}
		}
		if (theCollision.gameObject.tag == "Body") {
			// Remove from enemy list
			if(bodiesinlight.Contains (theCollision.gameObject)){
				bodiesinlight.Remove(theCollision.gameObject);
			}
		}
		if (theCollision.gameObject.tag == "Powerup") {
			// Remove from enemy list
			if (powerupsinlight.Contains (theCollision.gameObject)){
				powerupsinlight.Remove (theCollision.gameObject);
			}
		}
	}

	void GrowSpotlight ()
	{
		CapsuleCollider LightCollider = gameObject.GetComponentInChildren<CapsuleCollider>();

		if (Input.GetKey ("left ctrl")) {
			LightRadiusGrow = true;
			if (LightRadius<lightRadiusMax){
				LightRadius *= 1.005f;
				LightCollider.radius *= 1.0055f;
			}

		}
		if (LightRadius > lightRadiusMax) {
			LightRadius = lightRadiusMax;
		}
		
		if (Input.GetKeyUp ("left ctrl"))
			LightRadiusGrow = false;
		if (LightRadius>lightRadiusMin) {
			if (!LightRadiusGrow){
				LightRadius *= .995f;
				LightCollider.radius *= .995f;
			}
		}
		if (LightRadius < lightRadiusMin) {
			LightRadius = lightRadiusMin;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		//moveDir += new Vector3 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"), 0).normalized * .05f;
		/*
		if (!MovingInX) {
			if(moveDir.x > 0.01 || moveDir.x < -0.01){
				moveDir.x *= friction;
			}
			
			else{
				moveDir.x = 0;
			}
		}
		if (!MovingInY) {
			if(moveDir.y > 0.01 || moveDir.y < -0.01){
				moveDir.y *= friction;
			}

			else{
				moveDir.y = 0;
			}
		}
		*/
		MoveLight ();
		GrowSpotlight ();
		//transform.Translate(XDirection,YDirection,0, Space.World);
		GetComponentInChildren<Light>().spotAngle = LightRadius;
	}
	/*
	void FixedUpdate(){
		GetComponent<Rigidbody>().MovePosition (GetComponent<Rigidbody>().position + transform.TransformDirection (moveDir) * moveSpeed * Time.deltaTime);
	}
	*/
}
