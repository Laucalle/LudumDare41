﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	private List<int> touching_tiles;
	public PuzzleManager puzzle_manager;

    public float speed;
    public float bulletSpeed;
    public float fireRate;
    public GameObject bulletPrefab;
    public GameObject bulletSpawn;
	public GameObject child;
	public GameObject pause;
	public GameObject deathMenu;
    public Text lifeDisplay;
	public int life;
	public AudioManagerController audio_manager;
    public ParticleSystem particleScratch, particleMagic;

	private Animator animator;
	private bool moving;

	private Vector3 prev_position;

    private Rigidbody2D rb2d;
    private float lastShot;

    // Use this for initialization
    void Start()
    {
		animator = GetComponent<Animator> ();
		moving = false;
		touching_tiles = new List<int> ();
        rb2d = GetComponent<Rigidbody2D>();
        float angle = Mathf.Atan2(Vector2.up.y, Vector2.up.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        lastShot = 0.0f;
		Physics2D.IgnoreCollision (GetComponent<BoxCollider2D> (), GetComponentInChildren<BoxCollider2D> ());
		prev_position = transform.position;
        lifeDisplay.text = "Lives: " + life;

    }

    // Update que dispara con lass flechas
    void Update ()
    {
		
        Vector2 moveTarget = new Vector2(0, 0);
        Quaternion targetRotation;

		if (Input.GetKey(KeyCode.W)) moveTarget += new Vector2(0, 1);
		if (Input.GetKey(KeyCode.A)) moveTarget += new Vector2(-1, 0);
		if (Input.GetKey(KeyCode.S)) moveTarget += new Vector2(0, -1);
		if (Input.GetKey(KeyCode.D)) moveTarget += new Vector2(1, 0);

        if (moveTarget != Vector2.zero)
        {
            targetRotation = Quaternion.FromToRotation(Vector2.up, moveTarget);
            if (targetRotation.z == 0) targetRotation.z = targetRotation.x;
            targetRotation.x = 0;
            targetRotation.y = 0;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10);
        }

		if (Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.LeftShift)) {
			SwitchTiles();
		}
    }

    public void FixedUpdate()
    {
		Vector2 movement;
		float moveHorizontal = 0, moveVertical = 0;
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) moveHorizontal = Input.GetAxis("Horizontal");
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) moveVertical = Input.GetAxis("Vertical");

		movement = new Vector2(moveHorizontal, moveVertical);

		rb2d.velocity = movement.normalized * speed;
		child.GetComponent<Rigidbody2D>().velocity = movement.normalized * speed;

		if (movement.magnitude == 0 && moving) {
			moving = false;
			UpdateAnimation ("PlayerIdle");
		} else if (movement.magnitude > 0 && !moving) {
			audio_manager.PlayerSteps();
			moving = true;
			UpdateAnimation ("PlayerMoving");
		}
    }
		
	public void OnTriggerEnter2D(Collider2D collider2D){

		if (collider2D.gameObject.tag == "Tile") {
			TileManager tile_manager = collider2D.gameObject.GetComponent<TileManager> ();
			if (!touching_tiles.Contains (tile_manager.GetId ())) {
				touching_tiles.Add (tile_manager.GetId ());
			}

			if (tile_manager.GetEndTile ()) {
				SceneManager.LoadScene(3);
			} else if (tile_manager.GetDeadly ()) {
				Die ();
			}
		} else if (collider2D.transform.tag == "Enemy") {
			Destroy (collider2D.transform.gameObject);
			life -= 1;
            lifeDisplay.text = "Lives: " + life;
			if (life == 0) {
				Die ();
			}
            ParticleSystem p = Instantiate<ParticleSystem>(particleScratch, (transform.position+collider2D.gameObject.transform.position)/2, Quaternion.identity);
            p.Play();
            StartCoroutine(destroyParticle(p));
        } else if (collider2D.transform.tag == "Wall") {
			Die ();
		}
	}
		
	public void OnTriggerExit2D(Collider2D collider2D){
		if (collider2D.gameObject.tag == "Tile") {
			TileManager tile_manager = collider2D.gameObject.GetComponent<TileManager> ();
			touching_tiles.Remove (tile_manager.GetId ());
		}

	}
		
	private void Die() {
		SceneManager.LoadScene(2);
		Time.timeScale = 1;
	}
		
	private void SwitchTiles() {
		if (touching_tiles.Count == 1) {
			int tile_id = touching_tiles [0];
			int movement = puzzle_manager.TrySwitchTile (tile_id);
			float offset = puzzle_manager.GetOffset();
			Vector3 new_position = transform.position;
			switch(movement) {
			case 1:
				new_position.x -= offset;
				break;
			case 2:
				new_position.y += offset;
				break;
			case 3:
				new_position.x += offset;
				break;
			case 4:
				new_position.y -= offset;
				break;
			}
            if (movement != -1){
                ParticleSystem p = Instantiate<ParticleSystem>(particleMagic, transform.position, Quaternion.identity);
                p.Play();
                StartCoroutine(destroyParticle(p));
            }
            transform.position = new_position;
		}
	}

    IEnumerator destroyParticle(ParticleSystem p)
    {
        yield return new WaitForSeconds(4);
        Destroy(p.gameObject);
    }


	void PrintTouchingTiles() {
		string str = "Lista: ";
		for (int i=0; i<touching_tiles.Count; i++) {
			str += touching_tiles[i] + " ";
		}
		Debug.Log (str);
	}

	public void SpawnPlayer(int blank_position) {
		int pos;
		do {
			pos = Random.Range (0, 9);
		} while (pos == blank_position);

		Vector3 v_pos = new Vector3(0,0,0);
		float offset = puzzle_manager.GetOffset ();
		if (pos % 3 == 0) {
			v_pos.x -= offset;
		} else if (pos % 3 == 2) {
			v_pos.x += offset;
		}
		if (pos < 3) {
			v_pos.y += offset;
		} else if (pos > 5) {
			v_pos.y -= offset;
		}
		transform.position = v_pos;
	}

	public List<int> GetTouchingTiles() {
		return touching_tiles;
	}

	private void UpdateAnimation(string animation_name){
		if (animation_name != null) {
			animator.Play (animation_name);
		}
	}
}
