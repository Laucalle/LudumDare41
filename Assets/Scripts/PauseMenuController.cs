using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour {

    public GameObject PauseMenu;

	// Use this for initialization
	void Start () {

        PauseMenu.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKey(KeyCode.Escape))
        {
            Pause();
            PauseMenu.SetActive(true);
        }
		
	}

    public void Pause()
    {
        Time.timeScale = 0;
		transform.gameObject.GetComponent<AudioSource> ().Pause ();
    }

    public void Resume()
    {
        Time.timeScale = 1;
		transform.gameObject.GetComponent<AudioSource> ().Play ();
    }
}
