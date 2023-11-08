using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsController
{

    private string levelRecord = "levelRecord";
    private string coin = "coin";

    public int GetCoin()
    {
        return PlayerPrefs.GetInt(coin);
    }

    public int GetLevelRecord()
    {
        return PlayerPrefs.GetInt(levelRecord);
    }

    public void UpdateCoin()
    {
        //Debug.Log("UPDATE COOIN: " + coin);
        PlayerPrefs.SetInt(coin, SystemStatic.coin);
    }

    public void UpdateRecordLevel ()
    {
        //Debug.Log("SystemStatic.level: " + SystemStatic.level);
        //Debug.Log("SystemStatic.levelRecord: " + SystemStatic.levelRecord);

        if (SystemStatic.level > SystemStatic.levelRecord)
            PlayerPrefs.SetInt(levelRecord, SystemStatic.level);
    }

}
