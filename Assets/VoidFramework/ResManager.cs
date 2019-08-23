using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class ResManager {

    private static ResManager instance;

    public static ResManager Instance {
        get {
            if (instance == null) {
                instance = new ResManager();
            }
            return instance;
        }
    }

    public T Load<T>(string assetPath) where T: Object {
#if UNITY_EDITOR
        return AssetDatabase.LoadAssetAtPath<Object>("Assets/" + assetPath) as T;
#elif UNITY_IOS
        return null;
#else
        return null;
#endif


        //if (Application.platform == RuntimePlatform.WindowsEditor) {
        //    return AssetDatabase.LoadAssetAtPath<Object>("Assets/" + assetPath) as T;
        //} else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) {
        //    return AssetBundle.LoadFromFile("").LoadAsset<T>("");
        //}

        //return null;

//#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        
//#elif UNITY_IOS || UNITY_ANDROID
//        return null;
//#endif
    }


    

}
