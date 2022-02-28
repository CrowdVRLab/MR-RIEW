using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInitialPosition : MonoBehaviour
{

    Transform target;
    Transform forward;
    Transform floor;
    Transform camera;
    List<GameObject> uilist = new List<GameObject>();
    Bounds bounds;
    OVRPlayerController ovrpc;
    CharacterController cc;
    public bool set;

    // Start is called before the first frame update
    void Start()
    {
        
        target = GameObject.Find("OVRPlayerController").transform;
        forward = target.FindDeepChild("ForwardDirection").transform;
        camera = target.FindDeepChild("CenterEyeAnchor").transform;
        floor = GameObject.Find("Ground").transform;      
        ovrpc = target.GetComponent<OVRPlayerController>();
        cc = target.GetComponent<CharacterController>();

        ovrpc.GravityModifier = 0;

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        bounds = new Bounds(transform.position, Vector3.zero);
        foreach (Renderer r in renderers)
        {
            bounds.Encapsulate(r.bounds);
        }

        SetPlayerInitialPosition();
    }

    public void SetPlayerInitialPosition()
    {

        set = true;

    }

    IEnumerator SetUiInitialPosition()
    {

        yield return new WaitForSeconds(0.1f);

        getui();

        if (uilist.Count > 0)
        {
            foreach (GameObject ui in uilist)
            {

                ui.transform.position = new Vector3(bounds.center.x, camera.position.y, bounds.center.z);
                ui.transform.rotation = Quaternion.identity;
                ui.transform.Translate(ui.transform.forward * 2);
            }

        }
        else
        {
            SetUiInitialPosition();
        }

        yield return new WaitForSeconds(0.5f);
        if (TransitionManager.Instance && TransitionManager.Instance.isWaiting)
        {
            TransitionManager.Instance.setExperimentView();
        }

    }

    void getui()
    {


        if (GameObject.Find("EndOfTrialManager") != null) uilist.Add(GameObject.Find("EndOfTrialManager"));

        if (GameObject.Find("Questionaire Panel") != null) uilist.Add(GameObject.Find("Questionaire Panel"));

        if (GameObject.Find("Counter") != null) uilist.Add(GameObject.Find("Counter"));


        GameObject container = GameObject.Find("UiContainer");

        if (container != null)
        {
            EndOfTrialManager[] eList = container.GetComponentsInChildren<EndOfTrialManager>(true);

            foreach (EndOfTrialManager e in eList)
            {
                uilist.Add(e.gameObject);
            }

        }

    }


    void FixedUpdate()
    {
        if (set) {

            target.position = new Vector3(bounds.center.x, floor.position.y + 2.0f, bounds.center.z);
            target.rotation = Quaternion.identity;

            StartCoroutine(SetUiInitialPosition());

            set = false;
        }

        if (target.position.y < 0)
        {
            ovrpc.GravityModifier = 0;
          
            SetPlayerInitialPosition();

        }
        else {

            ovrpc.GravityModifier = 1;
            
        }
    }

    

}
