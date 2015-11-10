//using UnityEngine;
//using System.Collections;
//using System;
//using System.Collections.Generic;
//
//public class Parabola : MonoBehaviour 
//{   
//	private float a;
//	private float b;
//	private float c;
//
//	private LineRenderer mLine = new LineRenderer();
//	
//
//	public Material material;
//
//	private float size;
//
//	private List<Vector3> lineInfo;
//
//	void Start()	{
//		lineInfo = new List<Vector3>();
//		Init_Parabola ();
//	}
//
//
//
//	void Update () {
//
//
//
//	}
//
//	void Init_Parabola(){
//		a = 1;
//		b = 1;
//		c = 0;
//
//		float x, y, z = 0;
//
//		for (int i = 0; i != 10; ++i) {
//			x = i;
//			y = a*x*x + b*x +c;
//			z = 0;
//
//			Vector3 v = new Vector3(x,y,z);
//
//			lineInfo.Add(v);
//		}
//	}
//
//	//GL的绘制方法系统回调  
//	void OnPostRender() 
//	{ 
//		if(!material) 
//		{ 
//			Debug.LogError("material == null");  
//			return;  
//		} 
//		//材质通道，0为默认。  
//		material.SetPass(0);  
//		//绘制2D图像  
//		GL.LoadOrtho();  
//		//得到鼠标点信息总数量  
//		GL.Begin(GL.LINES);  
//		//遍历鼠标点的链表  
//		int size = lineInfo.Count;  
//		
//		for(int i =0; i < size - 1;i++) 
//		{ 
//			Vector3 start = lineInfo[i];  
//			Vector3 end = lineInfo[i+1];  
//			//绘制线  
//			DrawLineFun(start.x,start.y,end.x,end.y);  
//		} 
//		//结束绘制  
//		GL.End();  
//		
//	} 
//	
//	void DrawLineFun(float x1,float y1,float x2,float y2) 
//	{ 
//		Debug.Log("draw");
//		//绘制线段  
//		GL.Vertex(new Vector3(x1 / Screen.width,y1 / Screen.height,0));  
//		GL.Vertex(new Vector3(x2 / Screen.width, y2 / Screen.height,0));  
//	} 
//	
//}

using UnityEngine;
using System.Collections;

public class Parabola : MonoBehaviour {
	public float v = 2;
	public float an = 0.25f;
	//LineRenderer
	private LineRenderer lineRenderer;
	//定义一个Vector3,用来存储鼠标点击的位置
	private Vector3 position;
	//用来索引端点
	private int index = 0;
	//端点数
	private int LengthOfLineRenderer = 20;

	private bool Thro = false;

	void Start()
	{

		//添加LineRenderer组件
		lineRenderer = gameObject.AddComponent<LineRenderer>();
		//设置材质
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		//设置颜色
		lineRenderer.SetColors(Color.red, Color.yellow);
		//设置宽度
		lineRenderer.SetWidth(0.02f, 0.02f);
		
	}
	
	void Update()
	{  
 

		if (Input.GetKey (KeyCode.T)) {

			if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)){
				if(Input.GetKey(KeyCode.A)){
					an += 0.001f;
				}
				else{
					an -= 0.001f;
				}
			}

			Draw ();
		} else {
			lineRenderer.SetVertexCount(2);
			lineRenderer.SetPosition(0,new Vector3(0,0,0));
			lineRenderer.SetPosition(1,new Vector3(0,0,0));
		}


		
	}


	void Draw(){

		index = 0;

		lineRenderer.SetVertexCount(LengthOfLineRenderer * 1);

		//获取LineRenderer组件
		lineRenderer = GetComponent<LineRenderer>();
		//鼠标左击

		
		float a = -1f, b = 2f, c = 0f;



		float vy = v * Mathf.Sin (an*Mathf.PI);
		float vx = v * Mathf.Cos (an*Mathf.PI);

		a = (4f * -9.8f)/(3f*vy*vx);
		b = vy/vx;
		c = 0;
		
		//连续绘制线段
		while (index < LengthOfLineRenderer) {   

			for (int i=0; i != 1; ++i) {
				float buff = index;
				float x = buff / 10 + i / 1;
				//				Debug.Log(x);
				float y = a * x * x + b * x + c;
				float z = 0;
				
				position = new Vector3 (x + transform.position.x, y + transform.position.y + 1f, z + transform.position.z);
				
				//两点确定一条直线，所以我们依次绘制点就可以形成线段了
				lineRenderer.SetPosition (index * 1 + i, position);
			}
			index++;
		}
	}

	
	
}
