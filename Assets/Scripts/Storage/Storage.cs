using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public abstract class Storage : MonoBehaviour
{
    public abstract void SaveQuestionaire(DataToCollect questionaire);
}
