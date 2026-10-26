using System.Collections.Generic;
using UnityEngine;

// Procedural presentation and simple match AI layer for the Unity prototype.
// All generated assets use Unity primitives so the project has no external asset dependency.
public class EclipseStrikersPolish : MonoBehaviour
{
    class Rig
    {
        public Transform root, leftLeg, rightLeg, leftArm, rightArm, head;
        public Vector3 lastPosition;
        public bool home;
    }

    readonly List<Rig> rigs = new List<Rig>();
    Transform ball, keeperLeft, keeperRight;
    float aiKickTimer;
    Material MakeMat(Color c, float metallic = 0f)
    {
        Shader s = Shader.Find("Universal Render Pipeline/Lit");
        if (s == null) s = Shader.Find("Standard");
        Material m = new Material(s);
        m.color = c;
        if (m.HasProperty("_Metallic")) m.SetFloat("_Metallic", metallic);
        return m;
    }
    GameObject Part(string name, Transform parent, Vector3 pos, Vector3 scale, Color color, PrimitiveType type = PrimitiveType.Capsule)
    {
        GameObject g = GameObject.CreatePrimitive(type);
        g.name = name;
        g.transform.SetParent(parent, false);
        g.transform.localPosition = pos;
        g.transform.localScale = scale;
        Collider col = g.GetComponent<Collider>();
        if (col != null) Destroy(col);
        Renderer r = g.GetComponent<Renderer>();
        r.material = MakeMat(color, .08f);
        return g;
    }
    void Start()
    {
        BuildStadium();
        var ballObject = GameObject.Find("Football");
        if (ballObject != null) ball = ballObject.transform;
        PlayerMarker[] markers = FindObjectsOfType<PlayerMarker>();
        foreach (PlayerMarker marker in markers) BuildRig(marker);
        keeperLeft = BuildKeeper("Eclipse FC Keeper", -14.25f, new Color(.05f,.9f,.95f));
        keeperRight = BuildKeeper("Rival FC Keeper", 14.25f, new Color(1f,.65f,.08f));
    }
    Transform BuildKeeper(string keeperName, float x, Color trim)
    {
        Transform root = new GameObject(keeperName).transform;
        root.position = new Vector3(x,0,0);
        Part("Keeper body", root, new Vector3(0,.92f,0), new Vector3(.78f,.95f,.7f), trim);
        Part("Keeper head", root, new Vector3(0,1.9f,0), new Vector3(.48f,.48f,.48f), new Color(.94f,.7f,.53f), PrimitiveType.Sphere);
        Part("Keeper glove left", root, new Vector3(-.65f,1.05f,0), new Vector3(.25f,.25f,.25f), Color.white, PrimitiveType.Sphere);
        Part("Keeper glove right", root, new Vector3(.65f,1.05f,0), new Vector3(.25f,.25f,.25f), Color.white, PrimitiveType.Sphere);
        return root;
    }
    void BuildRig(PlayerMarker marker)
    {
        Transform root = marker.transform;
        Transform body = root.Find("Body");
        Transform head = root.Find("Head");
        if (body == null || head == null) return;
        Color jersey = marker.isHome ? new Color(.08f,.35f,1f) : new Color(.92f,.08f,.16f);
        Color trim = marker.isHome ? new Color(.12f,.95f,1f) : new Color(1f,.55f,.12f);
        Part("Anime hair", head, new Vector3(0,.35f,-.02f), new Vector3(.55f,.24f,.5f), new Color(.07f,.06f,.13f), PrimitiveType.Sphere);
        Part("Jersey trim", body, new Vector3(0,.17f,.02f), new Vector3(1.02f,.12f,1.02f), trim, PrimitiveType.Sphere);
        Part("Chest crest", body, new Vector3(0,.13f,.32f), new Vector3(.15f,.22f,.04f), Color.white, PrimitiveType.Sphere);
        Transform ll = Part("Left leg", root, new Vector3(-.2f,.38f,0), new Vector3(.19f,.65f,.22f), new Color(.08f,.08f,.14f)).transform;
        Transform rl = Part("Right leg", root, new Vector3(.2f,.38f,0), new Vector3(.19f,.65f,.22f), new Color(.08f,.08f,.14f)).transform;
        Part("Left boot", ll, new Vector3(0,-.43f,.08f), new Vector3(.22f,.13f,.38f), trim, PrimitiveType.Cube);
        Part("Right boot", rl, new Vector3(0,-.43f,.08f), new Vector3(.22f,.13f,.38f), trim, PrimitiveType.Cube);
        Transform la = Part("Left arm", root, new Vector3(-.42f,1.04f,0), new Vector3(.17f,.48f,.18f), jersey).transform;
        Transform ra = Part("Right arm", root, new Vector3(.42f,1.04f,0), new Vector3(.17f,.48f,.18f), jersey).transform;
        Part("Left glove", la, new Vector3(0,-.48f,0), Vector3.one*.14f, trim, PrimitiveType.Sphere);
        Part("Right glove", ra, new Vector3(0,-.48f,0), Vector3.one*.14f, trim, PrimitiveType.Sphere);
        rigs.Add(new Rig { root=root, leftLeg=ll, rightLeg=rl, leftArm=la, rightArm=ra, head=head, lastPosition=root.position, home=marker.isHome });
    }
    void BuildStadium()
    {
        Color dark = new Color(.025f,.035f,.09f);
        Color glow = new Color(.04f,.55f,1f);
        for (int side=-1; side<=1; side+=2)
        {
            MakeStand(new Vector3(0,1.2f,side*11.5f), new Vector3(34,2.5f,3), dark);
            for (int i=0;i<9;i++)
                Part("Stadium light", transform, new Vector3(-14+i*3.5f,5.4f,side*10.2f), new Vector3(.35f,.16f,.35f), i%2==0?glow:new Color(1f,.08f,.35f), PrimitiveType.Sphere);
            for (int i=0;i<5;i++)
                Part("Crowd tier", transform, new Vector3(-13+i*6.5f,2.7f,side*10.9f), new Vector3(4.5f,.22f,.35f), new Color(.1f,.12f,.24f), PrimitiveType.Cube);
        }
        for (int side=-1; side<=1; side+=2)
        {
            GameObject lightObject = new GameObject("Eclipse stadium spotlight");
            Light l = lightObject.AddComponent<Light>();
            l.type = LightType.Spot;
            l.color = side<0 ? new Color(.15f,.55f,1f) : new Color(1f,.16f,.35f);
            l.intensity = 4f;
            l.range = 32f;
            l.spotAngle = 75f;
            lightObject.transform.position = new Vector3(side*9,13,side*-2);
            lightObject.transform.rotation = Quaternion.Euler(55, side<0?25:-25, 0);
        }
    }
    void MakeStand(Vector3 pos, Vector3 scale, Color color)
    {
        GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "Stadium stand";
        g.transform.SetParent(transform,false);
        g.transform.localPosition=pos; g.transform.localScale=scale;
        g.GetComponent<Renderer>().material=MakeMat(color,.2f);
    }
    void UpdateKeeper(Transform keeper, float x)
    {
        if (keeper == null || ball == null) return;
        Vector3 target = keeper.position;
        target.x = x;
        target.z = Mathf.Clamp(ball.position.z * .62f, -1.65f, 1.65f);
        target.y = 0;
        keeper.position = Vector3.MoveTowards(keeper.position, target, 3.6f * Time.deltaTime);
        if (Mathf.Abs(ball.position.x - x) < 1.2f && Mathf.Abs(ball.position.z - keeper.position.z) < 1.25f && ball.position.y < 2.4f)
        {
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            if (rb != null && rb.velocity.sqrMagnitude > .1f)
            {
                rb.velocity = new Vector3(-Mathf.Sign(x) * Mathf.Abs(rb.velocity.x) * .7f, Mathf.Max(.7f, rb.velocity.y), rb.velocity.z + (ball.position.z-keeper.position.z)*1.2f);
            }
        }
    }
    void Update()
    {
        float dt = Mathf.Max(Time.deltaTime,.001f);
        foreach (Rig rig in rigs)
        {
            Vector3 delta=rig.root.position-rig.lastPosition;
            float run=Mathf.Clamp01(delta.magnitude/dt/3.5f);
            float swing=Mathf.Sin(Time.time*13f)*38f*run;
            rig.leftLeg.localRotation=Quaternion.Euler(swing,0,0);
            rig.rightLeg.localRotation=Quaternion.Euler(-swing,0,0);
            rig.leftArm.localRotation=Quaternion.Euler(-swing*.65f,0,0);
            rig.rightArm.localRotation=Quaternion.Euler(swing*.65f,0,0);
            rig.head.localRotation=Quaternion.Euler(0,Mathf.Sin(Time.time*2f+rig.root.position.x)*3f,0);
            rig.lastPosition=rig.root.position;
        }
        // Give the rival side a basic chance to attempt a shot instead of only chasing.
        if (ball != null)
        {
            UpdateKeeper(keeperLeft, -14.25f);
            UpdateKeeper(keeperRight, 14.25f);
            aiKickTimer-=Time.deltaTime;
            if (aiKickTimer<=0)
            {
                foreach (Rig rig in rigs)
                {
                    if (rig.home || Vector3.Distance(rig.root.position,ball.position)>1.65f) continue;
                    Vector3 target=new Vector3(-16f,.35f,0f)-ball.position;
                    ball.GetComponent<Rigidbody>().AddForce((target.normalized+Vector3.up*.18f)*7.5f,ForceMode.Impulse);
                    aiKickTimer=1.1f;
                    break;
                }
            }
        }
    }
}
