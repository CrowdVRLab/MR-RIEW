using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Questionaire/UiPrefab")]
public class QuestionaireUI : ScriptableObject
{


    [SerializeField]
    public GameObject Container;

    [SerializeField]
    public GameObject Dropdown;

    [SerializeField]
    public GameObject Slider;

    [SerializeField]
    public GameObject PanelTitle;

    [SerializeField]
    public GameObject Title;

    [SerializeField]
    public GameObject Text;

    [SerializeField]
    public GameObject Radio;

    [SerializeField]
    public GameObject Button;

    [SerializeField]
    public GameObject Notifications;
}

