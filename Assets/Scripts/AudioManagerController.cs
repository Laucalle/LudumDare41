using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerController : MonoBehaviour {

	public AudioClip[] player_steps;
	public AudioClip[] doll_steps;
	public AudioClip[] laughs;
	public AudioClip[] voices;
	public AudioClip shot;

	private float doll_step_delay;
	private float player_step_delay;
	private float voices_delay;
	private float shot_delay;

	private float doll_step_time;
	private float player_step_time;
	private float voices_time;
	private float shot_time;

	private AudioSource audio_source;

	void Start () {
		audio_source = GetComponent<AudioSource> ();

		doll_step_delay = 0.2f;
		player_step_delay = 0.2f;
		voices_delay = 1f;
		shot_delay = 0;

		doll_step_time = 0;
		player_step_time = 0;
		voices_time = 0;
		shot_time = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (doll_step_time > 0) {
			doll_step_time -= Time.deltaTime;
		}
		if (player_step_time > 0) {
			player_step_time -= Time.deltaTime;
		}
		if (voices_time > 0) {
			voices_time -= Time.deltaTime;
		}
		if (shot_time > 0) {
			shot_time -= Time.deltaTime;
		}
	}

	public void PlayerSteps () {
		if (player_step_time <= 0) {
			player_step_time = player_step_delay;
			PlayRandomOneClip (player_steps);
		}
	}

	public void DollSteps () {
		if (doll_step_time <= 0) {
			doll_step_time = doll_step_delay;
			PlayRandomOneClip (doll_steps);
		}
	}

	public void LaughsAndVoices () {
		if (voices_time <= 0) {
			voices_time = voices_delay;
			int rand = Random.Range (1, 6);
			if (rand == 5) {
				PlayRandomOneClip (voices);
			} else {
				PlayRandomOneClip (laughs);
			}
		}
	}

	public void Shot() {
		if (shot_time <= 0) {
			shot_time = shot_delay;
			PlayOneShot (shot);
		}
	}

	private void PlayRandomOneClip(AudioClip[] audio_clips) {
		int rand = Random.Range (0, audio_clips.Length);
		PlayOneShot (audio_clips [rand]);
	}

	private void PlayOneShot(AudioClip audio) {
		audio_source.PlayOneShot (audio, 1f);
	}
}
