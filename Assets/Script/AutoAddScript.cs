using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ILRuntime.Runtime.Enviorment;
using UnityEngine.Networking;
using VoidFramework;
using System.IO;

public class AutoAddScript : MonoBehaviour {

    private AppDomain appDomain;

    private void Awake() {
        appDomain = new AppDomain();
        //WWW www = new WWW();
        

        //StartCoroutine(DownloadDLL());
    }

    //private IEnumerator DownloadDLL() {
    //    using (UnityWebRequest request = UnityWebRequest.Get(ABManager.SERVERABPATH  + "/" + "TestHotFix.dll")) {
    //        yield return request.SendWebRequest();

    //        if (request.isDone) {
    //            File.WriteAllBytes(Application.persistentDataPath + "/" + "TestHotFix.dll", request.downloadHandler.data);

    //            MemoryStream ms = new MemoryStream(request.downloadHandler.data);
    //            appDomain.LoadAssembly(ms, null, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());

    //            InitializeILRuntime();
    //            OnILRuntimeInitialized();
    //        }
    //    }
    //}

    private void InitializeILRuntime() {

    }

    private unsafe void OnILRuntimeInitialized() {
        appDomain.Invoke("TestHotFix.Class1", "Test1", null, null);
    }
}
