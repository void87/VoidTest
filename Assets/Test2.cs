using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        if (name != "Cube2") {
            Vector3 v3 = transform.localEulerAngles;
            v3.x = 30;

            transform.localEulerAngles = v3;


            Vector3 v4 = transform.localEulerAngles;
            v4.y = 40;

            transform.localEulerAngles = v4;


            //transform.rota

        } else {

            Debug.Log("haha");

            Vector3 v3 = transform.localEulerAngles;
            v3.y = 40;

            transform.localEulerAngles = v3;


            Vector3 v4 = transform.localEulerAngles;
            v4.x = 30;

            transform.localEulerAngles = v4;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
