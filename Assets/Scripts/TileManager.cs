using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {
	public bool deadly;
	private bool end_tile;
	private SpriteRenderer sprite_renderer;
	private PuzzleManager puzzle_manager;
	public int id;

	void Awake() {
		Start ();
	}

	void Start () {
		end_tile = false;
		sprite_renderer = GetComponent<SpriteRenderer> ();
		puzzle_manager = GetComponentInParent<PuzzleManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetDeadly(bool b) {
		deadly = b;
	}

	public bool GetDeadly() {
		return deadly;
	}

	public void SetEndTileTrue() {
		end_tile = true;
	}

	public bool GetEndTile() {
		return end_tile;
	}

	public void SetSprite(Sprite sp) {
		sprite_renderer.sprite = sp;
	}

	public Sprite GetSprite() {
		return sprite_renderer.sprite;
	}

	public void SetId (int i) {
		id = i;
	}

	public int GetId () {
		return id;
	}


	public void SwitchTile() {
		puzzle_manager.TrySwitchTile (id);
	}
}
