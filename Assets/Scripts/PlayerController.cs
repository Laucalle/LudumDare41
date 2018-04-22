using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	// AÑADIDO POR JOSE
	List<int> touching_tiles;
	public PuzzleManager puzzle_manager;

    public float speed;
    public float bulletSpeed;
    public float fireRate;
    public GameObject bulletPrefab;
    public GameObject bulletSpawn;
	public GameObject child;

    private Rigidbody2D rb2d;
    private float lastShot;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        float angle = Mathf.Atan2(Vector2.up.y, Vector2.up.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        lastShot = 0.0f;
    }

    
    void Update2 () {

        Vector2 moveTarget = new Vector2(0, 0);
        Quaternion targetRotation;

        Vector3 shootDirection;
        Vector3 mousePosition;
        mousePosition = Input.mousePosition;
        mousePosition.z = 0.0f;
        shootDirection = Camera.main.ScreenToWorldPoint(mousePosition);
        shootDirection = shootDirection - transform.position;

        transform.rotation = Quaternion.LookRotation(mousePosition);

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
			//child.GetComponent<GunController>().CompensateRotation (targetRotation);
        }

		// AÑADIDO POR JOSE
		if (Input.GetKeyDown(KeyCode.L)) {
			SwitchTiles();
		}
    }

    public void FixedUpdate()
    {
        float moveHorizontal = 0, moveVertical = 0;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) moveHorizontal = Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        rb2d.velocity = movement.normalized * speed;
		child.GetComponent<Rigidbody2D>().velocity = movement.normalized * speed;  
    }

	// AÑADIDO POR JOSE
	public void OnTriggerEnter2D(Collider2D collider2D){
		if (collider2D.gameObject.tag == "Tile") {
			TileManager tile_manager = collider2D.gameObject.GetComponent<TileManager> ();
			if (!touching_tiles.Contains (tile_manager.GetId ())) {
				touching_tiles.Add (tile_manager.GetId ());
			}

			if (tile_manager.GetEndTile ()) {
				Debug.Log ("He Ganado!");
			} else if (tile_manager.GetDeadly ()) {
				Die ();
			}
		} else if (collider2D.transform.tag == "Enemy") {
			Debug.Log ("Te han pillado");
			Destroy (collider2D.transform.gameObject);
		}
	}

	// AÑADIDO POR JOSE
	public void OnTriggerExit2D(Collider2D collider2D){
		if (collider2D.gameObject.tag == "Tile") {
			TileManager tile_manager = collider2D.gameObject.GetComponent<TileManager> ();
			touching_tiles.Remove (tile_manager.GetId ());
		}

	}

	// AÑADIDO POR JOSE
	private void Die() {
		Debug.Log ("He muerto");
	}

	// AÑADIDO POR JOSE
	private void SwitchTiles() {
		if (touching_tiles.Count == 1) {
			int tile_id = touching_tiles [0];
			int movement = puzzle_manager.TrySwitchTile (tile_id);
			float offset = 2.57f;
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
			transform.position = new_position;
		}
	}

	void PrintTouchingTiles() {
		string str = "Lista: ";
		for (int i=0; i<touching_tiles.Count; i++) {
			str += touching_tiles[i] + " ";
		}
		Debug.Log (str);
	}
}
