using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

using UnityEngine.UI;


using UnityEngine.Events;

using TMPro;
public class UIBuilder : MonoBehaviour
{

    public GameObject Parent;

    public float scale;

    public Vector3 adjust;

    public ScriptableObject dataToCollect;

    public ScriptableObject useriinterface;

    protected GameObject Panel;

    protected GameObject Container;

    protected QuestionaireUI ui;

    [HideInInspector]
    public DataToCollect data;

    public Storage databaseManager;

    public UnityEvent OnQuestionairePartCompleted;


    void Awake() {

        ui = (QuestionaireUI)useriinterface;
        data = (DataToCollect)dataToCollect;

        data.Init();

    }

    public void EditorBuild(int i) { 

#if UNITY_EDITOR

        ui = (QuestionaireUI)useriinterface;       
        data = (DataToCollect)dataToCollect;

        data.Init();
#endif

        string referenceName = data.questionaire.parts[i].referenceName;

        Build(referenceName);
    } 

    public void Build(string referenceName, int subPartNumber=0)
    {

        int partNumber = GetQuestionairePartIndex(referenceName);

        if (partNumber == -1) {

            Debug.Log("Questionaire with Refrence name -->" + referenceName+" not found can't build interface");
            return;
        }

        if (Panel == null)
        {
            Panel = new GameObject("Questionaire Panel");

            if (Parent != null) {

                Panel.transform.position = Parent.transform.position;
                Panel.transform.rotation = Parent.transform.rotation;
                Panel.transform.localScale = new Vector3(scale, scale, scale);
                Panel.transform.Translate(adjust);
            }

        }


        Container = Instantiate(ui.Container, Panel.transform);
        GameObject PanelTitle = Instantiate(ui.PanelTitle, Container.transform);        
        PanelTitle.GetComponent<TextMeshProUGUI>().text = data.questionaire.parts[partNumber].subparts[subPartNumber].name;

        if (data.questionaire.parts[partNumber].subparts[subPartNumber].description != "") {
            GameObject PanelDescription = Instantiate(ui.Text, Container.transform); 
            PanelDescription.GetComponent<TextMeshProUGUI>().text = data.questionaire.parts[partNumber].subparts[subPartNumber].description;
        }

        for (int questionNumber = 0; questionNumber < data.questionaire.parts[partNumber].subparts[subPartNumber].questions.Length; questionNumber++)
        {

            QuestionaireQuestion question = data.questionaire.parts[partNumber].subparts[subPartNumber].questions[questionNumber];

            GameObject QuestionTitle = Instantiate(ui.Title, Container.transform);
            QuestionTitle.GetComponent<TextMeshProUGUI>().text = question.question;
            GameObject QuestionText = Instantiate(ui.Text, Container.transform);
            QuestionText.GetComponent<TextMeshProUGUI>().text = question.helpText;

            if (question.uielement == UitType.Radio)
            {

                GameObject QuestionUiElement = Instantiate(ui.Radio, Container.transform);
                QuestionUiElement.name = QuestionName(partNumber, subPartNumber, questionNumber);

                Transform ChildOption = QuestionUiElement.transform.GetChild(0);
                int l = QuestionUiElement.transform.childCount;
                for (int i = 1; i < l; i++)
                {
                    Transform QuestionUiElementChild = QuestionUiElement.transform.GetChild(QuestionUiElement.transform.childCount - 1);

#if UNITY_EDITOR
                    DestroyImmediate(QuestionUiElementChild.gameObject);
#else
                    Destroy(QuestionUiElementChild.gameObject);
#endif
                }


                for (int i = 0; i < question.Options.Length; i++)
                {

                    //generate captured variables for lambda functions
                    var newPart = partNumber;
                    var newSubPart = subPartNumber;
                    var newQuestionNumber = questionNumber;
                    var newOptionNumber = i;
                    GameObject Option;

                    if (i == 0) Option = ChildOption.gameObject;
                    else Option = Instantiate(ChildOption.gameObject, QuestionUiElement.transform);

                    Option.GetComponentInChildren<TextMeshProUGUI>().text = question.Options[i];
                     MRRIEWRadio radio = Option.GetComponentInChildren<MRRIEWRadio>();
                    radio.onPointerClick.AddListener(delegate { RadioValueChanged(newPart, newSubPart, newQuestionNumber, newOptionNumber); });

                }

                foreach (MRRIEWRadio radio in QuestionUiElement.GetComponentsInChildren<MRRIEWRadio>())
                {
                    radio.isOn = false;
                }

            }
            else if (question.uielement == UitType.Slider)
            {
                //generate captured variables for lambda functions
                var newPartNumber = partNumber;
                var newSubPartNumber = subPartNumber;
                var newQuestionNumber = questionNumber;

                GameObject QuestionUiElement = Instantiate(ui.Slider, Container.transform);
                QuestionUiElement.name = QuestionName(newPartNumber, newSubPartNumber, newQuestionNumber);
                QuestionUiElement.GetComponentInChildren<MRRIEWSlider>().onValueChanged.AddListener((value) => SliderValueChanged(newPartNumber, newSubPartNumber, newQuestionNumber, value));
            }

        }

        GameObject Button = Instantiate(ui.Button, Container.transform);

        if (subPartNumber == data.questionaire.parts[partNumber].subparts.Length - 1)
        {
            Button.GetComponentInChildren<Button>().onClick.AddListener(delegate { SaveQuestionaire(partNumber, subPartNumber); });
        }
        else {

            Button.GetComponentInChildren<Button>().onClick.AddListener(delegate { MovetoNext(partNumber, subPartNumber); });
        }

#if UNITY_EDITOR

        GameObject DeveloperButton = Instantiate(ui.Button, Container.transform);

        DeveloperButton.GetComponentInChildren<TextMeshProUGUI>().text = "DevForward";

        DeveloperButton.GetComponentInChildren<Button>().onClick.AddListener(delegate {   OnQuestionairePartCompleted.Invoke(); });
#endif      

    }

