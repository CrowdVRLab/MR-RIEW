using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.IO;

public class LocalStorage : Storage
{

    public ScriptableObject Questionaire;

    // Start is called before the first frame update
    void Start()
    {
    }

    public override void SaveQuestionaire(DataToCollect questionaire) {

        DataToCollectUpload q = (DataToCollectUpload)questionaire;
        string json = JsonUtility.ToJson(q);
        StartCoroutine(SaveData(json));

    }

    IEnumerator SaveData(string json)
    {
        yield return new WaitForSeconds(1);
        string path = Application.persistentDataPath + "/results.json";
        StreamWriter writer = new StreamWriter(path, true);
        writer.Write(json);
        writer.Close();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
