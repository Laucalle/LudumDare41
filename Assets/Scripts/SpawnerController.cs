using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour {

	public float spawnFreq;
	public GameObject enemyPrefab;
	public GameObject player;
	private float prevSpawn;

	// Use this for initialization
	void Start () {

		prevSpawn = Time.time;
		
	}
	
	// Update is called once per frame
	void Update () {

		if(Time.time - prevSpawn > spawnFreq){
			GameObject enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
			enemy.GetComponent<EnemyController> ().player = player;
			prevSpawn = Time.time;
		}
		
	}
}
