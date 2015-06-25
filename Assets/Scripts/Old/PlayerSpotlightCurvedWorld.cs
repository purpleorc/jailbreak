/*using UnityEngine;
using System.Collections;

public class PlayerSpotlightCurvedWorld : MonoBehaviour {

	public bool LightRadiusGrow = false;
	public bool playingClimax = false;
	public static bool InSpotlight = false;
	public bool PlayingChase = false;
	public static bool capturedEnemy = false;
	
	public float LightRadius = 7.0f;
	public float LightRadiusMin = 7.0f;
	public float LightRadiusMax = 20.0f;
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

	void GrowSpotlight ()
	{
		CapsuleCollider LightCollider = gameObject.GetComponentInChildren<CapsuleCollider>();

		if (Input.GetKey ("left ctrl")) {
			LightRadiusGrow = true;
			if (LightRadius<LightRadiusMax){
				LightRadius *= 1.005f;
				LightCollider.radius *= 1.005f;
			}

		}
		if (Input.GetKeyUp ("left ctrl"))
			LightRadiusGrow = false;
		if (LightRadius>LightRadiusMin) {
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
		GrowSpotlight ();
		GetComponent<Light>().spotAngle = LightRadius;
		ChaseMusic ();
		CaptureMusic ();
		ClimaxMusic ();
	}
}*/
