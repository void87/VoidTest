using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game {

    public interface IWindow {
        void Init();
        void Update();
        void Destroy();
    }

    public enum ENUM_WindowName {

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

        //private Dictionary<>

        public void Init() {
            Window_Loading loading = new Window_Loading();

            loading.Init();
        }
    }
}


