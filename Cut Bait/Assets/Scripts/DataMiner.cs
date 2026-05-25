using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;
using System;
using System.Linq;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DataMiner : MonoBehaviour
{
    ////probably important
    ////bool dataHasBeenSent = false;
    ////public int StopLoadingData = 0;
    //public TimeManager timeManager;
    //public MoneyManager moneyManager;
    public VideoManager videoManager;
    //public TextManager textManager;
    
    public bool SentData;
    public  DataMiner dataMiner;
    public string logString;
    StreamWriter file;

    // Per-day counts of correct/incorrect scans and final decisions.
    // I suggest including ratios (correct/incorrect) or something similar in the final study.
    // May add more data such as avg. time per decision, low on time.
    //public int day1CorrectDecisions, day2CorrectDecisions, day3CorrectDecisions, day4CorrectDecisions,
    //    day1IncorrectDecisions, day2IncorrectDecisions, day3IncorrectDecisions, day4IncorrectDecisions,
    //    day1CorrectScans, day2CorrectScans, day3CorrectScans, day4CorrectScans,
    //    day1IncorrectScans, day2IncorrectScans, day3IncorrectScans, day4IncorrectScans;
    public float playTime;

    public string ID, version;
    public TMP_InputField input;
    public GameObject WorkerIDScreen, endScreen;


    public void readID()
    {
        ID = input.text;
        WorkerIDScreen.SetActive(false);
        Debug.Log(ID);

        if (version == "video")
            videoManager.PlayVideo();
    }

    void Start()
    {
        WorkerIDScreen.SetActive(true);
    }

    //void Update()
    //{
    //    if (version != "video" && version != "text") {
    //        if (((timeManager.day >= 3 && playTime >= 1800f) || timeManager.day > 4) && !SentData) {
    //                LogData();
    //        }
    //    }
    //}

    public void LogData()
    {
        endScreen.SetActive(true);
        SentData = true;

        logString += ID + ", " + version + ", " + playTime.ToString(); 
        //+ ", " + day1CorrectDecisions.ToString() + ", " + day2CorrectDecisions.ToString() + ", " + day3CorrectDecisions.ToString() + ", " + day4CorrectDecisions.ToString() + ", " +
        //day1IncorrectDecisions.ToString() + ", " + day2IncorrectDecisions.ToString() + ", " + day3IncorrectDecisions.ToString() + ", " + day4IncorrectDecisions.ToString() + ", " +
        //day1CorrectScans.ToString() + ", " + day2CorrectScans.ToString() + ", " + day3CorrectScans.ToString() + ", " + day4CorrectScans.ToString() + ", " +
        //day1IncorrectScans.ToString() + ", " + day2IncorrectScans.ToString() + ", " + day3IncorrectScans.ToString() + ", " + day4IncorrectScans.ToString();

       StartCoroutine(WriteTextViaPHP(logString, "https://gamesux.com/fromunity_cutbait.php"));
    }

    IEnumerator WriteTextViaPHP(string data, string destination)
    {
        //so it compiles
        yield return new WaitForSeconds(3f);
        //WWWForm form = new WWWForm();
        //form.AddField("data", data);
        //UnityWebRequest www = UnityWebRequest.Post(destination, form);
        //if (Application.platform != RuntimePlatform.WebGLPlayer)
        //    www.SetRequestHeader("User-Agent", "Unity 2020");
        //www.SendWebRequest();
        //yield return www.isDone;
        //if ((www.result == UnityWebRequest.Result.ConnectionError) || (www.result == UnityWebRequest.Result.ProtocolError))
        //{
        //    Debug.Log(www.error);
        //}
        //else
        //{
        //    Debug.Log("Data Sent Successfully");
        //}
    }
}