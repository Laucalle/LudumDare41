using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMenu : MonoBehaviour {

    bool pressed, menuActive;
    GameObject menuPanel;
    GameObject imagePanel;
    Animator anim;
    float timeTrigger;

    // Use this for initialization
    void Start () {

        menuActive = false;
        anim = GetComponent<Animator>();

        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
            if (child.name == "MenuPanel") menuPanel = child.gameObject;
            if (child.name == "GameName") imagePanel = child.gameObject;
        }

        imagePanel.SetActive(true);

	}
	
	// Update is called once per frame
	void Update () {

        anim.SetTrigger("Start");
        
		if(Input.anyKey && Time.time - timeTrigger >= 2.5 && !menuActive)
        {
            menuPanel.SetActive(true);
            menuActive = true;
        }

	}
}
