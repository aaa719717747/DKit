using System.Collections;
using System.Collections.Generic;
using DKit.ThirdPartyPlugins.DKitObjectPool.Runtime.Core;
using UnityEngine;

public class DKitPoolTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ObjectPool.Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DKitObject a= ObjectPool.Get("t1");
            Debug.Log(a.name);
        }
    }
}
