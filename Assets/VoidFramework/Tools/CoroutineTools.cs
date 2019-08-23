using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoidFramework {

    public class CoroutineTools : MonoBehaviour {

        private static CoroutineTools instance;

        public static CoroutineTools Instance {
            get {
                return instance;
            }
        }

        private void Awake() {
            instance = this;
        }


        public Coroutine StartCoroutineCustom(IEnumerator coroutine) {
            return StartCoroutine(coroutine);
        }

        public void StopCoroutineCustom(Coroutine coroutine) {
            StopCoroutine(coroutine);
        }

        public void StopAllCoroutinesCustom() {
            StopAllCoroutines();
        }
    }
}


