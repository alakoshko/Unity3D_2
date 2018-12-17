using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GraphicsOptions : MonoBehaviour {

	// Use this for initialization
	void Start () {

        Debug.Log(Application.platform);

#if UNITY_ANDROID || UNITY_IOS
        Application.targetFrameRate = 30;
#elif UNITY_EDITOR
        Application.targetFrameRate = 60;
#endif
    }
	
}
