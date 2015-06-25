/*using UnityEngine;
using System.Collections;

public class PlayerSpotlight : MonoBehaviour {

	public bool LightRadiusGrow = false;
	public bool MovingInX = false;
	public bool MovingInY = false;
	public bool playingClimax = false;
	public static bool InSpotlight = false;
	public bool PlayingChase = false;
	public static bool capturedEnemy = false;

	public float FrictionValue = 0.98f;
	public float LightRadius = 20.0f;
	public float XDirection = 0f;
	public float YDirection = 0f;
	public float chaseTime = 0;
	public static float climaxSoundTime = 0;
	public static float enemyClimaxTimer = 0f;

	public AudioClip chaseClip; 
	public AudioClip caughtClip;
	public AudioClip climaxClip;
	public AudioSource chaseAudio;
	public AudioSource caughtAudio;
	public AudioSource climaxAudio;

	void Start(){
		chaseClip = (AudioClip)Resources.Load ("Sounds/EnemyChase");
		caughtClip = (AudioClip)Resources.Load ("Sounds/CellDoorsClose");
		climaxClip = (AudioClip)Resources.Load ("Sounds/Climax");
		AudioSource[] aSources = GetComponents<AudioSource> ();
		chaseAudio = aSources [0];
		caughtAudio = aSources [1];
		climaxAudio = aSources [2];
	}
	void MoveLight ()
	{
		if (Input.GetKey ("left")) {
			XDirection = (XDirection * FrictionValue) - 2f;
			MovingInX = true;
		}

		if (Input.GetKey ("right")) {
			XDirection = (XDirection * FrictionValue) + 2f;
			MovingInX = true;
		}

		if (Input.GetKey ("up")) {
			YDirection = (YDirection * FrictionValue) + 2f;
			MovingInY = true;
		}

		if (Input.GetKey ("down")) {
			YDirection = (YDirection * FrictionValue) - 2f;
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
			XDirection *= .97f;
		if (!MovingInY)
			YDirection *= .97f;
	}

	void GrowSpotlight ()
	{
		CapsuleCollider LightCollider = gameObject.GetComponentInChildren<CapsuleCollider>();

		if (Input.GetKey ("left ctrl")) {
			LightRadiusGrow = true;
			if (LightRadius<50){
				LightRadius *= 1.005f;
				LightCollider.radius *= 1.005f;
			}

		}
		if (Input.GetKeyUp ("left ctrl"))
			LightRadiusGrow = false;
		if (LightRadius>20) {
			if (!LightRadiusGrow){
				LightRadius *= .995f;
				LightCollider.radius *= .995f;
			}
		}
	}

	void ChaseMusic()
	{
		chaseAudio.clip = chaseClip;

		if (InSpotlight) {
			if (chaseTime < 1) {
				chaseTime += Time.deltaTime / 3;
			}

			if (!PlayingChase) {
				chaseAudio.Play ();
				PlayingChase = true;
			}
		}
		if (!InSpotlight) {
			if(chaseTime > 0){
				chaseTime -= Time.deltaTime / 3;
			}
			if(chaseTime <= 0){
				chaseAudio.Stop ();
				PlayingChase = false;
			}
		}
		chaseAudio.volume = chaseTime;
	}

	void ClimaxMusic()
	{
		climaxAudio.clip = climaxClip;

		if (InSpotlight && !playingClimax) {
			if(enemyClimaxTimer <= 1.9f){
				climaxAudio.time = 1.9f - enemyClimaxTimer;
				Debug.Log (enemyClimaxTimer);
				Debug.Log (climaxAudio.time);
				climaxAudio.Play ();
				playingClimax = true;
			}
		}
		if (InSpotlight && playingClimax) {
			if(enemyClimaxTimer > 1.9f){
				climaxAudio.Stop ();
				playingClimax = false;
			}
		}
		if (!InSpotlight && playingClimax) {
			climaxAudio.Stop ();
			playingClimax = false;
		}
	}

	void CaptureMusic()
	{
		if (capturedEnemy) {
			caughtAudio.clip = caughtClip;
			caughtAudio.Play ();
			capturedEnemy = false;
		}
	}
	// Update is called once per frame
	void Update () 
	{
		MoveLight ();
		GrowSpotlight ();
		transform.Translate(XDirection * Time.deltaTime,0,YDirection * Time.deltaTime, Space.World);
		GetComponent<Light>().spotAngle = LightRadius;
		ChaseMusic ();
		CaptureMusic ();
		ClimaxMusic ();
	}
}
*/