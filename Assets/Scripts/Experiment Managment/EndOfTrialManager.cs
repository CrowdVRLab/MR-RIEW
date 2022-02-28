using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EndOfTrialManager : MonoBehaviour
{
    public UnityEvent OnEndOfTrialEvent;

    public bool invoked = false;


    public void InvokeEndOfTrial() {

        if (invoked)
        {
            return;
        }
        else
        {
            invoked = false;
            OnEndOfTrialEvent.Invoke();
            Destroy(gameObject);
        }
    }
}
