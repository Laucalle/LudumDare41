using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour {

	public float spawnFreq;
	public int maxEnemies;
	public GameObject enemyPrefab;
	public GameObject player;
	public PuzzleManager puzzle_manager;
	public AudioManagerController audio_manager;

	private float prevSpawn;
	private int enemiesOnScene;

	// Use this for initialization
	void Start () {
		enemiesOnScene = 0;
		prevSpawn = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

		int pos = puzzle_manager.SpawnEnemyPosition ();
		Vector3 v_pos = transform.position;
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


		if(Time.time - prevSpawn > spawnFreq && enemiesOnScene < maxEnemies){
			GameObject enemy = Instantiate(enemyPrefab, v_pos, transform.rotation);
			enemy.GetComponent<EnemyController> ().player = player;
			enemy.GetComponent<EnemyController> ().spawner = transform.gameObject;
			enemy.GetComponent<EnemyController> ().audio_manager = audio_manager;
			prevSpawn = Time.time;
			enemiesOnScene += 1;

			audio_manager.Voices ();
		}
		
	}

	public void ChildDied(){
		enemiesOnScene -= 1;
	}
}
