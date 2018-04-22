using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPlacing : MonoBehaviour {

    public GameObject[] walls;
    public PuzzleManager pm;
	// Use this for initialization
	void Start () {
        float offset = pm.GetOffset();
        float height = Camera.main.transform.position.z;

        //left
        walls[0].transform.localScale = new Vector3(1f, 3f * offset + 2, Mathf.Abs(height) + 1f);
        walls[0].transform.position = new Vector3(-1.5f * offset - 0.5f, 0f, height/2);
        //right
        walls[1].transform.localScale = new Vector3(1f, 3f * offset + 2, Mathf.Abs(height) + 1f);
        walls[1].transform.position = new Vector3(1.5f * offset + 0.5f, 0f, height / 2);
        //top
        walls[2].transform.localScale = new Vector3(3f * offset + 2, 1f, Mathf.Abs(height) + 1f);
        walls[2].transform.position = new Vector3(0f, +1.5f * offset + 0.5f, height / 2);
        //bottom
        walls[3].transform.localScale = new Vector3(3f * offset + 2, 1f, Mathf.Abs(height) + 1f);
        walls[3].transform.position = new Vector3(0f, -1.5f * offset - 0.5f, height / 2);
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
