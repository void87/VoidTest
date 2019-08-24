using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoidFramework;

namespace Game {
    public class Window_Login : IWindow {

        private RectTransform rectTransform;

        private Button btn_Login;

        public void Init() {
            rectTransform = GameObject.Instantiate(ResManager.Instance.Load<GameObject>("prefabs", "ABRes/Prefabs/UI/Loading/Window_Login.prefab")).transform as RectTransform;
            GameLauncher.AddToUI(rectTransform);

            btn_Login = rectTransform.Find("btn_Login").GetComponent<Button>();



            //GameLauncher.Instance.StartCoroutine(ABManager.Instance.DownloadVersionFile(ABManager.Instance.VERSIONPATH, DownloadVersionFileComplete));
        }

        public void Update() {

        }

        public void Destroy() {

        }

        //private void DownloadVersionFileComplete() {
        //    txt_Loading.text = "版本文件下载完成";

        //    ABManager.Instance.DownloadABs((progress) => {
        //        txt_Loading.text = "下载AB包";

        //        Debug.Log(progress);

        //        txt_Loading.text = progress.ToString();
        //        slider_Progress.value = progress;

        //        if (progress == 1) {
        //            txt_Loading.text = "加载AB包";

        //            ABManager.Instance.LoadABs((loadProgress) => {
        //                Debug.Log(loadProgress);

        //                if (loadProgress == 1) {
        //                    txt_Loading.text = "AB包加载完成";
        //                }
        //            });
        //        }
        //    });

        //}
    }
}


