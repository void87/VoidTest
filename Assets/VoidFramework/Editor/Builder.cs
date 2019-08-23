using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LitJson;
using System.Text;
using System.IO;
using System;
using UnityEngine.Networking;

namespace VoidFramework {

    public class Builder : Editor {

        public static string sourcePath = Application.dataPath + "/ABRes";
        private const string ASSETBUNDLESOUTPUTPATH = "Assets/StreamingAssets";

        private static string m_assetPath = Application.streamingAssetsPath;
        private static string assetTail = ".unity3d";
        private static bool canFinish = false;

        private static List<string> bundleRelativePaths = new List<string>();


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

        ///// <summary>
        ///// 生成资源版本文件
        ///// </summary>
        //[MenuItem("Tools/AssetBundle/Generate Resource Version File")]
        //public static void GenerateVersionFile() {
        //    GenerateCfgFile();

        //    EditorCoroutineRunner.StartEditorCoroutine(Wait());
        //    Debug.Log("成功生成资源版本文件");
        //    AssetDatabase.Refresh();
        //}

        private static IEnumerator Wait() {
            Debug.Log("wait");

            while (!canFinish) {
                yield return null;
            }
        }

        /// <summary>
        /// 删除包
        /// </summary>
        [MenuItem("Tools/AssetBundle/Delete Bundles")]
        public static void DeleteAssetBundle() {
            string outputPath = ASSETBUNDLESOUTPUTPATH + "/" + Platform.GetPlatformFolder(EditorUserBuildSettings.activeBuildTarget);

            if (Directory.Exists(outputPath)) {
                Directory.Delete(outputPath, true);
            }

            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 打包
        /// </summary>
        private static void Pack(string source) {
            DirectoryInfo folder = new DirectoryInfo(source);

            FileSystemInfo[] files = folder.GetFileSystemInfos();

            Debug.Log(source);

            foreach (var item in files) {
                if (item is DirectoryInfo) {
                    Pack(item.FullName);
                } else {
                    if (!item.Name.EndsWith(".meta")) {
                        HandleFile(item.FullName);
                    }
                }
            }
        }



        /// <summary>
        /// 将 \ 转换为 /
        /// </summary>
        private static string Replace(string s) {
            return s.Replace("\\", "/");
        }

        /// <summary>
        /// 获取文件的依赖
        /// </summary>
        public static void FileDependency(string path) {         
            try {
                Debug.Log("path is " + path);

                string[] dependencies = AssetDatabase.GetDependencies(path);

                int counter = 0;

                foreach (var item in dependencies) {
                    if (item.Equals(path) || item.EndsWith(".cs")) {
                        continue;
                    }
                    counter++;
                }

                if (counter == 0) {
                    return;
                }

                for (int i = 0; i < dependencies.Length; i++) {
                    Debug.Log("dependencies" + i + " is " + dependencies[i]);

                    if (dependencies[i].Equals(path) || dependencies[i].EndsWith(".cs")) {
                        continue;
                    }

                    AssetImporter assetImporter = AssetImporter.GetAtPath(dependencies[i]);
                    string assetName = dependencies[i] + ".unity3d";
                    assetName = assetName.Substring(assetName.IndexOf("/") + 1);
                    assetImporter.assetBundleName = assetName;

                    Debug.Log("assetName is " + assetName);

                    if (!bundleRelativePaths.Contains(assetName)) {
                        bundleRelativePaths.Add(assetName);
                    }

                    FileDependency(dependencies[i]);

                }

                
            } catch (Exception error) {
                Debug.Log("error is " + error);
            }
        }

        private static void GenerateCfgFile() {
            Debug.Log("GenerateCfgFile");
            GetManifest();
        }

        /// <summary>
        ///  获取Manifest
        /// </summary>
        private static void GetManifest() {
            LoadAssetBundleManifest((manifest) => {
                WriteCfgFile(manifest);
            });
        }

        /// <summary>
        /// 加载Manifest
        /// </summary>
        /// <param name="action"></param>
        private static void LoadAssetBundleManifest(Action<AssetBundleManifest> action) {
            string manifestName = GetRuntimePlatform();
            Debug.Log("Application.platform is " + Application.platform);
            manifestName += "/" + manifestName; // eg Windows/Windows
            Debug.Log("manifestName is " + manifestName);

            LoadResReturnWWW(manifestName, (www) => {
                AssetBundle assetBundle = www.assetBundle;
                UnityEngine.Object obj = assetBundle.LoadAsset("AssetBundleManifest");
                assetBundle.Unload(false);
                AssetBundleManifest manifest = obj as AssetBundleManifest;
                Debug.Log("(www)" + manifest.name);

                if (action != null) {
                    action(manifest);
                }
            });


        }

        public static void LoadResReturnWWW(string name, Action<WWW> callback) {
            string path = "file://" + m_assetPath + "/" + name;
            Debug.Log("m_assetPath is " + name);
            Debug.Log("name is " + name);
            Debug.Log("加载: " + path);
            EditorCoroutineRunner.StartEditorCoroutine(LoadRes(path, callback));
        }

        private static IEnumerator LoadRes(string path, Action<WWW> callback) {
            WWW www = new WWW(path);
            yield return www;
            if (callback != null) {
                callback(www);
            }
        }

        private static void WriteCfgFile(AssetBundleManifest manifest) {
            if (manifest == null) {
                return;
            }

            Debug.Log("hahah");

            StringBuilder sb = new StringBuilder();
            JsonWriter js = new JsonWriter(sb);

            try {
                js.WriteObjectStart();
                js.WritePropertyName("id");
                js.Write(0);
                js.WritePropertyName("version");
                js.Write("1.0");
                string platform = Platform.GetPlatformFolder(EditorUserBuildSettings.activeBuildTarget);
                js.WritePropertyName("manifest");
                js.Write(platform);
                js.WritePropertyName("resource");

                string[] bundleNames = AssetDatabase.GetAllAssetBundleNames();
                js.WriteObjectStart();
                foreach (var item in bundleNames) {
                    Hash128 hash = manifest.GetAssetBundleHash(item);
                    js.WritePropertyName(item);
                    js.Write(hash.ToString());
                }
                js.WriteObjectEnd();
                js.WriteObjectEnd();
            } catch (Exception error) {
                Debug.Log("Write json error: " + error.Message);
            }

            string strVersion = sb.ToString().ToLower();

            try {
                string platform = GetRuntimePlatform();
                System.IO.File.WriteAllText(ASSETBUNDLESOUTPUTPATH + "/" + platform + "/resource_version", strVersion);
            } catch (Exception error) {
                Debug.Log("Write Cfg file error: " + error.Message);
            }

            canFinish = true;
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


