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
		rb2D.velocity = direction * speed;

		
	}
}
