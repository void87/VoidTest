using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoidFramework;

namespace Game {
    public class GameManager : MonoBehaviour {

        private RectTransform panel;
        private Text text;


        private static readonly string VERSION_FILE = "bundle_list";
        private string SERVER_RES_URL = "";
        private string LOCAL_RES_URL = "";
        private string LOCAL_RES_PATH = "";

        
        private Dictionary<string, string> LocalBundleVersion;
        private Dictionary<string, string> ServerBundleVersion;
        private List<string> NeedDownFiles;
        private bool NeedUpdateLocalVersionFile = false;





        void Start() {
            StartCoroutine(LoadAssetBundle());
            

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

        void Update() {

        }


        public IEnumerator LoadAssetBundle() {
            using (WWW www = new WWW("file://" + Application.streamingAssetsPath + "/" + "windows/prefabs/panel.prefab.unity3d")) {
                Debug.Log("hahah");
                yield return www;
                Debug.Log("heihei");
                if (www.progress >= 1) {
                    Debug.Log("zzzzz");
                    AssetBundle ab = www.assetBundle;

                    Debug.Log(ab);

                    if (ab != null) {

                        var prefab = ab.LoadAsset("Panel");

                        Debug.Log(prefab);

                        Instantiate(prefab);

                        //assetLoader = new AssetLoader(ab);

                        //if (abLoadCompleteHandler != null) {
                        //    abLoadCompleteHandler(abName);
                        //}

                    } else {
                        // Debug.LogError(GetType() + "/LoadAssetBundle()/www下载出错,请检查! AssetBundle URL: " + abDownloadPath + " 错误信息: " + www.error);
                    }
                }
            }
        }
    }
}



