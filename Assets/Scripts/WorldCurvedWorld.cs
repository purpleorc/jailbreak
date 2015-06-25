using UnityEngine;
using System.Collections;

public class WorldCurvedWorld: MonoBehaviour {
	
	public bool MovingInX = false;
	public bool MovingInY = false;

	public float FrictionValue = 0.98f;
	public float XRotation = 0f;
	public float YRotation = 0f;

	void RotateWorld ()
	{
		if (Input.GetKey ("left")) {
			YRotation = (YRotation * FrictionValue) - 0.5f;
			MovingInX = true;
		}

		if (Input.GetKey ("right")) {
			YRotation = (YRotation * FrictionValue) + 0.5f;
			MovingInX = true;
		}

		if (Input.GetKey ("up")) {
			XRotation = (XRotation * FrictionValue) - 0.5f;
			MovingInY = true;
		}

		if (Input.GetKey ("down")) {
			XRotation = (XRotation * FrictionValue) + 0.5f;
			MovingInY = true;
		}

		if (Input.GetKeyUp ("down"))
			MovingInY = false;
		if (Input.GetKeyUp ("up"))
			MovingInY = false;
		if (Input.GetKeyUp ("left"))
			MovingInX = false;
		if (Input.GetKeyUp ("right"))
			MovingInX = false;

		if (!MovingInX)
			XRotation *= .97f;
		if (!MovingInY)
			YRotation *= .97f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		RotateWorld ();
		transform.Rotate(XRotation * Time.deltaTime,YRotation * Time.deltaTime,0, Space.World);
	}
}
