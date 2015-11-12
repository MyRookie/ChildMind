using UnityEngine;
using System.Collections;

public class PlayerVoice : MonoBehaviour {

	private int status;

	public AudioClip step_1;
	public AudioClip step_2;

	private int frame;

	// Use this for initialization
	void Start () {
		status = PlayerControl.state;
//		GetComponent<AudioSource>().PlayOneShot (step_1);
//		GetComponent<AudioSource>().PlayOneShot (step_2);
	}
	
	// Update is called once per frame
	void Update () {


		if (status != PlayerControl.state) {
			frame = 23;
			status = PlayerControl.state;
		}
		if (status == PlayerControl.STATE_MOVE) {
			if(frame == 23)
				GetComponent<AudioSource>().PlayOneShot (step_1);
			if(frame == 12)
				GetComponent<AudioSource>().PlayOneShot (step_2);
			if(frame == 0){
				frame = 23;
			}
			frame --;
		}
	
	}
}
