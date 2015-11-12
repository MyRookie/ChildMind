using UnityEngine;
using System.Collections;

public class RockVoice : MonoBehaviour {

	public bool pre;



	private const float MIN_DIS = 0.05f;

	public AudioClip roll;
	public AudioClip crash;

	public Rigidbody rigid;

		private Transform objectTransfom;

	private int noMovementFrames = 3;
	Vector3 previousLocations;
	private bool isMoving;

	// Use this for initialization
	void Start () {
		rigid = GetComponentInParent<Rigidbody> ();
		objectTransfom = rigid.transform;
		previousLocations = objectTransfom.position;
	}
	
//	// Update is called once per frame
//	void Update () {
//		Debug.Log (rigid.);
//	}
//
//	void changeStatus(){
//
//
//	}



	//Let other scripts see if the object is moving
	

	
	void Update()
	{
		noMovementFrames--;
		if (noMovementFrames == 0) {
			noMovementFrames = 3;

			if(Vector3.Distance(previousLocations,objectTransfom.position) < MIN_DIS)
				isMoving = false;
			else
				isMoving = true;

			previousLocations = objectTransfom.position;
		}

		if (pre != isMoving && isMoving) {
			GetComponent<AudioSource>().PlayOneShot(roll);
			pre = isMoving;
		}
		if (pre != isMoving && !isMoving) {
			GetComponent<AudioSource>().PlayOneShot(crash);
			pre = isMoving;
		}


	}

}
