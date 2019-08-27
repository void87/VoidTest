using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoidFramework;
using System.IO;
using System;
using UnityEngine.Networking;
using Game;

public class GameLauncher : MonoBehaviour {

    private ILRuntime.Runtime.Enviorment.AppDomain appDomain;
    private object gameManager;

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

        appDomain = new ILRuntime.Runtime.Enviorment.AppDomain();

        uiRoot = GameObject.Find("UIRoot").transform;
        //sceneRoot = GameObject.Find("SceneRoot").transform;

        //ABManager.Instance.Init();
        //ABManager.Instance.DownloadVersionFile(() => {
        //    UIManager.Instance.InitLoading();
        //});

        //WWW www = new WWW(string.Empty);

        StartCoroutine(DownloadDLL());
    }



    private void Start() {

    }

    private void Update() {

    }


    private IEnumerator DownloadDLL() {
        using (UnityWebRequest request = UnityWebRequest.Get(ABManager.SERVERABPATH + "/" + "TestHotFix.dll")) {
            yield return request.SendWebRequest();

            if (request.isDone) {
                File.WriteAllBytes(Application.persistentDataPath + "/" + "TestHotFix.dll", request.downloadHandler.data);

                MemoryStream ms = new MemoryStream(request.downloadHandler.data);
                appDomain.LoadAssembly(ms, null, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());

                InitializeILRuntime();
                OnILRuntimeInitialized();
            }
        }
    }

    private void InitializeILRuntime() {
        LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appDomain);
        appDomain.RegisterCrossBindingAdaptor(new CoroutineAdapter());
    }

    private void OnILRuntimeInitialized() {
        gameManager = appDomain.Instantiate("TestHotFix.GameManager", null);

        appDomain.Invoke("TestHotFix.GameManager", "Init", gameManager, null);

        //appDomain.Invoke("TestHotFix.Class1", "Test1", null, null);
    }

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




