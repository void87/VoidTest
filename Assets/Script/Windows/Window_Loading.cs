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
            rectTransform = GameObject.Instantiate(ResManager.Instance.Load<GameObject>("ABRes/Prefabs/UI/Loading/Window_Loading.prefab")).transform as RectTransform;
            GameLauncher.AddToUI(rectTransform);

            txt_Loading = rectTransform.Find("txt_Loading").GetComponent<Text>();
            slider_Progress = rectTransform.Find("slider_Progress").GetComponent<Slider>();

            GameLauncher.Instance.StartCoroutine(ABManager.Instance.DownloadVersionFile(ABManager.Instance.VERSIONPATH, DownloadVersionFileComplete));
        }

        public void Update() {

        }

        public void Destroy() {

        }

        private void DownloadVersionFileComplete() {
            txt_Loading.text = "版本文件下载完成";

            ABManager.Instance.DownloadABs((progress) => {
                Debug.Log(progress);

                txt_Loading.text = progress.ToString();
                slider_Progress.value = progress;
            });

        }
    }
}


