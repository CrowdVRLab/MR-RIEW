using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Questionaire/NewQuestionaireSubPart")]
public class QuestionaireSubPart : ScriptableObject
{
    public string name;
    public string description;
    public QuestionaireQuestion[] questions;
   
    public QuestionaireSubPart(string name, string description, QuestionaireQuestion[] questions)
    {
        this.name = name;
        this.description = description;
        this.questions = questions;
    }

    // method for cloning object 
    public QuestionaireSubPart DeepCopy()
    {
        QuestionaireSubPart deepcopySubPart = new QuestionaireSubPart(this.name, this.description, this.questions);

        return deepcopySubPart;
    }
}

[Serializable]
public class QuestionaireQuestion
{
    public string question;
    public string helpText;
    public UitType uielement;
    [HideInInspector]
    public bool valuebool;
    [HideInInspector]
    public float valuefloat;
    public string[] Options;
    [HideInInspector]
    public string answer;
}


//upload dataset

[Serializable]
public class QuestionaireSubPartUpload
{
    public string name;
    public QuestionaireQuestionUpload[] questions;

    public QuestionaireSubPartUpload(string name, string description, QuestionaireQuestion[] questions1)
    {

        this.name = name;
        this.questions = new QuestionaireQuestionUpload[questions1.Length];

        for (int i = 0; i < questions1.Length; i++)
        {
            this.questions[i] = (QuestionaireQuestionUpload)questions1[i];
        }
    }

    public static explicit operator QuestionaireSubPartUpload(QuestionaireSubPart v)
    {
        return new QuestionaireSubPartUpload(v.name, v.description, v.questions);
    }
}

[Serializable]
public class QuestionaireQuestionUpload
{
    public string question;
    public string answer;

    public QuestionaireQuestionUpload(string question, string answer)
    {
        this.question = question;
        this.answer = answer;
    }


    public static explicit operator QuestionaireQuestionUpload(QuestionaireQuestion v)
    {
        return new QuestionaireQuestionUpload(v.question, v.answer);
    }
}


//enum
public enum UitType
{
    Slider,
    Radio,
}
