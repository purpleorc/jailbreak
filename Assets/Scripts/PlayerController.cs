using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float moveSpeed = 100;
	private Vector3 moveDir;

	void Start(){
		InvokeRepeating ("MoveEnemy",0f,3f);
	}
	void MoveEnemy(){
		transform.Rotate (Vector3.up * Random.Range (0,359));
		//moveDir = new Vector3(Random.Range(-1f,1f), 0 , Random.Range (-1f,1f)).normalized;
		//Debug.Log (moveDir);
	}

	void Update(){
		moveDir = transform.forward;
		//moveDir = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical")).normalized;

	}

	void FixedUpdate(){
		GetComponent<Rigidbody>().MovePosition (GetComponent<Rigidbody>().position + transform.TransformDirection (moveDir) * moveSpeed * Time.deltaTime);
	}
}
