using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game {

    public interface IWindow {
        void Init();
        void Update();
        void Destroy();
        void Show();
        void Hide();
        void PushData(object param);
    }

    public enum ENUM_WindowName {
        Loading,
        Login
    }

    public class UIManager {

        private static UIManager instance;

        public static UIManager Instance {
            get {
                if (instance == null) {
                    instance = new UIManager();
                }
                return instance;
            }
        }

        private Dictionary<ENUM_WindowName, IWindow> windowDict = new Dictionary<ENUM_WindowName, IWindow>();

        public void InitLoading() {
            Window_Loading loading = new Window_Loading();
            loading.Init();

            windowDict.Add(ENUM_WindowName.Loading, loading);
        }

        public void InitOther() {
            Window_Login login = new Window_Login();
            login.Init();

            windowDict.Add(ENUM_WindowName.Login, login);
        }

        public void Show(ENUM_WindowName windowName) {
            windowDict[windowName].Show();
        }

        public void Hide(ENUM_WindowName windowName) {
            windowDict[windowName].Hide();
        }
    }
}