    string QuestionName(int part, int subPartNumber, int questionNumber) {

        return "P" + part.ToString() + "-SP" +subPartNumber.ToString() +"-QN" +questionNumber.ToString();
    }
    
    int GetQuestionairePartIndex(string referenceName) {


        for (int i=0; i<data.questionaire.parts.Length; i++)
        {

            if (referenceName == data.questionaire.parts[i].referenceName)
            { 
                return i; 
            }

        }

        return -1;
    }

    public void RadioValueChanged(int part, int subPartNumber, int questionNumber, int indexOption)
    {
        string value = data.questionaire.parts[part].subparts[subPartNumber].questions[questionNumber].Options[indexOption];
        Debug.Log("Chaging Questionaire Part " + part.ToString() + " Question->" + questionNumber.ToString()+" Value->"+ value);
        data.questionaire.parts[part].subparts[subPartNumber].questions[questionNumber].answer = value;
        deleteNotifications(part, questionNumber);
    }

    public void SliderValueChanged(int part, int subPartNumber, int questionNumber, float value)
    {
        Debug.Log("Chaging Questionaire Part "+ part.ToString() + " Question->" + questionNumber.ToString() + " Value->" + value.ToString());
        data.questionaire.parts[part].subparts[subPartNumber].questions[questionNumber].answer = value.ToString();
        deleteNotifications(part, questionNumber);
    }

    public void SaveQuestionaire(int part, int subpart)
    {

        if (!triggerNotifactions(part, subpart)) {
            TransitionManager.Instance.setLogoLocation();
            TransitionManager.Instance.setWaitingView();
            StartCoroutine(SaveQuestionaireveCoroutine());
        }
    }

    IEnumerator SaveQuestionaireveCoroutine()
    {
        
        yield return new WaitForSeconds(1.0f);

        databaseManager.SaveQuestionaire(data);
        Destroy();

        OnQuestionairePartCompleted.Invoke();
    }

    public void MovetoNext(int part,int subpart)
    {

        if (!triggerNotifactions(part,subpart))
        {

            DestroyKeepPanel();

            var nextsubpart = subpart + 1;

            Build(data.questionaire.parts[part].referenceName, nextsubpart);
        }
    }

