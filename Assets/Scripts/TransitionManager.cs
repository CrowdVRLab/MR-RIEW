using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    private static TransitionManager instance = null;
    private Camera camera;
    public GameObject logo;
    private CameraClearFlags previous;

    private bool _isWaiting = false;
    public bool isWaiting
    {
        get { return _isWaiting; }
    }

    public static TransitionManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);


        camera = GameObject.Find("CenterEyeAnchor").GetComponent<Camera>();
    }

    public void setWaitingView()
    {
        _isWaiting = true;
        //camera.backgroundColor = Color.black;
        camera.cullingMask = 1 << LayerMask.NameToLayer("logo");
        //previous = camera.clearFlags;
        //camera.clearFlags = CameraClearFlags.SolidColor;
    }

    public void setExperimentView()
    {
        camera.cullingMask = ~(1 << LayerMask.NameToLayer("logo"));
        //camera.clearFlags = CameraClearFlags.Skybox;
        //camera.clearFlags = previous;
        _isWaiting = false;
         logo.SetActive(false);
    }

    public void setLogoLocation(float upOffset = 0.0f) {

        GameObject cameraobj = GameObject.Find("CenterEyeAnchor");
     
        if (logo != null)
        {
            logo.SetActive(true);
            logo.transform.position = cameraobj.transform.position;
            logo.transform.rotation = cameraobj.transform.rotation;
            logo.transform.Translate(logo.transform.forward * 1.5f);
            logo.transform.Translate(logo.transform.up * upOffset);
        }
       
    }
    
}
