using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.ComponentModel;

[CreateAssetMenu(menuName = "Questionaire/NewDataToCollect")]
public class DataToCollect : ScriptableObject
{
    [SerializeField]
    public Questionaire questionaire;

    [HideInInspector]
    public string[] sequenceExp;

    [HideInInspector]
    public int[] sequenceImp;

    private void Awake()
    {
        Init();
    }
    public void Init() {

        loadScriptableObjects();
        deepCopyScriptableObjects();
        cleanAnswers();

    }
    private void deepCopyScriptableObjects() {

        for (int j = 0; j < questionaire.parts.Length; j++)
        {
            for (int i = 0; i < questionaire.parts[j].subparts.Length; i++)
            {

                questionaire.parts[j].subparts[i] = questionaire.parts[j].subparts[i].DeepCopy();

            }
        }
    }
    private void cleanAnswers() {

        for (int j = 0; j < questionaire.parts.Length; j++)
        {
            for (int i = 0; i < questionaire.parts[j].subparts.Length; i++)
            {
                for (int k = 0; k < questionaire.parts[j].subparts[i].questions.Length; k++)
                {
                    questionaire.parts[j].subparts[i].questions[k].answer = null;
                }
            }
        }
    }
    private void loadScriptableObjects() {

        for (int j = 0; j < questionaire.parts.Length; j++)
        {
            questionaire.parts[j].subparts = new QuestionaireSubPart[questionaire.parts[j].subpartsScripts.Length];

            for (int i = 0; i < questionaire.parts[j].subpartsScripts.Length; i++)
            {
                Debug.Log(j.ToString() + " " + i.ToString());
                var newObject = Instantiate(questionaire.parts[j].subpartsScripts[i]);
                var qsp = (QuestionaireSubPart)newObject;
                questionaire.parts[j].subparts[i] = qsp;
            }
        }

    }
}

[Serializable]
public class Questionaire
{
    public QuestionairePart[] parts;
}

[Serializable]
public class QuestionairePart
{
    public string name;
    public string referenceName;
    public string description;
    
    [HideInInspector]
    public QuestionaireSubPart[] subparts;
   
    public ScriptableObject[] subpartsScripts;
}


//upload dataset
public class DataToCollectUpload : ScriptableObject
{
    [SerializeField]
    public QuestionaireUpload questionaire;

    [HideInInspector]
    public string[] sequenceExp;

    [HideInInspector]
    public int[] sequenceImp;

    public DataToCollectUpload(Questionaire v, int[] sequenceImp, string[] sequenceExp)
    {
        this.questionaire = (QuestionaireUpload)v;
        this.sequenceExp = sequenceExp;
        this.sequenceImp = sequenceImp;
    }

    public static explicit operator DataToCollectUpload(DataToCollect v)
    {
        return new DataToCollectUpload(v.questionaire,v.sequenceImp,v.sequenceExp);
    }

}

[Serializable]
public class QuestionaireUpload
{
    public QuestionairePartUpload[] parts;

    public QuestionaireUpload(QuestionairePart[] v)
    {
        this.parts = new QuestionairePartUpload[v.Length];

        for (int i = 0; i < v.Length; i++)
        {
            this.parts[i] = (QuestionairePartUpload)v[i];
        }
    }

    public static explicit operator QuestionaireUpload(Questionaire v)
    {
        return new QuestionaireUpload(v.parts);
    }
}

[Serializable]
public class QuestionairePartUpload
{
    public string name;
    public QuestionaireSubPartUpload[] subparts;

    public QuestionairePartUpload(string name,  QuestionaireSubPart[] subparts1)
    {
        this.name = name;

        this.subparts = new QuestionaireSubPartUpload[subparts1.Length];

        for (int i= 0; i < subparts1.Length; i++)
        {
            this.subparts[i] = (QuestionaireSubPartUpload)subparts1[i];
        }

    }

    public static explicit operator QuestionairePartUpload(QuestionairePart v)
    {
        return new QuestionairePartUpload(v.referenceName, v.subparts);
    }
}

