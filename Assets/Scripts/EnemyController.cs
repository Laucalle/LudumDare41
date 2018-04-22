using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public float speed;
	private Rigidbody2D rb2D;
	public GameObject player;

	// Use this for initialization
	void Start () {

		rb2D = GetComponent<Rigidbody2D> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 direction = player.transform.position - transform.position;
		rb2D.velocity = direction.normalized * Time.deltaTime * speed;
		//transform.LookAt (player);
	}

	public void OnTriggerEnter2D(Collider2D collider){

		if(collider.transform.tag == "Bullet"){
			Destroy (collider.transform.gameObject);
			Destroy (transform.gameObject);
		} else if (collider.gameObject.tag == "Tile") {
			TileManager tile_manager = collider.gameObject.GetComponent<TileManager> ();

			if (tile_manager.GetDeadly ()) {
				Destroy (transform.gameObject);
			}
		}
	}

	public void OnTriggerStay2D(Collider2D collider){

		if (collider.gameObject.tag == "Tile") {
			TileManager tile_manager = collider.gameObject.GetComponent<TileManager> ();

			if (tile_manager.GetDeadly ()) {
				Destroy (transform.gameObject);
			}
		}
	}
}
