using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	public Animator animator;
	
	private bool isSwimming;

	
	public MeshRenderer mesh;
	
	private Rigidbody rigid;
	
	public int swim_status;
	
	public static int SWIM_UP = 1;
	
	public static int SWIM_DOWN = 2;
	
	public static int SWIM_FORWARD = 3;
	
	public static int SWIM_BACKWORD = 4;
	
	public static int SWIM_STOP = 0;
	
	public static int STATE_MOVE = 1;
	
	public static int STATE_JUMP = 2;
	
	public static int STATE_STOP = 3;
	
	public static int DIR_LEFT = 1;
	
	public static int DIR_RIGHT = 2;
	
	//人物朝向(左右)
	private int dir;
	
	private bool onGround;

	float moveSpeed;
	
	//人物状态（行走、跳跃）
	public static int state;
	
	private int frames;
	
	private Quaternion rotation;
	
	private Vector3 position;
	
	private Vector3 cameraPos;
	
	void Start ()
	{
			
		animator = GetComponentInChildren<Animator> ();
		isSwimming = false;
		rigid = GetComponent<Rigidbody> ();
		state = STATE_STOP;
		dir = DIR_RIGHT;
		rotation = Quaternion.Euler(0f,0f,0f);//new Quaternion (0, 0f, 0,0);
		//		Vector3 cp = new Vector3(-3.6f,0f,0f);
	}
	
	
	void Update ()
	{
		if (!isSwimming)
			LandControl ();
		else
			WaterControl ();
		
		
	}
	
	void fix_position(){

		//fix the position of child(avoid rotate and move)
		position = transform.position;
		position.z = -0.43f;
		transform.position = position;
		//transform.rotation = rotation;
		if(dir == DIR_LEFT)
			transform.rotation = Quaternion.Euler(0f,-90f,0f);
		if (dir == DIR_RIGHT)
			transform.rotation = Quaternion.Euler (0f, 90f, 0f);
	}
	
	void WaterControl(){
		
		//fix the position of the character
		fix_position ();

		if (swim_status == SWIM_STOP) {
			if (Input.GetKey (KeyCode.UpArrow)) {
				transform.Translate (0f, 0.03f, 0f);
				swim_status = SWIM_UP;
			}
			if (Input.GetKey (KeyCode.DownArrow)) {
				transform.Translate (0f, -0.03f, 0f);
				swim_status = SWIM_DOWN;
			}
			if (Input.GetKey (KeyCode.LeftArrow)) {
				swim_status = SWIM_FORWARD;
				transform.Translate (-0.03f, 0f, 0f);
			}
			if (Input.GetKey (KeyCode.RightArrow)) {
				swim_status = SWIM_BACKWORD;
				transform.Translate (0.03f, 0f, 0f);
			}
			swim_status = SWIM_STOP;
			transform.Translate (0f, -0.01f, 0f);
		}
		
	}
	
	void LandControl(){
		
		//fix the position of the character
		fix_position ();

		UpdateAnimation ();
		
		if (state == STATE_MOVE && !onGround) { //if the character is falling
			return;
		}
		
		//人物状态处理
		//静止处理
		if (state == STATE_STOP) {
			//
			moveSpeed = 0f;
		} else if (state == STATE_JUMP) {

			moveSpeed = 0f;

			frames++;
			if (frames < 18) {
				transform.Translate (0, 0.2f, 0f);
				transform.Translate (0.05f * Vector3.forward);
			}
			else{
				transform.Translate (0.05f * Vector3.forward);	
			}
			
			if (frames>36 && onGround) {
				state = STATE_STOP;
			}
			//移动处理
		} else if (state == STATE_MOVE) {
			
			
			moveSpeed = 0.05f;
			if(Input.GetKey (KeyCode.LeftShift))
				moveSpeed = 0.07f;
			
			transform.Translate (moveSpeed * Vector3.forward);

			if (Time.frameCount % 4 == 0) {
				frames++;
			}
			if (frames >= 5) {
				state = STATE_STOP;
			}
		}
		
		//按键控制
		if (state == STATE_STOP || state == STATE_MOVE) {
			if (Input.GetKey (KeyCode.Space)) {
				state = STATE_JUMP;
				frames = 0;
			} else if (Input.GetKey (KeyCode.LeftArrow)) {
				state = STATE_MOVE;
				frames = 0;
				dir = DIR_LEFT;
			} else if (Input.GetKey (KeyCode.RightArrow)) {
				frames = 0;
				state = STATE_MOVE;
				dir = DIR_RIGHT;
			}
		}
	}

	void UpdateAnimation(){

		float animationSpeed = moveSpeed * 200f;
		animator.SetFloat ("Speed", animationSpeed);



		if (state == STATE_MOVE) { //IN MOVING STATUS
			if(!onGround){
				animator.SetBool("Land",false);
//				Debug.Log("Move to: "+transform.position);
			}
		}
		if (state == STATE_JUMP) { //ON JUMP
			animator.SetBool("Jumping",true);
//			Debug.Log("Jumping at: "+transform.position);
		}
		else
			animator.SetBool("Jumping",false);
		//Debug.Log()
	}

	void OnCollisionExit(Collision obj){
		onGround = false;
	}
	
	void OnCollisionEnter(Collision obj){
		onGround = true;
	}
	
	void OnTriggerEnter(Collider obj){
		if (obj.gameObject.tag == "water") {
			//don't use gravety, but using the transform
			rigid.useGravity = false;
			rigid.isKinematic = true;
			//modify the swimming status
			isSwimming = true;
			swim_status = SWIM_STOP;
		}
	}
	
	void OnTriggerExit(Collider obj){
		if (obj.gameObject.tag == "water"){
			//start to use gravety to control
			rigid.useGravity = true;
			rigid.isKinematic = false;
			//modify the swimming status
			isSwimming = false;
		}
	}
	
	
}
