using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoidFramework;

namespace Game {
    public class Window_Loading : IWindow {

        private RectTransform rectTransform;

        private Text txt_Loading;
        private Slider slider_Progress;

        public void Init() {
            rectTransform = GameObject.Instantiate(ResManager.Instance.Load<GameObject>("loading", "ABRes/Loading/Window_Loading.prefab")).transform as RectTransform;
            GameLauncher.AddToUI(rectTransform);

            txt_Loading = rectTransform.Find("txt_Loading").GetComponent<Text>();
            txt_Loading.text = "下载AB: 0";
            slider_Progress = rectTransform.Find("slider_Progress").GetComponent<Slider>();

            //ABManager.Instance.DownloadABs(DownloadAB, LoadAB, DownloadAndLoadComplete);

            //GameLauncher.Instance.StartCoroutine(ABManager.Instance.DownloadVersionFileCorountine(ABManager.Instance.VERSIONPATH, DownloadVersionFileComplete));
        }

        public void Update() {

        }

        public void Destroy() {

        }

        public void Show() {
            rectTransform.gameObject.SetActive(true);
        }

        public void Hide() {
            rectTransform.gameObject.SetActive(false);
        }

        public void PushData(object param) {

        }

        private void DownloadAB(float progress) {
            slider_Progress.value = progress;
            txt_Loading.text = "下载AB: " + progress;
        }

        private void LoadAB(float progress) {
            slider_Progress.value = progress;
            txt_Loading.text = "加载AB: " + progress;
        }

        private void DownloadAndLoadComplete() {
            Debug.Log("complete");
            UIManager.Instance.InitOther();

            UIManager.Instance.Hide(ENUM_WindowName.Loading);
            UIManager.Instance.Show(ENUM_WindowName.Login);
        }

        private void DownloadVersionFileComplete() {
            txt_Loading.text = "版本文件下载完成";

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
    }
}


