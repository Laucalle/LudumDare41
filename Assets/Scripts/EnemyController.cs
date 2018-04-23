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
	public AudioManagerController audio_manager;
    public ParticleSystem particleSystem;

	// Use this for initialization
	void Start () {

		animator = GetComponent<Animator> ();
		rb2D = GetComponent<Rigidbody2D> ();
		dead = false;
		time_to_death = 0.5f;
        float angle = Mathf.Atan2(Vector2.up.y, Vector2.up.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

	// Update is called once per frame
	void Update () {
		if (dead) {
			if (actual_time_to_death <= 0) {
				Die();
			}
			actual_time_to_death -= Time.deltaTime;
		} else {
			audio_manager.DollSteps ();
			Vector2 direction = player.transform.position - transform.position;
			rb2D.velocity = direction.normalized * Time.deltaTime * speed;

            Vector2 moveTarget = new Vector2(0, 0);
            Quaternion targetRotation;

            if (direction.y > -0.5) moveTarget += new Vector2(0, 1);
            if (direction.x < 0.5) moveTarget += new Vector2(-1, 0);
            if (direction.y < -0.5) moveTarget += new Vector2(0, -1);
            if (direction.x > 0.5) moveTarget += new Vector2(1, 0);

            if (moveTarget != Vector2.zero)
            {
                targetRotation = Quaternion.FromToRotation(Vector2.up, moveTarget);
                if (targetRotation.z == 0) targetRotation.z = targetRotation.x;
                targetRotation.x = 0;
                targetRotation.y = 0;
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 50);
            }
		}
	}

	public void OnTriggerEnter2D(Collider2D collider){
		if (collider.transform.tag == "Bullet") {
			Destroy (collider.transform.gameObject);
			DieByWeapon ();
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

    private void DieByWeapon() {
        spawner.GetComponent<SpawnerController>().ChildDied();
        dead = true;
        actual_time_to_death = time_to_death * 3.5f;
        rb2D.velocity = Vector3.zero;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        ParticleSystem p = Instantiate<ParticleSystem>(particleSystem, transform.position, Quaternion.identity);
        p.Play();
        StartCoroutine(destroyParticle(p));
        GetComponent<SpriteRenderer>().enabled = false;
    }


    private void Die() {
		Destroy (transform.gameObject);
	}

	private void UpdateAnimation(string animation_name){
		if (animation_name != null) {
			animator.Play (animation_name);
		}
	}

    IEnumerator destroyParticle(ParticleSystem p)
    {
        yield return new WaitForSeconds(4);
        Destroy(p.gameObject);
    }
}
