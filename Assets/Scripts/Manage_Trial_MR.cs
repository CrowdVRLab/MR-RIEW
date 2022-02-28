using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Manage_Trial_MR : MonoBehaviour
{

    private bool isStarted = false;
    private bool isRepeated = false;
    
    private float currentTime = 0;
    public float trialTime = 10f;
    public UnityEvent OnEndOfTrialEvent;

    public GameObject[] listui;

    private GameObject rig;
    private OVRPassthroughLayer ovrptl;

    int i = 0;

    public void toggleUI()
    {
        if(i== listui.Length - 1 && listui[i].activeSelf==true)
        {
            return;
        }
        listui[i].SetActive(!listui[i].activeSelf);
        if (!listui[i].activeSelf) i += 1;
    }

    void AddMR() {
        //replace here the type of MR
        ovrptl = rig.GetComponent<OVRPassthroughLayer>();
        ovrptl.overlayType = OVROverlay.OverlayType.Underlay;
        ovrptl.hidden = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        TransitionManager.Instance.setExperimentView();
        rig = GameObject.Find("OVRCameraRig");

        AddMR();
    }

    public void OnStartTrial()
    {
        isStarted = true;
    }

    public void OnRepeatTrial()
    {
        isRepeated = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStarted == false) return;

        if(currentTime > trialTime)
        {
            // event to launch at the end of a my code to trigger next dialog
            toggleUI();

            RemoveMR();
        }

        currentTime += Time.deltaTime;
    }

    void RemoveMR()
    {
        ovrptl.hidden = true;
        //ovrptl.overlayType = OVROverlay.OverlayType.None;
    }
}
