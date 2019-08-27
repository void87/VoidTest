using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.IO;
using System;
using UnityEngine.Networking;

namespace VoidFramework {

    public class AssetBuilder : Editor {

        private class VersionData {
            public string Version;
            public List<SingleResouce> Resouces;
        }

        private class SingleResouce {
            public string Name;
            public string Hash;
        }

        public static string sourcePath = Application.dataPath + "/ABRes";
        private const string ASSETBUNDLESOUTPUTPATH = "Assets/StreamingAssets";

        //private static string m_assetPath = Application.streamingAssetsPath;
        //private static string assetTail = ".unity3d";
        //private static bool canFinish = false;

        //private static List<string> bundleRelativePaths = new List<string>();


        /// <summary>
        /// 设置AB包名
        /// </summary>
        [MenuItem("Tools/AssetBundle/SetABName")]
        public static void SetBundleNames() {
            HandleDirectory(sourcePath);
            Debug.Log("AB包名设置完成");
        }


        private static void HandleDirectory(string directoryPath) {

            DirectoryInfo di = new DirectoryInfo(directoryPath);
            FileSystemInfo[] fsis = di.GetFileSystemInfos();

            foreach (var fsi in fsis) {
                if (fsi is DirectoryInfo) {
                    HandleDirectory(fsi.FullName);
                } else {
                    if (!fsi.FullName.EndsWith(".meta") && !fsi.FullName.EndsWith(".keep")) {
                        HandleFile(fsi.FullName);
                    }
                }
            }
        }

        private static void HandleFile(string filePath) {
            filePath = filePath.Replace("\\", "/");
            filePath = "Assets/" + filePath.Substring(Application.dataPath.Length + 1);

            var assetName = filePath.Split('/')[2];

            AssetImporter assetImporter = AssetImporter.GetAtPath(filePath);
            assetImporter.assetBundleName = assetName;
        }

        /// <summary>
        /// 清除AB包名
        /// </summary>
        [MenuItem("Tools/AssetBundle/ClearABName")]
        public static void ClearABName() {
            string[] abNames = AssetDatabase.GetAllAssetBundleNames();

            foreach (var abName in abNames) {
                Debug.Log(abName);
                AssetDatabase.RemoveAssetBundleName(abName, true);
            }

            Debug.Log("AB包名清除完成");
        }

        /// <summary>
        /// 生成包
        /// </summary>
        [MenuItem("Tools/AssetBundle/BuildAB")]
        public static void BuildAssetBundle() {
            string outputPath = ASSETBUNDLESOUTPUTPATH + "/" + Platform.GetPlatformFolder(EditorUserBuildSettings.activeBuildTarget);

            if (!Directory.Exists(outputPath)) {
                Directory.CreateDirectory(outputPath);
            }

            BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

            AssetDatabase.Refresh();

            Debug.Log("打包完成");
        }

        /// <summary>
        /// 删除包
        /// </summary>
        [MenuItem("Tools/AssetBundle/DeleteBundles")]
        public static void DeleteAssetBundle() {
            string outputPath = ASSETBUNDLESOUTPUTPATH + "/" + Platform.GetPlatformFolder(EditorUserBuildSettings.activeBuildTarget);

            if (Directory.Exists(outputPath)) {
                Directory.Delete(outputPath, true);
            }

            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 生成资源版本文件
        /// </summary>
        [MenuItem("Tools/AssetBundle/GenerateResourceVersionFile")]
        public static void GenerateVersionFile() {
            LoadAssetBundleManifest();

            Debug.Log("成功生成资源版本文件");
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 加载Manifest
        /// </summary>
        private static void LoadAssetBundleManifest() {
            string manifestName = GetRuntimePlatform();
            manifestName += "/" + manifestName; // eg Windows/Windows

            EditorCoroutineRunner.StartEditorCoroutine(LoadResCoroutine(Application.streamingAssetsPath + "/" + manifestName, (www) => {
                AssetBundle assetBundle = www.assetBundle;
                AssetBundleManifest manifest = assetBundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
                assetBundle.Unload(false);

                var jo = new JObject();
                jo.Add("Version", "1.0");

                string[] bundleNames = AssetDatabase.GetAllAssetBundleNames();

                List<SingleResouce> singleHashList = new List<SingleResouce>();

                foreach (var item in bundleNames) {
                    Hash128 hash = manifest.GetAssetBundleHash(item);

                    var singleHash = new SingleResouce();
                    singleHash.Name = item;
                    singleHash.Hash = hash.ToString();

                    singleHashList.Add(singleHash);
                }

                jo.Add("Resouces", JToken.FromObject(singleHashList));

                try {
                    File.WriteAllText(ASSETBUNDLESOUTPUTPATH + "/" + GetRuntimePlatform() + "/resource_version.json", jo.ToString());
                } catch (Exception error) {
                    Debug.Log("Write Cfg file error: " + error.Message);
                }

                AssetDatabase.Refresh();
            }));
        }

        private static IEnumerator LoadResCoroutine(string path, Action<WWW> callback) {
            WWW www = new WWW(path);
            yield return www;
            if (callback != null) {
                callback(www);
            }
        }

        /// <summary>
        /// 获取平台对应文件夹
        /// </summary>
        private static string GetRuntimePlatform() {
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

            Debug.Log(platform);

            return platform;
        }

    }

    public class Platform {
        public static string GetPlatformFolder(BuildTarget target) {
            switch (target) {
                case BuildTarget.Android:
                    return "android";
                case BuildTarget.iOS:
                    return "ios";
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return "windows";
                case BuildTarget.StandaloneOSX:
                case BuildTarget.StandaloneOSXIntel:
                case BuildTarget.StandaloneOSXIntel64:
                    return "osx";
            }

            return null;
        }
    }
}


