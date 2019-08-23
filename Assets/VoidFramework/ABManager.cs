using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;
using System;

namespace VoidFramework {

    public class ABManager {

        private class VersionData {
            public double id;
            public string version;
            public string manifest;
            public JsonData resource; 
        }

        private class SingleVersion {
            public string name;
            public string hash;
        }

        public readonly string SERVERPATH = "http://192.168.8.105/AB/";

        public readonly string VERSIONPATH = "http://192.168.8.105/AB/resource_version.json";

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
        }


        public IEnumerator DownloadVersionFile(string versionPath, Action callback) {
            Debug.Log(versionPath);
            using (UnityWebRequest request = UnityWebRequest.Get(versionPath)) {
                yield return request.SendWebRequest();

                if (request.isDone) {
                    //Debug.Log("isDone");
                    //Debug.Log(request.isNetworkError);
                    //Debug.Log(request.downloadHandler.text);

                    var version = JsonMapper.ToObject<VersionData>(request.downloadHandler.text);

                    //Debug.Log(version.resource);

                    if (callback != null) {
                        callback();
                    }
                }
            }
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
            Debug.Log("ii:" + ii);
            using (UnityWebRequest request = UnityWebRequest.Get(SERVERPATH + abPaths[ii - 1])) {
                yield return request.SendWebRequest();

                if (request.isDone) {
                    Debug.Log(abPaths[ii - 1]);

                    if (callback != null) {
                        callback((float)ii / (float)abPaths.Length);
                    }

                    ii++;

                    if (ii == abPaths.Length + 1) {
                        yield return null;
                            
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
    
    }
}

