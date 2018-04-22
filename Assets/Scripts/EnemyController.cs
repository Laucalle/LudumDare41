using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	private Animator animator;
	public float speed;
	private Rigidbody2D rb2D;
	public GameObject player;
	public GameObject spawner;
	private bool dead;
	private float time_to_death;
	private float actual_time_to_death;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		rb2D = GetComponent<Rigidbody2D> ();
		dead = false;
		time_to_death = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
		if (dead) {
			if (actual_time_to_death <= 0) {
				Die();
			}
			actual_time_to_death -= Time.deltaTime;
		} else {
			Vector2 direction = player.transform.position - transform.position;
			rb2D.velocity = direction.normalized * Time.deltaTime * speed;
			//transform.LookAt (player.transform.position);
			Quaternion lookAt = Quaternion.FromToRotation(Vector2.up,player.transform.position);
			if (lookAt.z == 0) lookAt.z = lookAt.x;
			lookAt.x = 0;
			lookAt.y = 0;
			transform.rotation = lookAt;
		}
	}

	public void OnTriggerEnter2D(Collider2D collider){
		if (collider.transform.tag == "Bullet") {
			Destroy (collider.transform.gameObject);
			Die ();
		} else if (collider.gameObject.tag == "Tile") {
			TileManager tile_manager = collider.gameObject.GetComponent<TileManager> ();

			if (tile_manager.GetDeadly ()) {
				DieByFalling ();
			}
		}
	}
		
	public void OnTriggerStay2D(Collider2D collider){
		if (!dead) {
			if (collider.gameObject.tag == "Tile") {
				TileManager tile_manager = collider.gameObject.GetComponent<TileManager> ();

				if (tile_manager.GetDeadly ()) {
					DieByFalling ();
				}
			}
		}
	}

	private void DieByFalling() {
		spawner.GetComponent<SpawnerController> ().ChildDied ();
		dead = true;
		UpdateAnimation ("EnemyFalling");
		actual_time_to_death = time_to_death;
		rb2D.velocity = Vector3.zero;
		GetComponent<BoxCollider2D> ().enabled = false;
		GetComponent<CircleCollider2D> ().enabled = false;
	}

	private void Die() {
		Destroy (transform.gameObject);
	}

	private void UpdateAnimation(string animation_name){
		if (animation_name != null) {
			animator.Play (animation_name);
		}
	}
}