    public bool triggerNotifactions(int part, int subpart) {
       
        List<int> AnansweredAnswers = checkAnansweredAnswers(part,subpart);

        if (AnansweredAnswers.Count == 0) return false;
        else
        {
            GenerateNotification(part,subpart, AnansweredAnswers);
            return true;
        }

    }

    List<int> checkAnansweredAnswers(int part, int subpart)
    {
        List<int> anaswered = new List<int>();

        for (int i=0; i < data.questionaire.parts[part].subparts[subpart].questions.Length;i++)
        {
            QuestionaireQuestion question = data.questionaire.parts[part].subparts[subpart].questions[i];

            if (question.answer == null)
            {
                anaswered.Add(i);
            }
        }

        return anaswered;
    }

    void GenerateNotification(int part, int subpart,  List<int> AnansweredAnswers) {

        deleteNotifications();

        foreach(int index in AnansweredAnswers){

            Transform AnasweredQ = Panel.transform.FindDeepChild(QuestionName(part,subpart,index));
            GameObject allert = Instantiate(ui.Notifications);           
            allert.name = "Notification" + part + index;

            if (Parent != null)
            {
                allert.transform.localScale = new Vector3(scale, scale, scale);
                allert.transform.localPosition = AnasweredQ.position;
                allert.transform.rotation = AnasweredQ.rotation;
                allert.transform.Translate(new Vector3(2.50f*scale, 0f, 0f), Space.Self);
            }
            else { 
                allert.transform.position = AnasweredQ.position;    
                allert.transform.Translate(new Vector3(2.50f, 0f,0f));                 
            }
        }
    }

    void deleteNotifications() {

        GameObject[] Notifications = GameObject.FindGameObjectsWithTag("Notification");

        foreach (GameObject n in Notifications)  Destroy(n);
        
    }

    void deleteNotifications(int part, int questionNumber)
    {

        GameObject Notification = GameObject.Find("Notification" + part + questionNumber);

        Destroy(Notification);

    }

    void Destroy()
    {

#if UNITY_EDITOR
        DestroyImmediate(Panel);
#else

    Destroy(Panel);
#endif

    }

    void DestroyKeepPanel()
    {

        
#if UNITY_EDITOR
        DestroyImmediate(Container);
#else

    Destroy(Container);
#endif

    }


#if UNITY_EDITOR
    [CustomEditor(typeof(UIBuilder))]
	public class UIBuilder_Editor : Editor
	{

		public UIBuilder Target;

		void Awake()
		{
			Target = (UIBuilder)target;
		}

		public override void OnInspectorGUI()
		{


			Undo.RecordObject(Target, Target.name);

			Inspector();

			if (GUI.changed)
			{
				EditorUtility.SetDirty(Target);
			}
		}

        private void Inspector()
        {
            Utility.SetGUIColor(Color.gray);
            using (new EditorGUILayout.VerticalScope("Box"))
            {
                Utility.ResetGUIColor();

                EditorGUILayout.LabelField("UI builder");

                Target.adjust = EditorGUILayout.Vector3Field("Adjust", Target.adjust);
                Target.scale = EditorGUILayout.FloatField("Scale", Target.scale);
                Target.Parent = EditorGUILayout.ObjectField("Parent", Target.Parent, typeof(GameObject), true) as GameObject;
                Target.dataToCollect = EditorGUILayout.ObjectField("Data To Collect", Target.dataToCollect, typeof(ScriptableObject), true) as ScriptableObject;
                Target.useriinterface = EditorGUILayout.ObjectField("Ui Prefabs", Target.useriinterface, typeof(ScriptableObject), true) as ScriptableObject;
                Target.databaseManager = EditorGUILayout.ObjectField("Database Manager", Target.databaseManager, typeof(Firebase), true) as Firebase;

                if (Utility.GUIButton("Layout Create Test", Color.gray, Color.white))
                {
                    Target.EditorBuild(0);
                }
                if (Utility.GUIButton("Destroy Test", Color.gray, Color.white))
                {
                    Target.Destroy();
                }




            }
        }
    }
#endif

}

