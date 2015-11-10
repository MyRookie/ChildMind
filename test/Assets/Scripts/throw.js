#pragma strict

    private var a:int;//a>0,开口向上；a<0,开口向下。  
 
    private var b:int;//b=0,抛物线对称轴为y轴  
 
    private var c:int;//c=0,抛物线经过原点  
 
    private var size:int;  
 
    function Start () {  
 
        a=-1;  
 
        b=1;  
 
        c=0;  
 
        size=10;  
 
        gameObject.AddComponent.<MeshFilter>();  
 
        gameObject.AddComponent.<MeshRenderer>();  
 
        var mesh : Mesh = GetComponent(MeshFilter).mesh;  
 
        mesh.Clear();  
 
        var v:Vector3[]=new Vector3[size];  
 
        var v2:Vector2[]=new Vector2[size];  
 
        var index:int []=new int[(size-2)*3];  
 
        for(var i:int=0;i<size;i++){  
 
 			
           if(i==0)  
           {  
               v[i]=Vector3(0,0,0);  
 
           }   
           else  
           {        
               var x:float=(i-size/2)*0.1;  
 

 			   v[i].x=x;  
 
               v[i].y=a*x*x+b*x+c;  
 
               v[i].z=0;  
           }  
 
           if(i>1){  
 
               index[3*i-6]=0;  
 
               index[3*i-5]=i-1;  
 
               index[3*i-4]=i;  
               //Unity3D教程手册：www.unitymanual.com  
           }  
 
           print("v["+i+"]="+v[i]);  
        }  
 
        mesh.vertices = v;  
 
        mesh.uv = v2;  
 
        mesh.triangles = index;  
 
        for(var j:int=0;j<index.Length;j++){  
 
            print(j+"=="+index[j]);  
        }  
 
    }