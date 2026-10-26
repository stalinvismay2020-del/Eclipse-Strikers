using System.Collections.Generic;
using UnityEngine;

// Early Unity migration prototype. All geometry is generated at runtime;
// replace the placeholder shapes with original, licensed character assets.
public class EclipseStrikersBootstrap : MonoBehaviour
{
    const float Length=30f, Width=18f, MatchSeconds=300f;
    readonly List<Transform> home=new List<Transform>(), away=new List<Transform>();
    Transform ball, player, cam;
    Vector3 move;
    float timeLeft=MatchSeconds;
    int homeScore, awayScore;
    bool ended;
    GUIStyle hud, buttons;

    void Start(){ Application.targetFrameRate=60; Pitch(); Goals(); Teams(); Ball(); CameraSetup(); }

    Material MaterialOf(Color color){
        Shader shader=Shader.Find("Universal Render Pipeline/Lit");
        if(shader==null) shader=Shader.Find("Standard");
        var material=new Material(shader); material.color=color; return material;
    }
    GameObject Box(string n,Vector3 p,Vector3 s,Color c){
        var g=GameObject.CreatePrimitive(PrimitiveType.Cube); g.name=n; g.transform.position=p; g.transform.localScale=s;
        g.GetComponent<Renderer>().material=MaterialOf(c); return g;
    }
    void Pitch(){
        Color grass=new Color(.07f,.32f,.15f), white=new Color(.9f,.94f,.9f);
        Box("Pitch",Vector3.zero,new Vector3(Length,.2f,Width),grass);
        Box("Halfway line",new Vector3(0,.12f,0),new Vector3(.07f,.025f,Width),white);
        Box("Touchline",new Vector3(0,.12f,Width/2),new Vector3(Length,.025f,.07f),white);
        Box("Touchline",new Vector3(0,.12f,-Width/2),new Vector3(Length,.025f,.07f),white);
        Box("Endline",new Vector3(-Length/2,.12f,0),new Vector3(.07f,.025f,Width),white);
        Box("Endline",new Vector3(Length/2,.12f,0),new Vector3(.07f,.025f,Width),white);
        var circle=GameObject.CreatePrimitive(PrimitiveType.Cylinder); circle.name="Center circle";
        circle.transform.position=new Vector3(0,.13f,0); circle.transform.localScale=new Vector3(5,.025f,5);
        circle.GetComponent<Renderer>().material=MaterialOf(white);
        var inner=GameObject.CreatePrimitive(PrimitiveType.Cylinder); inner.name="Center circle grass";
        inner.transform.position=new Vector3(0,.16f,0); inner.transform.localScale=new Vector3(4.8f,.03f,4.8f);
        inner.GetComponent<Renderer>().material=MaterialOf(grass);
    }
    void Goals(){
        foreach(int side in new int[]{-1,1}){
            float x=side*(Length/2+1);
            Box("Goal post",new Vector3(x,.9f,-2),new Vector3(.15f,1.8f,.15f),Color.white);
            Box("Goal post",new Vector3(x,.9f,2),new Vector3(.15f,1.8f,.15f),Color.white);
            Box("Crossbar",new Vector3(x,1.8f,0),new Vector3(.15f,.15f,4.1f),Color.white);
            Box("Net back",new Vector3(x+side*.7f,.9f,0),new Vector3(.08f,1.8f,4),Color.gray);
        }
    }
    Transform MakePlayer(string n,Vector3 pos,Color shirt,bool isHome){
        var root=new GameObject(n).transform; root.position=pos;
        var body=GameObject.CreatePrimitive(PrimitiveType.Capsule); body.name="Body"; body.transform.SetParent(root,false);
        body.transform.localPosition=new Vector3(0,.9f,0); body.transform.localScale=new Vector3(.62f,.85f,.62f);
        body.GetComponent<Renderer>().material=MaterialOf(shirt);
        var head=GameObject.CreatePrimitive(PrimitiveType.Sphere); head.name="Head"; head.transform.SetParent(root,false);
        head.transform.localPosition=new Vector3(0,1.85f,0); head.transform.localScale=Vector3.one*.46f;
        head.GetComponent<Renderer>().material=MaterialOf(new Color(.94f,.7f,.53f));
        root.gameObject.AddComponent<PlayerMarker>().isHome=isHome; return root;
    }
    void Teams(){
        for(int i=0;i<4;i++){
            float z=(i-1.5f)*3.2f;
            home.Add(MakePlayer("Eclipse FC "+(i+1),new Vector3(-8+i*2,0,z),new Color(.05f,.32f,1),true));
            away.Add(MakePlayer("Rival FC "+(i+1),new Vector3(7-i*2,0,-z),new Color(.9f,.06f,.15f),false));
        }
        player=home[1];
    }
    void Ball(){
        var b=GameObject.CreatePrimitive(PrimitiveType.Sphere); b.name="Football"; b.transform.position=new Vector3(-1,.5f,0);
        b.transform.localScale=Vector3.one*.62f; b.GetComponent<Renderer>().material=MaterialOf(Color.white);
        var rb=b.AddComponent<Rigidbody>(); rb.mass=.45f; rb.drag=.45f; rb.angularDrag=.2f;
        rb.collisionDetectionMode=CollisionDetectionMode.Continuous; ball=b.transform;
    }
    void CameraSetup(){
        var c=Camera.main;
        if(c==null){var go=new GameObject("Main Camera"); c=go.AddComponent<Camera>(); go.tag="MainCamera";}
        cam=c.transform; cam.position=new Vector3(0,16,-19); cam.rotation=Quaternion.Euler(38,0,0); c.fieldOfView=62;
    }
    void Update(){
        if(ended)return; timeLeft=Mathf.Max(0,timeLeft-Time.deltaTime); if(timeLeft<=0)ended=true;
        ReadMove(); player.position+=move.normalized*5.2f*Time.deltaTime;
        player.position=new Vector3(Mathf.Clamp(player.position.x,-14,14),0,Mathf.Clamp(player.position.z,-8,8));
        if(move.sqrMagnitude>.05f)player.rotation=Quaternion.Slerp(player.rotation,Quaternion.LookRotation(move),Time.deltaTime*10);
        if((Input.GetKeyDown(KeyCode.Space)||ShootTapped())&&Vector3.Distance(player.position,ball.position)<2.8f)Kick();
        if(Input.GetKeyDown(KeyCode.Tab))SwitchPlayer();
        foreach(var p in away) Chase(p,ball.position,2.5f);
        foreach(var p in home)if(p!=player)Chase(p,ball.position+new Vector3(-1,0,Mathf.Sin(Time.time+p.position.z)),1.6f);
        cam.position=Vector3.Lerp(cam.position,player.position+new Vector3(0,14,-17),Time.deltaTime*3);
        cam.LookAt(player.position+Vector3.up*.5f); CheckGoal();
    }
    void ReadMove(){
        move=Vector3.zero;
        if(Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.UpArrow))move.z++;
        if(Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.DownArrow))move.z--;
        if(Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.LeftArrow))move.x--;
        if(Input.GetKey(KeyCode.D)||Input.GetKey(KeyCode.RightArrow))move.x++;
        if(Input.touchCount>0){
            Touch t=Input.GetTouch(0);
            if(t.position.x<Screen.width*.48f){
                Vector2 d=(t.position-new Vector2(Screen.width*.22f,Screen.height*.22f))/Mathf.Max(80,Screen.width*.16f);
                move=new Vector3(Mathf.Clamp(d.x,-1,1),0,Mathf.Clamp(d.y,-1,1));
            }
        }
    }
    bool ShootTapped(){
        for(int i=0;i<Input.touchCount;i++)if(Input.GetTouch(i).phase==TouchPhase.Began&&Input.GetTouch(i).position.x>Screen.width*.55f)return true;
        return false;
    }
    void Chase(Transform p,Vector3 target,float speed){
        target.y=0; Vector3 d=target-p.position; if(d.magnitude>1.4f)p.position+=d.normalized*speed*Time.deltaTime;
    }
    void SwitchPlayer(){int i=home.IndexOf(player);player=home[(i+1)%home.Count];}
    void Kick(){if(Vector3.Distance(player.position,ball.position)>3f)return;
        Vector3 d=(Vector3.right+player.forward*.35f+Vector3.up*.22f).normalized;
        ball.GetComponent<Rigidbody>().AddForce(d*12f,ForceMode.Impulse);
    }
    void CheckGoal(){
        if(Mathf.Abs(ball.position.x)<Length/2-1||Mathf.Abs(ball.position.z)>2.2f||ball.position.y>3)return;
        if(ball.position.x>0)homeScore++;else awayScore++;
        ball.position=new Vector3(0,.5f,0); var rb=ball.GetComponent<Rigidbody>();rb.velocity=Vector3.zero;rb.angularVelocity=Vector3.zero;
        for(int i=0;i<4;i++){float z=(i-1.5f)*3.2f;home[i].position=new Vector3(-8+i*2,0,z);away[i].position=new Vector3(7-i*2,0,-z);}
    }
    void OnGUI(){
        if(hud==null){hud=new GUIStyle(GUI.skin.label);hud.fontSize=Mathf.Max(18,Screen.height/38);hud.normal.textColor=Color.white;buttons=new GUIStyle(GUI.skin.button);buttons.fontSize=20;}
        GUI.Box(new Rect(10,10,Screen.width-20,56),"");
        int seconds=Mathf.CeilToInt(timeLeft);
        GUI.Label(new Rect(22,15,Screen.width-44,45),"ECLIPSE STRIKERS     "+homeScore+" - "+awayScore+"     "+(seconds/60)+":"+((seconds%60).ToString("00")),hud);
        if(GUI.Button(new Rect(Screen.width-150,Screen.height-100,130,72),"SHOOT"))Kick();
        if(GUI.Button(new Rect(Screen.width-150,Screen.height-180,130,64),"SWITCH"))SwitchPlayer();
        if(ended)GUI.Box(new Rect(Screen.width/2-160,Screen.height/2-55,320,110),"FULL TIME  "+homeScore+" - "+awayScore);
    }
}
public class PlayerMarker : MonoBehaviour { public bool isHome; }
