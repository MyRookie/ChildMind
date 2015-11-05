using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	public Animator animator;

	private bool isSwimming;

	public Texture2D[] runImgs;
	
	public Texture2D[] jumpImgs;
	
	public MeshRenderer mesh;

	private Rigidbody rigid;

	private int swim_status;

	private static int SWIM_UP = 1;

	private static int SWIM_DOWN = 2;

	private static int SWIM_FORWARD = 3;

	private static int SWIM_BACKWORD = 4;

	private static int SWIM_STOP = 0;

	private static int STATE_MOVE = 1;
	
	private static int STATE_JUMP = 2;
	
	private static int STATE_STOP = 3;
	
	private static int DIR_LEFT = 1;
	
	private static int DIR_RIGHT = 2;
	
	//人物朝向(左右)
	private int dir;

	private bool onGround;

	//人物状态（行走、跳跃）
	private int state;
	
	private int frames;
	
	private Quaternion rotation;
	
	private Vector3 position;
	
	private Vector3 cameraPos;
	
	void Start ()
	{
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

	void FIX(){
		//fix the position of child(avoid rotate and move)
//		transform.eulerAngles = new Vector3 (0f, 90f, 0f);
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
		FIX ();

		//
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
		FIX ();


		if (state == STATE_MOVE && !onGround) { //if the character is falling
	//		animator.SetInteger("Status", 3);

			Debug.Log(onGround);
			Debug.Log("called");
			return;
		}
		
		//人物状态处理
		//静止处理
		if (state == STATE_STOP) {
			//
		} else if (state == STATE_JUMP) {
			//			Debug.Log("JUMP");

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

			transform.Translate (0.05f * Vector3.forward);

			//			Debug.Log(frames);
			if (Time.frameCount % 4 == 0) {
				frames++;
			}
			if (frames >= 5) {
				state = STATE_STOP;
			}
		}
		
		//按键控制
		if (state == STATE_STOP) {
			if (Input.GetKey (KeyCode.Space)) {
				//				Debug.Log("Jump");
				state = STATE_JUMP;
				frames = 0;
			} else if (Input.GetKey (KeyCode.LeftArrow)) {
				//				Debug.Log("Move");
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
			Debug.Log ("Enter:" + obj.gameObject.tag);
		}
	}
	
	void OnTriggerExit(Collider obj){
		if (obj.gameObject.tag == "water"){
			//start to use gravety to control
			rigid.useGravity = true;
			rigid.isKinematic = false;
			//modify the swimming status
			isSwimming = false;
			Debug.Log ("Exit:" + obj.gameObject.tag);
		}
	}

	
}
