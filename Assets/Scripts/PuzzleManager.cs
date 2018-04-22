using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour {

	public GameObject tile_prefab;
	public Sprite[] tile_sprites;
	public TileManager[] tile_managers;
	public Sprite blank_sprite;
	public Sprite end_sprite;
	public PlayerController player_controller;
	float offset;

	private bool end_tile_activated;
	//List<int> permutation;
	int blank_position;

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha0)) {
			tile_managers[0].SwitchTile();
		} else if (Input.GetKeyDown (KeyCode.Alpha1)) {
			tile_managers[1].SwitchTile();
		} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			tile_managers[2].SwitchTile();
		} else if (Input.GetKeyDown (KeyCode.Alpha3)) {
			tile_managers[3].SwitchTile();
		} else if (Input.GetKeyDown (KeyCode.Alpha4)) {
			tile_managers[4].SwitchTile();
		} else if (Input.GetKeyDown (KeyCode.Alpha5)) {
			tile_managers[5].SwitchTile();
		} else if (Input.GetKeyDown (KeyCode.Alpha6)) {
			tile_managers[6].SwitchTile();
		} else if (Input.GetKeyDown (KeyCode.Alpha7)) {
			tile_managers[7].SwitchTile();
		} else if (Input.GetKeyDown (KeyCode.Alpha8)) {
			tile_managers[8].SwitchTile();
		}
	}
    private void Awake()
    {
        offset = 2.57f;
    }
    // Use this for initialization
    void Start () {
		
		CreatePuzzle();

		end_tile_activated = false;
		blank_position = Random.Range (0, 9);
		tile_managers = GetComponentsInChildren<TileManager> ();

		AssignSprites ();
		MixPuzzle ();
		player_controller.SpawnPlayer(blank_position);
	}

	// Generates the tiles on the map
	void CreatePuzzle () {
		float x_off = -offset, y_off = offset;
		for (int i = 0; i < 9; i++) {
			Instantiate (tile_prefab, new Vector3 (x_off, y_off, 0), Quaternion.identity, transform);

			x_off += offset;
			if (i == 2 || i == 5) {
				y_off -= offset;
				x_off = -offset;
			}
		}
	}

	void AssignSprites() {
		int n_image = Random.Range (3, 4);
		string name_image = "background" + n_image;
		tile_sprites = Resources.LoadAll<Sprite> (name_image);
		for (int i = 0; i < 9; i++) {
			tile_managers [i].SetSprite(tile_sprites [i]);
			tile_managers [i].SetId(i);
		}
		tile_managers [blank_position].SetSprite(blank_sprite);
		tile_managers [blank_position].SetDeadly (true);
	}

	// Swaps positions blank and pos within permutation
	/*void SwapBlankPositionInPermutation (int pos) {
		int aux = permutation [blank_position];
		permutation [blank_position] = permutation [pos];
		permutation [pos] = aux;
	}*/

	/*void PrintPuzzle() {
		string str = "";
		for (int i=0; i<9; i++) {
			str += (tile_managers[i].GetId() + " ");
		}
		Debug.Log(str);
	}*/

	// Mixes the puzzle
	void MixPuzzle() {
		int moves = 0;
		while (moves < 1) {
			// 4 = up, 1 = right, 2 = down, 3 = left
			int rand_move = Random.Range(0,9);

			if (SwitchTile (rand_move) != -1) {
				moves++;
			}
		}
	}

	// Swaps sprites between tile_id and blank_position, and set the "deadly" value correctly for both
	private void SwapSprites(int tile_id) {
		tile_managers [tile_id].SetDeadly (true);
		tile_managers [blank_position].SetDeadly (false);

		Sprite aux = tile_managers[tile_id].GetSprite ();
		tile_managers [tile_id].SetSprite (tile_managers [blank_position].GetSprite ());
		tile_managers [blank_position].SetSprite(aux);

		int aux_id = tile_managers[tile_id].GetId ();
		tile_managers [tile_id].SetId (tile_managers [blank_position].GetId ());
		tile_managers [blank_position].SetId(aux_id);

		blank_position = tile_id;
	}

	public int TrySwitchTile(int tile_id) {
		int movement = SwitchTile (tile_id);
		if (movement != -1) {
			CheckIfSolved ();
		}
		return movement;
	}
	
	
	// Returns the direction of the movement: 1 = right, 2 = down, 3 = left,  4 = up, -1 = not-moved
	private int SwitchTile (int tile_id) {
		if (!end_tile_activated) {
			if (blank_position != 2 && blank_position != 5 && blank_position != 8 &&
			    tile_managers [blank_position + 1].GetId () == tile_id) {
				SwapSprites (blank_position + 1);
				return 1;
			} else if (blank_position != 6 && blank_position != 7 && blank_position != 8 &&
			           tile_managers [blank_position + 3].GetId () == tile_id) {
				SwapSprites (blank_position + 3);
				return 2;
			} else if (blank_position != 0 && blank_position != 3 && blank_position != 6 &&
			           tile_managers [blank_position - 1].GetId () == tile_id) {
				SwapSprites (blank_position - 1);
				return 3;
			} else if (blank_position != 0 && blank_position != 1 && blank_position != 2 &&
			           tile_managers [blank_position - 3].GetId () == tile_id) {
				SwapSprites (blank_position - 3);
				return 4;
			} else {
				return -1;
			}
		}
		return -1;
	}

	private void CheckIfSolved() {
		bool solved = true;

		for (int i=0; i<9; i++) {
			if (i != tile_managers [i].GetId ()) {
				solved = false;
			}
		}

		if (solved) {
			end_tile_activated = true;
			tile_managers [blank_position].SetEndTileTrue ();
			tile_managers [blank_position].SetSprite (end_sprite);
		}
	}
	
	public int GetBlankPosition() {
		return blank_position;
	}

	public float GetOffset() {
		return offset;
	}

	public List<int> UnspawnablePositions() {
		List<int> l = player_controller.GetTouchingTiles ();
		if (! l.Contains(blank_position)) {
			l.Add (blank_position);
		}
		return l;
	} 

	public int SpawnEnemyPosition() {
		List<int> l = player_controller.GetTouchingTiles ();
		int pos;
		do {
			pos = Random.Range (0, 9);
		} while (pos == blank_position || l.Contains(tile_managers[pos].GetId()));
		return pos;
	}
}
