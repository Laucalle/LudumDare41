using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

	public float bulletSpeed;
	public float fireRate;
	public GameObject bulletPrefab;
	public GameObject bulletSpawn;

	private AudioManagerController audio_manager;
    public ParticleSystem particleShot;
	private float lastShot;
	private Animator animator;
	private float initial_position;

	// Use this for initialization
	void Start () {
		audio_manager = GetComponentInParent<PlayerController> ().audio_manager;
		animator = GetComponent < Animator> ();
		lastShot = 0.0f;
		initial_position = (transform.position - GetComponentInParent<PlayerController> ().transform.position).magnitude;
	}

	public void CompensateRotation(Quaternion rot){

		Quaternion targetRotation;
		targetRotation = new Quaternion (-rot.x, -rot.y, 0, rot.w);
		if (targetRotation.z == 0) targetRotation.z = targetRotation.x;
		targetRotation.x = 0;
		targetRotation.y = 0;
		transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10);
		
	}
	
	// Update is called once per frame
	void Update ()
	{

		Vector2 moveTarget = new Vector2(0, 0);
		Quaternion targetRotation;

		if (Input.GetKey("up")) moveTarget += new Vector2(0, 1);
		if (Input.GetKey("left")) moveTarget += new Vector2(-1, 0);
		if (Input.GetKey("down")) moveTarget += new Vector2(0, -1);
		if (Input.GetKey("right")) moveTarget += new Vector2(1, 0);

		if (moveTarget != Vector2.zero) {
			float angle = Mathf.Atan2 (moveTarget.y, moveTarget.x) * Mathf.Rad2Deg;
			targetRotation = Quaternion.FromToRotation (Vector2.up, moveTarget);
			if (targetRotation.z == 0)
				targetRotation.z = targetRotation.x;
			targetRotation.x = 0;
			targetRotation.y = 0;
			transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, Time.deltaTime * 15);
		}
	}

	private void FixedUpdate() {
		if (Input.GetKey(KeyCode.Space) && Time.time - lastShot > fireRate)
		{
			GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, transform.rotation);
			bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * bulletSpeed;
			lastShot = Time.time;
			UpdateAnimation ("GunnerShot");
            audio_manager.Shot ();
		} 
	}

	private void UpdateAnimation(string animation_name){
		if (animation_name != null) {
			animator.Play (animation_name);
		}
	}

    public void Recolocate() {
		Vector3 pos = GetComponentInParent<Rigidbody2D> ().velocity.normalized * initial_position;
		if (pos != Vector3.zero)
			transform.position = GetComponentInParent<PlayerController> ().transform.position + pos;
	}
}
