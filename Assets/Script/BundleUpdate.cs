using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.IO;


namespace Game {

    public class BundleUpdate : MonoBehaviour {

        private static readonly string VERSION_FILE = "resource_version.json";
        private string LOCAL_RES_PATH = ""; 

        private string SERVER_RES_PATH =  "http://192.168.8.105";
        // private string SERVER_RES_URL = "";
        // private string LOCAL_RES_URL = "";



        //private JsonData jdLocalFile;
        //private JsonData jdServerFile;

        private Dictionary<string, string> LocalBundleVersion;  // 本地资源名和路径字典
        private Dictionary<string, string> ServerBundleVersion; // 服务器资源名和路径字典

        private List<string> NeedDownFiles; // 需要下载的文件List

        private bool NeedUpdateLocalVersionFile = false;    // 是否需要更新本地版本文件

        //private Action<WWW> DownLoadFinish; // 下载完成委托

        private int totalUpdateFileCount = 0;   //

        void Start() {
            LOCAL_RES_PATH = Application.persistentDataPath + "/";
            //LocalBundleVersion = new Dictionary<string, string>();
            //ServerBundleVersion = new Dictionary<string, string>();
            //NeedDownFiles = new List<string>();

            //// 加载本地Version配置
            //string tempLocalVersion = "";

            //if (!File.Exists(LOCAL_RES_PATH + "/" + VERSION_FILE)) {

            //}

            DownLoadRes();
        }

        private void DownLoadRes() {
            //StartCoroutine(DownLoad(SERVER_RES_PATH + "/" + VERSION_FILE, (www) => {
            //    Debug.Log(www.url);
            //    Debug.Log(www.text);

            //    File.WriteAllText(Application.persistentDataPath + "/" + VERSION_FILE, www.text);


            //    //JsonMapper.ToObject(www.t)

            //    //File.WriteAllBytes(Application.persistentDataPath + "/" + VERSION_FILE, www.bytes);
            //}));
        }



        //private IEnumerator DownLoad(string url, Action<WWW> callback) {
        //    using (WWW www = new WWW(url)) {
        //        yield return www;
                
        //        if (callback != null) {
        //            callback(www);
        //        }
        //    }
        //    //    WWW www = new WWW(url);
        //    //yield return www;

        //    //if (callback != null) {
        //    //    callback(www);
        //    //}
        //}




        //// flag: 0 表示本地版本文件, 1 表示服务器版本文件
        //private void ParseVersionFile(string content, Dictionary<string, string> dict, int flag) {
        //    if (content == null || content.Length == 0) {
        //        return;
        //    }

        //    JsonData jd = null;

        //    try {
        //        jd = JsonMapper.ToObject(content);
        //    } catch (Exception ex) {
        //        Debug.LogError(ex.Message);
        //        return;
        //    }

        //    if (flag == 0) {    // 本地
        //        jdLocalFile = jd;
        //    } else if (flag == 1) { // 服务器
        //        jdServerFile = jd;
        //    } else {
        //        return;
        //    }

        //    JsonData resObj = null;

        //    resObj = jd["resource"];

        //    resObj.Add()
        //}

    }
}


