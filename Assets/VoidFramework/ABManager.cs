using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;
using System.IO;

namespace VoidFramework {

    public class VersionData {
        public string Version;
        public List<SingleResouce> Resouces;
    }

    public class SingleResouce {
        public string Name;
        public string Hash;
    }

    public class ABManager {

        public static readonly string SERVERPATH = "http://192.168.8.105/AB/";
        
        public static readonly string VERSIONFILENAME = "resource_version.json";


        /// <summary>
        /// 获取平台对应文件夹
        /// </summary>
        public static string GetRuntimePlatform() {
            string platform = "";
            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) {
                platform = "windows";
            } else if (Application.platform == RuntimePlatform.Android) {
                platform = "android";
            } else if (Application.platform == RuntimePlatform.IPhonePlayer) {
                platform = "ios";
            } else if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor) {
                platform = "osx";
            }
            return platform;
        }

        //public readonly string VERSIONPATH = "http://192.168.8.105/AB/resource_version.json";

        private Dictionary<string, AssetBundle> abDict = new Dictionary<string, AssetBundle>();

        private static ABManager instance;

        public static ABManager Instance {
            get {
                if (instance == null) {
                    instance = new ABManager();
                }
                return instance;
            }
        }

        private Coroutine coroutine;

        public void Init() {
            Debug.Log("ABManager Init");


            //GameLauncher.Instance.StartCoroutine(StartDownLoad(VERSIONPATH));
            //coroutine = GameLauncher.Instance.StartCoroutine(Test1());

            GameLauncher.Instance.StartCoroutine(DownloadVersionFileCorountine(SERVERPATH + GetRuntimePlatform() + "/" +VERSIONFILENAME , DownloadVersionFileComplete));
        }

        
        public IEnumerator DownloadVersionFileCorountine(string versionPath, Action callback) {
            using (UnityWebRequest request = UnityWebRequest.Get(versionPath)) {
                yield return request.SendWebRequest();

                if (request.isDone) {

                    var versionData = JsonConvert.DeserializeObject<VersionData>(request.downloadHandler.text);

                    Debug.Log("版本文件下载完成");
                    Debug.Log(versionData.Version);

                    foreach (var item in versionData.Resouces) {
                        Debug.Log(item.Name + "|" + item.Hash);
                    }

                    if (!Directory.Exists(Application.persistentDataPath + "/" + GetRuntimePlatform())) {
                        Directory.CreateDirectory(Application.persistentDataPath + "/" + GetRuntimePlatform());
                    }

                    if (!File.Exists(Application.persistentDataPath + "/" + GetRuntimePlatform() + "/" + VERSIONFILENAME)) {
                        Debug.Log("没有版本文件");
                    } else {
                        Debug.Log("有版本文件");
                    }

                    //File.WriteAllBytes(Application.persistentDataPath + "/" + )

                    //var tempFilePath = Application.persistentDataPath + "/" + abPaths[ii - 1];

                    ////Debug.Log(Application.persistentDataPath + "/" + abPaths[ii - 1].Split('/')[0]);

                    //if (!Directory.Exists(Application.persistentDataPath + "/" + abPaths[ii - 1].Split('/')[0])) {
                    //    Directory.CreateDirectory(Application.persistentDataPath + "/" + abPaths[ii - 1].Split('/')[0]);
                    //}

                    //File.WriteAllBytes(Application.persistentDataPath + "/" + abPaths[ii - 1], request.downloadHandler.data);




                    //File.WriteAllText()



                    if (callback != null) {
                        callback();
                    }
                }
            }
        }


        private void DownloadVersionFileComplete() {
            //txt_Loading.text = "版本文件下载完成";

            //ABManager.Instance.DownloadABs((progress) => {
            //    txt_Loading.text = "下载AB包";

            //    //Debug.Log(progress);

            //    txt_Loading.text = progress.ToString();
            //    slider_Progress.value = progress;

            //    if (progress == 1) {
            //        txt_Loading.text = "加载AB包";

            //        ABManager.Instance.LoadABs((loadProgress) => {
            //            //Debug.Log(loadProgress);

            //            if (loadProgress == 1) {
            //                txt_Loading.text = "AB包加载完成";

            //                UIManager.Instance.InitOther();
            //            }
            //        });
            //    }
            //});

        }

        private string[] abPaths = new string[] {
            "windows/prefabs",
            "windows/textures"
        };

        public void DownloadABs(Action<float> callback) {

            GameLauncher.Instance.StartCoroutine(DownloadAB(callback));

        }

        private int ii = 1;

        private IEnumerator DownloadAB(Action<float> callback) {
            //Debug.Log("ii:" + ii);
            using (UnityWebRequest request = UnityWebRequest.Get(SERVERPATH + abPaths[ii - 1])) {
                yield return request.SendWebRequest();

                if (request.isDone) {

                    //Debug.Log(abPaths[ii - 1]);

                    var tempFilePath = Application.persistentDataPath + "/" + abPaths[ii - 1];

                    //Debug.Log(Application.persistentDataPath + "/" + abPaths[ii - 1].Split('/')[0]);

                    if (!Directory.Exists(Application.persistentDataPath + "/" + abPaths[ii - 1].Split('/')[0])) {
                        Directory.CreateDirectory(Application.persistentDataPath + "/" + abPaths[ii - 1].Split('/')[0]);
                    }

                    File.WriteAllBytes(Application.persistentDataPath + "/" + abPaths[ii - 1], request.downloadHandler.data);


                    if (callback != null) {
                        callback((float)ii / (float)abPaths.Length);
                    }

                    ii++;

                    if (ii == abPaths.Length + 1) {
                        yield break;
                    } else {
                        GameLauncher.Instance.StartCoroutine(DownloadAB(callback));
                    }
                }
            }
        }

        //private IEnumerator DownloadAB(string assetPath) {

        //}

        private int i = 0;

        private IEnumerator DownloadVersionFile(string path) {
            //UnityWebRequest request = new UnityWebRequest(path);
            //yield return request.SendWebRequest();

            while (true) {
                if (i > 1000) {
                    Debug.Log(i++);
                } else {
                    CoroutineTools.Instance.StopCoroutine(coroutine);
                    Debug.Log("Stop");
                }
                
                yield return new WaitForEndOfFrame();
            }


        }

        private int index = 0; 

        public void LoadABs(Action<float> callback) {
            foreach (var item in abPaths) {
                //Debug.Log("abName: " + item);

                LoadAB(item, callback);
            }
        }

        private void LoadAB(string abName, Action<float> callback) {
#if UNITY_ANDROID
            AssetBundle ab = AssetBundle.LoadFromFile(Application.persistentDataPath + "/" + abName);
#else
            AssetBundle ab = AssetBundle.LoadFromFile(Application.persistentDataPath + "/" + abName);
#endif

            abDict.Add(abName.Split('/')[1], ab);


            //Debug.Log("Count: " + abDict.Values.Count);


            index++;
            if (callback != null) {
                callback((float)(index + 1) / abPaths.Length);
            }
        }

        public T LoadAsset<T>(string abName, string assetName) where T: UnityEngine.Object {
            Debug.Log(abDict[abName]);

            return abDict[abName].LoadAsset<T>(assetName) as T;
        }
    
    }
}

