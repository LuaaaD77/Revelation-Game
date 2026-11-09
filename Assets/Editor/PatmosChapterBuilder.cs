#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public static class PatmosChapterBuilder
{
    [MenuItem("Tools/Revelation/Build Chapter 1: Patmos")]
    public static void Build()
    {
        if (!EditorUtility.DisplayDialog("Build Chapter 1",
            "Generate the playable Patmos scene? This replaces the current open scene.", "Build", "Cancel")) return;

        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        RenderSettings.ambientLight = new Color(.34f, .34f, .39f);
        RenderSettings.fog = true; RenderSettings.fogColor = new Color(.25f,.28f,.33f); RenderSettings.fogDensity = .012f;

        var gm = new GameObject("GameManager"); gm.AddComponent<GameManager>();
        var audio = new GameObject("AudioManager"); audio.AddComponent<AudioManager>();

        Material stone = Mat(new Color(.34f,.32f,.29f));
        Material sand = Mat(new Color(.48f,.40f,.29f));
        Material gold = Mat(new Color(.83f,.62f,.24f), true);
        Material dark = Mat(new Color(.075f,.095f,.12f));
        Material glow = Mat(new Color(1f,.78f,.31f), true);
        Material water = Mat(new Color(.08f,.18f,.25f));

        Cube("Island Ground", new Vector3(0,-.6f,0), new Vector3(32,1,32), sand);
        Cube("Sea", new Vector3(0,-1.25f,0), new Vector3(200,.2f,200), water);
        // Ruined columns and rocks around the island path.
        for(int i=0;i<34;i++)
        {
            float a=i*Mathf.PI*2/34f; float radius=12.5f+(i%3)*1.2f;
            Vector3 p=new Vector3(Mathf.Cos(a)*radius,0,Mathf.Sin(a)*radius);
            if(i%3==0) Cylinder("Ancient Pillar",p+Vector3.up*1.1f,new Vector3(.65f,2.2f,.65f),stone);
            else Cube("Island Rock",p+Vector3.up*.35f,new Vector3(1.2f,.8f,1.1f),stone);
        }
        // Path lanterns
        for(int z=-5;z<=7;z+=3)
        {
            Cylinder("Path Marker",new Vector3(-2,.35f,z),new Vector3(.18f,.7f,.18f),gold);
            Sphere("Lantern Glow",new Vector3(-2,1.0f,z),.28f,glow);
        }

        // Simple ancient sanctuary at the far end.
        Cube("Sanctuary Platform",new Vector3(0,.1f,10),new Vector3(10,.5f,5),stone);
        for(int x=-4;x<=4;x+=2)
        {
            Cylinder("Sanctuary Column",new Vector3(x,2.1f,11),new Vector3(.48f,4f,.48f),gold);
            Cylinder("Column Capital",new Vector3(x,4.2f,11),new Vector3(.85f,.25f,.85f),gold);
        }
        Cube("Sanctuary Roof",new Vector3(0,4.55f,11),new Vector3(10,.5f,5),dark);

        // Three fragments, placed on the walkable map.
        Fragment("The First Message", "Revelation 1:17–18",
            "Fear not; I am the first and the last: I am he that liveth, and was dead; and, behold, I am alive for evermore.",
            "The vision begins with the risen Christ, who tells John not to fear.",
            new Vector3(5,1,1), glow);
        Fragment("The Open Door", "Revelation 3:8",
            "Behold, I have set before thee an open door, and no man can shut it.",
            "The open door points to an opportunity and authority that Christ provides.",
            new Vector3(-6,1,5), glow);
        Fragment("The Faithful Promise", "Revelation 2:10",
            "Be thou faithful unto death, and I will give thee a crown of life.",
            "The promise calls believers to steadfast faithfulness through trials.",
            new Vector3(5,1,8), glow);

        // Gateway arch
        var gate = new GameObject("Heavenly Gateway");
        gate.transform.position = new Vector3(0,0,14);
        var left = GameObject.CreatePrimitive(PrimitiveType.Cube); left.name="Gateway Left"; left.transform.SetParent(gate.transform); left.transform.localPosition=new Vector3(-2,2,0); left.transform.localScale=new Vector3(.45f,4,.5f); left.GetComponent<Renderer>().sharedMaterial=gold;
        var right = GameObject.CreatePrimitive(PrimitiveType.Cube); right.name="Gateway Right"; right.transform.SetParent(gate.transform); right.transform.localPosition=new Vector3(2,2,0); right.transform.localScale=new Vector3(.45f,4,.5f); right.GetComponent<Renderer>().sharedMaterial=gold;
        var top = GameObject.CreatePrimitive(PrimitiveType.Cube); top.name="Gateway Top"; top.transform.SetParent(gate.transform); top.transform.localPosition=new Vector3(0,4,0); top.transform.localScale=new Vector3(4.4f,.45f,.5f); top.GetComponent<Renderer>().sharedMaterial=gold;
        var trigger=GameObject.CreatePrimitive(PrimitiveType.Cube); trigger.name="Gateway Interaction"; trigger.transform.SetParent(gate.transform); trigger.transform.localPosition=new Vector3(0,1.8f,0); trigger.transform.localScale=new Vector3(3.4f,3.2f,.8f); Object.DestroyImmediate(trigger.GetComponent<Renderer>());
        var box=trigger.GetComponent<BoxCollider>(); box.isTrigger=false;
        var gw=gate.AddComponent<HeavenlyGateway>(); gw.Prompt="Enter the heavenly gateway"; gw.gateVisual=gate.transform;

        // Player
        var player=new GameObject("Player");
        player.transform.position=new Vector3(0,0,-9);
        var cc=player.AddComponent<CharacterController>(); cc.height=1.8f; cc.radius=.32f; cc.center=new Vector3(0,.9f,0);
        var camObj=new GameObject("Player Camera"); camObj.tag="MainCamera"; camObj.transform.SetParent(player.transform); camObj.transform.localPosition=new Vector3(0,1.6f,0);
        var cam=camObj.AddComponent<Camera>(); cam.fieldOfView=75; cam.nearClipPlane=.05f;
        player.AddComponent<AudioListener>();
        var fps=player.AddComponent<FirstPersonController>(); fps.cameraRoot=camObj.transform;
        var inter=player.AddComponent<PlayerInteractor>(); inter.playerCamera=cam;

        // HUD using legacy Canvas + TextMeshPro components.
        var canvasGO=new GameObject("HUD Canvas",typeof(Canvas),typeof(CanvasScaler),typeof(GraphicRaycaster));
        var canvas=canvasGO.GetComponent<Canvas>(); canvas.renderMode=RenderMode.ScreenSpaceOverlay;
        var scaler=canvasGO.GetComponent<CanvasScaler>(); scaler.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize; scaler.referenceResolution=new Vector2(1920,1080);
        var hud=canvasGO.AddComponent<HUDController>(); inter.hud=hud;

        TMP_Text objective=Text(canvasGO.transform,"OBJECTIVE",new Vector2(.02f,.91f),new Vector2(.6f,.99f),26);
        TMP_Text prompt=Text(canvasGO.transform,"PROMPT",new Vector2(.25f,.04f),new Vector2(.75f,.13f),24);
        hud.objectiveText=objective; hud.promptText=prompt;

        GameObject journal=Panel(canvasGO.transform,"Journal Panel",new Vector2(.18f,.25f),new Vector2(.82f,.78f));
        hud.journalPanel=journal;
        hud.journalTitle=Text(journal.transform,"JOURNAL TITLE",new Vector2(.05f,.7f),new Vector2(.95f,.95f),27);
        hud.journalBody=Text(journal.transform,"JOURNAL BODY",new Vector2(.05f,.12f),new Vector2(.95f,.68f),23);
        GameObject complete=Panel(canvasGO.transform,"Chapter Complete",new Vector2(.2f,.3f),new Vector2(.8f,.7f));
        hud.completionPanel=complete;
        Text(complete.transform,"CHAPTER COMPLETE — THE DOOR IS OPEN",new Vector2(.05f,.35f),new Vector2(.95f,.7f),32);

        // Mobile interaction button. Touch look and joystick are handled by MobileInput.
        var mobile=new GameObject("Mobile Input"); mobile.AddComponent<MobileInput>();
        GameObject button=UIObject("Interact Button",canvasGO.transform,new Vector2(.83f,.12f),new Vector2(.97f,.27f));
        var img=button.AddComponent<Image>(); img.color=new Color(.35f,.25f,.12f,.85f);
        var btn=button.AddComponent<Button>(); var mi=button.AddComponent<MobileInteractButton>(); btn.onClick.AddListener(mi.Press);
        var label=Text(button.transform,"INTERACT",new Vector2(0,0),new Vector2(1,1),20); label.alignment=TextAlignmentOptions.Center;

        EditorSceneManager.SaveScene(scene,"Assets/Scenes/Chapter01_Patmos.unity");
        AssetDatabase.SaveAssets();
        Debug.Log("Chapter 1 Patmos scene created. Press Play to explore.");
    }

    static Material Mat(Color c, bool emission=false)
    {
        var m=new Material(Shader.Find("Standard")); m.color=c;
        if(emission){m.EnableKeyword("_EMISSION");m.SetColor("_EmissionColor",c*1.4f);}
        return m;
    }
    static void Cube(string n,Vector3 p,Vector3 s,Material m){var o=GameObject.CreatePrimitive(PrimitiveType.Cube);o.name=n;o.transform.position=p;o.transform.localScale=s;o.GetComponent<Renderer>().sharedMaterial=m;}
    static void Cylinder(string n,Vector3 p,Vector3 s,Material m){var o=GameObject.CreatePrimitive(PrimitiveType.Cylinder);o.name=n;o.transform.position=p;o.transform.localScale=s;o.GetComponent<Renderer>().sharedMaterial=m;}
    static void Sphere(string n,Vector3 p,float s,Material m){var o=GameObject.CreatePrimitive(PrimitiveType.Sphere);o.name=n;o.transform.position=p;o.transform.localScale=Vector3.one*s;o.GetComponent<Renderer>().sharedMaterial=m;}
    static void Fragment(string title,string verse,string quote,string journal,Vector3 pos,Material glow)
    {
        var o=GameObject.CreatePrimitive(PrimitiveType.Sphere);o.name=title;o.transform.position=pos;o.transform.localScale=Vector3.one*.65f;o.GetComponent<Renderer>().sharedMaterial=glow;
        var light=o.AddComponent<Light>();light.type=LightType.Point;light.range=5;light.intensity=2.2f;light.color=new Color(1,.75f,.35f);
        var f=o.AddComponent<ScriptureFragment>();f.Title=title;f.Verse=verse;f.JournalText=quote+"\n\n"+journal;
        f.Prompt="Discover "+title;
    }
    static GameObject Panel(Transform parent,string name,Vector2 min,Vector2 max)
    {
        var o=UIObject(name,parent,min,max);var image=o.AddComponent<Image>();image.color=new Color(.035f,.045f,.065f,.94f);return o;
    }
    static GameObject UIObject(string name,Transform parent,Vector2 min,Vector2 max)
    {
        var o=new GameObject(name,typeof(RectTransform));o.transform.SetParent(parent,false);
        var r=o.GetComponent<RectTransform>();r.anchorMin=min;r.anchorMax=max;r.offsetMin=Vector2.zero;r.offsetMax=Vector2.zero;return o;
    }
    static TMP_Text Text(Transform parent,string value,Vector2 min,Vector2 max,int size)
    {
        var o=UIObject(value,parent,min,max);var t=o.AddComponent<TextMeshProUGUI>();t.text=value;t.fontSize=size;t.color=new Color(.96f,.88f,.69f);t.enableWordWrapping=true;t.alignment=TextAlignmentOptions.Center;return t;
    }
}
#endif
