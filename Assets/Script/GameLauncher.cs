using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoidFramework;
using System.IO;
using System;
using Game;

public class GameLauncher : MonoBehaviour {

    private static Transform sceneRoot;
    private static Transform sceneCamera;
    private static Transform uiRoot;
    private static Transform uiCamera;


    public static GameLauncher Instance {
        get;
        private set;
    }

    private RectTransform panel;
    private Text text;
    private Button button;


    private static readonly string VERSION_FILE = "bundle_list";
    private string SERVER_RES_URL = "";
    private string LOCAL_RES_URL = "";
    private string LOCAL_RES_PATH = "";

        
    private Dictionary<string, string> LocalBundleVersion;
    private Dictionary<string, string> ServerBundleVersion;
    private List<string> NeedDownFiles;
    private bool NeedUpdateLocalVersionFile = false;

    private void Awake() {
        Instance = this;

        uiRoot = GameObject.Find("UIRoot").transform;
        sceneRoot = GameObject.Find("SceneRoot").transform;

        //UIManager.Instance.Init();
        ABManager.Instance.Init();
    }



    private void Start() {
        //panel = GameObject.Find("Canvas/Panel").transform as RectTransform;
        //text = panel.Find("Text").GetComponent<Text>();
        //button = panel.Find("Button").GetComponent<Button>();

        //text.text += "persistentDataPath: " + Application.persistentDataPath + "\r\n";
        //text.text += "dataPath: " + Application.dataPath + "\r\n";
        //text.text += "temporaryCachePath: " + Application.temporaryCachePath + "\r\n";
        //text.text += "streamingAssetsPath: " + Application.streamingAssetsPath + "\r\n";

        //string ss = string.Empty;

        //button.onClick.AddListener(() => {
        //    //if (Application.platform == RuntimePlatform.) {
        //        try {

        //            DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath);

        //            ss += di.GetFiles().Length + "\r\n";
        //            //Debug.Log(di.GetFiles().Length);

        //            text.text = di.GetFiles().Length + "\r\n";

        //            File.Create(Application.persistentDataPath + "/test1.txt");

        //            ss += di.GetFiles().Length + "\r\n";

        //            text.text += di.GetFiles().Length;
        //            //Debug.Log(di.GetFiles().Length);


        //        } catch (Exception ex) {
        //            ss += ex.Message;
        //            text.text = ss;
        //        }
        //    //}
        //});


        //StartCoroutine(LoadAssetBundle());


        //            panel = GameObject.Find("Canvas/Panel").transform as RectTransform;
        //            text = panel.Find("Text").GetComponent<Text>();

        //            //text.text = Application.persistentDataPath;

        //            //text.text = Application.platform.ToString();


        //            SERVER_RES_URL = "file://" + Application.streamingAssetsPath + "/android";


        //#if UNITY_EDITOR && UNITY_ANDROID
        //            text.text = "1";
        //            //Deb

        //            //SERVER_RES_URL = "file://" + Application.streamingAssetsPath + "/android/";
        //#elif UNITY_EDITOR && UNITY_IOS

        //            text.text = "2";

        //#elif UNITY_ANDROID

        //            text.text = "3";

        //#elif UNITY_IOS

        //            text.text = "4";

        //#endif

        //            LocalBundleVersion = new Dictionary<string, string>();
        //            ServerBundleVersion = new Dictionary<string, string>();
        //            NeedDownFiles = new List<string>();



    }

    private void Update() {

    }

    //public Coroutine StartCoroutineCustom(IEnumerator enumerator) {
    //    return StartCoroutine(enumerator);
    //}

    //public void StopCoroutineCustom(Coroutine coroutine) {
    //    StopCoroutine(coroutine);
    //}


    //public IEnumerator LoadAssetBundle() {
        //using (WWW www = new WWW("file://" + Application.streamingAssetsPath + "/" + "windows/prefabs/panel.prefab.unity3d")) {
        //    Debug.Log("hahah");
        //    yield return www;
        //    Debug.Log("heihei");
        //    if (www.progress >= 1) {
        //        Debug.Log("zzzzz");
        //        AssetBundle ab = www.assetBundle;

        //        Debug.Log(ab);

        //        if (ab != null) {

        //            var prefab = ab.LoadAsset("Panel");

        //            Debug.Log(prefab);

        //            Instantiate(prefab);

        //            //assetLoader = new AssetLoader(ab);

        //            //if (abLoadCompleteHandler != null) {
        //            //    abLoadCompleteHandler(abName);
        //            //}

        //        } else {
        //            // Debug.LogError(GetType() + "/LoadAssetBundle()/www下载出错,请检查! AssetBundle URL: " + abDownloadPath + " 错误信息: " + www.error);
        //        }
        //    }
        //}
    //}


    public static void AddToUI(Transform transform) {
        transform.SetParent(uiRoot, false);
    }

    public static void AddToScene(Transform transform) {
        transform.SetParent(sceneRoot, false);
    }

    public Coroutine StartCoroutineCustom(IEnumerator coroutine) {
        return StartCoroutine(coroutine);
    }

    public void StopCoroutineCustom(Coroutine coroutine) {
        StopCoroutine(coroutine);
    }

    public void StopAllCoroutinesCustom() {
        StopAllCoroutines();
    }
}




