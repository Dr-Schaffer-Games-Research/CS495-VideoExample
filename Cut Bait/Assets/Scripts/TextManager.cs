using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    public DataMiner dataMiner;


    public void finish()
    {
        dataMiner.LogData();
    }

    private void Update()
    {
         dataMiner.playTime += Time.deltaTime;
    }

}
