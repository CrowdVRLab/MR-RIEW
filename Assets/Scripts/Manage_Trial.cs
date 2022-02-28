using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Manage_Trial : MonoBehaviour
{

    public TextMeshPro counter;
    private bool isStarted = false;
    private bool isRepeated = false;
    
    private float currentTime = 0;
    public float trialTime = 10f;
    public UnityEvent OnEndOfTrialEvent;

    public GameObject[] listui;

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

    // Start is called before the first frame update
    void Start()
    {
        TransitionManager.Instance.setExperimentView();
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

        if (currentTime > trialTime)
        {
            // event to launch at the end of a my code to trigger next dialog
            toggleUI();

            if (counter != null) counter.enabled = false;
        }
        else
        {

            currentTime += Time.deltaTime;

            var timeLeft = trialTime - currentTime;

            if (counter != null) counter.text = timeLeft.ToString();
        }

        
    }
}
