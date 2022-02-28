using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(menuName = "Experiment/NewExperiment")]
public class ExperimentScriptableObject : ScriptableObject
{
    [SerializeField]
    public Session[] Session;
}


[Serializable]
public class Session {

    public string name;

    public bool randomizeTrialsOrder;

    public Trial[] trials;

}

[Serializable]
public class Trial
{

    public string name;

    public string sceneName;

    public string questionaireRefName;


}