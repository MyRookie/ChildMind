using UnityEngine;
using System.Collections;

public class PetAI : MonoBehaviour {
	
	public float M_DIS = 2f; 

	private bool DIRECTION;

	private bool onGround;
	private bool isJumping;

	private int OffsetY = 1;
	private const bool LEFT = true;
	private  bool RIGHT = false;


	private int timer;

	private float Range; 

	private Rigidbody Rigid;
	private Rigidbody TargetRigid;
	
	// Use this for initialization
	void Start () {

		isJumping = false;
		TargetRigid = GameObject.Find ("Child").GetComponent<Rigidbody> ();
	}


	// Update is called once per frame
	void Update () {

		//Debug.Log(Vector3.Distance(transform.position,TargetRigid.transform.position));
		FollowTarget ();

	}

	void FollowTarget(){
		// judge the distance
		if (!onGround && isJumping) {
			Jump(DIRECTION);
			return;
		}
		
		//change the status of jump after reach the ground
		isJumping = false;
		
		//judge if the distance is big enough follow
		Range = Vector3.Distance(transform.position,TargetRigid.transform.position);
		if (Range <= M_DIS) {
			//if the target is in the chasing range, but the target is at a lower position, probably need to move towards target as well
			if (transform.position.y - TargetRigid.transform.position.y > OffsetY){
				ResetDirection();
				MoveTo(DIRECTION);
			}
			return;
		}



		//get move direction
		ResetDirection();

		if (PlayerControl.state == PlayerControl.STATE_JUMP) {
			MoveTo (DIRECTION);
			return;
		}

		//if target is in different position of heigt, Detect the environment around and check if it necessary to jump
		if (Mathf.Abs( transform.position.y - TargetRigid.transform.position.y) > OffsetY) {
			//detect nearby and judge if it is nessary to jump
			DetectJump();
			return;
		}
		
		//otherwise, just move toward the target
		MoveTo (DIRECTION);
	}

	void ResetDirection(){
		if ((transform.position.x - TargetRigid.transform.position.x) > 0)
			DIRECTION = LEFT;
		else
			DIRECTION = RIGHT;
	}

	int DetectJump(){

		int HIT_DIR;
		if (transform.position.y - TargetRigid.transform.position.y < OffsetY) {//Detect top
			if(DIRECTION == LEFT){
				Vector3 RAY = new Vector3(-1f,1.0f,0);
				if (Physics.Raycast (transform.position, RAY, 3.0f))
					MoveTo(RIGHT);
				else{
					timer = 0;
					isJumping = true;
					Jump(DIRECTION);//jump to left
				}
			}
			if(DIRECTION == RIGHT){
				Vector3 RAY = new Vector3(1f,1.0f,0);
				if (Physics.Raycast (transform.position, RAY, 3.0f))
					MoveTo(LEFT);
				else{
					timer = 0;
					isJumping = true;
					Jump(DIRECTION);//jump to left
				}
			}
		} 
		else {
//			Vector3 RAY = transform.TransformDirection (Vector3.down);
//			if (Physics.Raycast (transform.position, RAY, 3.0f))
				MoveTo(DIRECTION);
		}

		return 1;
	}

	void Jump(bool DIR) {

		timer++;
		if (timer < 20) {
			transform.Translate (0, 0.15f, 0f);
			if (DIR == LEFT) {
				transform.Translate (-0.05f, 0f, 0f);
				//mesh.material.mainTexture = jumpImgs[9 + 4];
			} else if (DIR == RIGHT) {
				transform.Translate (0.05f, 0f, 0f);
				//mesh.material.mainTexture = jumpImgs[4];
			}
		}
		else{
			if (DIR == LEFT) {
				transform.Translate (-0.05f, 0f, 0f);
			} else if (DIR == RIGHT) {
				transform.Translate (0.05f, 0f, 0f);
			}	
		}
	}

	void OnCollisionExit(Collision obj){
		onGround = false;
	}
	void OnCollisionEnter(Collision obj){
		onGround = true;
	}

	void MoveTo(bool DIR) {
		if(DIR == LEFT)
			transform.Translate (-0.05f, 0f, 0f);
		if(DIR == RIGHT)
			transform.Translate (0.05f, 0f, 0f);
	}

}
