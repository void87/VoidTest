using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

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

        private static ABManager instance;

        public static ABManager Instance {
            get {
                if (instance == null) {
                    instance = new ABManager();
                }
                return instance;
            }
        }

        // 服务端AB路径
        public static readonly string SERVERABPATH = "http://192.168.8.105/AB";
        // 版本文件名称
        public static readonly string VERSIONFILENAME = "resource_version.json";

        // 本地AB字典
        private Dictionary<string, string> localABDict = new Dictionary<string, string>();
        // 服务端AB字典
        private Dictionary<string, string> serverABDict = new Dictionary<string, string>();
        // 需要下载的AB
        private Dictionary<string, string> needDownloadABDict = new Dictionary<string, string>();
        // 下载的AB索引
        private int downloadedABIndex = 0;

        // loadingAB名称
        private static readonly string LOADINGABNAME = "loading";

        // 需要下载的AB包字典
        private Dictionary<string, string> needLoadABDict = new Dictionary<string, string>();
        // 已加载的AB包字典
        private Dictionary<string, AssetBundle> loadedABDict = new Dictionary<string, AssetBundle>();
        // 加载的AB索引,不包含loading
        private int loadedABIndex = 0;




        private Coroutine coroutine;

        public void Init() {
            Debug.Log("ABManager Init");
        }

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

        /// <summary>
        /// 下载版本文件
        /// </summary>
        public void DownloadVersionFile(Action callback) {
            GameLauncher.Instance.StartCoroutine(DownloadVersionFileCorountine(SERVERABPATH + "/" + GetRuntimePlatform() + "/" + VERSIONFILENAME, callback));
        }

        private IEnumerator DownloadVersionFileCorountine(string versionPath, Action callback) {
            using (UnityWebRequest request = UnityWebRequest.Get(versionPath)) {
                yield return request.SendWebRequest();

                if (request.isDone) {

                    var serverVersionData = JsonConvert.DeserializeObject<VersionData>(request.downloadHandler.text);

                    Debug.Log("版本文件下载完成");
                    Debug.Log(serverVersionData.Version);

                    // 创建平台文件夹
                    if (!Directory.Exists(Application.persistentDataPath + "/" + GetRuntimePlatform())) {
                        Directory.CreateDirectory(Application.persistentDataPath + "/" + GetRuntimePlatform());
                    }

                    // 本地没有版本文件,下载版本文件, 下载所有AB
                    if (!File.Exists(Application.persistentDataPath + "/" + GetRuntimePlatform() + "/" + VERSIONFILENAME)) {
                        Debug.Log("没有版本文件");
                        // 写入版本文件
                        File.WriteAllBytes(Application.persistentDataPath + "/" + GetRuntimePlatform() + "/" + VERSIONFILENAME, request.downloadHandler.data);
                        // 加入下载字典
                        foreach (var item in serverVersionData.Resouces) {
                            localABDict.Add(item.Name, item.Hash);

                            needDownloadABDict.Add(item.Name, item.Hash);
                        }

                    // 本地有版本文件,对比更新
                    } else {
                        Debug.Log("有版本文件");

                        var localVersionData = JsonConvert.DeserializeObject<VersionData>(File.ReadAllText(Application.persistentDataPath + "/" + GetRuntimePlatform() + "/" + VERSIONFILENAME));

                        // 写入版本文件
                        File.WriteAllBytes(Application.persistentDataPath + "/" + GetRuntimePlatform() + "/" + VERSIONFILENAME, request.downloadHandler.data);

                        // 加入服务端AB字典
                        foreach (var item in serverVersionData.Resouces) {
                            serverABDict.Add(item.Name, item.Hash);
                        }

                        // 加入本地AB字典
                        foreach (var item in localVersionData.Resouces) {
                            localABDict.Add(item.Name, item.Hash);
                        }

                        foreach (var item in serverABDict) {
                            var hash = string.Empty;

                            // 本地有AB
                            if (localABDict.TryGetValue(item.Key, out hash)) {
                                // hash不同,使用服务端hash
                                if (item.Value != hash) {
                                    needDownloadABDict.Add(item.Key, item.Value);
                                }

                            // 本地没有AB
                            } else {
                                needDownloadABDict.Add(item.Key, item.Value);
                            }
                        }

                        localABDict.Clear();
                        // 重新设置本地AB字典为最新
                        foreach (var item in serverVersionData.Resouces) {
                            localABDict.Add(item.Name, item.Hash);
                        }
                    }

                    // 需要下载新的ab包
                    if (needDownloadABDict.Count > 0) {
                        // 有最新的LoadingAB包需要下载
                        if (needDownloadABDict.Keys.Contains(LOADINGABNAME)) {
                            GameLauncher.Instance.StartCoroutine(DownloadLoadingABCoroutine(callback));
                        // 不需要下载LoadingAB包
                        } else {
                            AssetBundle ab = AssetBundle.LoadFromFile(Application.persistentDataPath + "/" + GetRuntimePlatform() + "/" + LOADINGABNAME);
                            loadedABDict.Add(LOADINGABNAME, ab);

                            if (callback != null) {
                                callback();
                            }
                        }
                    // 不需要下载新的AB包
                    } else {
                        AssetBundle ab = AssetBundle.LoadFromFile(Application.persistentDataPath + "/" + GetRuntimePlatform() + "/" + LOADINGABNAME);
                        loadedABDict.Add(LOADINGABNAME, ab);

                        if (callback != null) {
                            callback();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 下载LoadingAB
        /// </summary>
        private IEnumerator DownloadLoadingABCoroutine(Action downloadCallback) {
            using (UnityWebRequest request = UnityWebRequest.Get(SERVERABPATH + "/" + GetRuntimePlatform() + "/" + LOADINGABNAME)) {
                yield return request.SendWebRequest();

                if (request.isDone) {
                    File.WriteAllBytes(Application.persistentDataPath + "/" + GetRuntimePlatform() + "/" + LOADINGABNAME, request.downloadHandler.data);

                    needDownloadABDict.Remove(LOADINGABNAME);

                    AssetBundle ab = AssetBundle.LoadFromFile(Application.persistentDataPath + "/" + GetRuntimePlatform() + "/" + LOADINGABNAME);
                    loadedABDict.Add(LOADINGABNAME, ab);

                    if (downloadCallback != null) {
                        downloadCallback();
                    }
                }
            }
        }

        /// <summary>
        /// 下载需要更新的AB包
        /// </summary>
        public void DownloadABs(Action<float> downloadCallback, Action<float> loadCallback, Action callback) {
            if (needDownloadABDict.Count > 0) {
                GameLauncher.Instance.StartCoroutine(DownloadAB(downloadCallback, loadCallback, callback));
            } else {
                LoadABs(loadCallback, callback);
            }
        }

        private IEnumerator DownloadAB(Action<float> downloadCallback, Action<float> loadCallback, Action callback) {
            using (UnityWebRequest request = UnityWebRequest.Get(SERVERABPATH + "/" + GetRuntimePlatform() + "/" + needDownloadABDict.Keys.ToList()[downloadedABIndex])) {
                yield return request.SendWebRequest();

                if (request.isDone) {
                    File.WriteAllBytes(Application.persistentDataPath + "/" + GetRuntimePlatform() + "/" + needDownloadABDict.Keys.ToList()[downloadedABIndex] , request.downloadHandler.data);
                    
                    if (downloadCallback != null) {
                        downloadCallback((float)(downloadedABIndex + 1) / (float)(localABDict.Count - 1));
                    }

                    downloadedABIndex++;

                    if (downloadedABIndex == needDownloadABDict.Count ) {
                        LoadABs(loadCallback, callback);

                    } else {
                        GameLauncher.Instance.StartCoroutine(DownloadAB(downloadCallback, loadCallback, callback));
                    }
                }
            }
        }

        /// <summary>
        /// 加载AB包
        /// </summary>
        public void LoadABs(Action<float> loadCallback, Action callback) {
            foreach (var item in localABDict) {
                if (item.Key != LOADINGABNAME) {
                    needLoadABDict.Add(item.Key, item.Value);
                }
            }

            GameLauncher.Instance.StartCoroutine(LoadABCoroutine(loadCallback, callback));
        }

        private IEnumerator LoadABCoroutine(Action<float> loadCallback, Action callback) {
            yield return new WaitForEndOfFrame();

            AssetBundle ab = AssetBundle.LoadFromFile(Application.persistentDataPath + "/" + GetRuntimePlatform() + "/" + needLoadABDict.ToList()[loadedABIndex].Key);
            loadedABDict.Add(needLoadABDict.ToList()[loadedABIndex].Key, ab);

            if (loadCallback != null) {
                loadCallback((float)(loadedABIndex + 1) / (float)(needLoadABDict.Count - 1));
            }

            loadedABIndex++;

            if (loadedABIndex == needLoadABDict.Count) {
                if (callback != null) {
                    callback();
                }

                yield break;
            } else {
                GameLauncher.Instance.StartCoroutine(LoadABCoroutine(loadCallback, callback));
            }

        }

        public T LoadAsset<T>(string abName, string assetName) where T: UnityEngine.Object {
            Debug.Log(loadedABDict[abName]);

            return loadedABDict[abName].LoadAsset<T>(assetName) as T;
        }
    
    }
}

