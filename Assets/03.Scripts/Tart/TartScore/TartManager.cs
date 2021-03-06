﻿using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions.Must;
using System;

[Serializable]
public struct ToppingData
{
    public GameObject thisObject;
    public int toppingCode;
    public int toppingSize;
    public int toppingNum;
    public int toppingType;

    public bool isCheck;

    public float toppingScore;
    public float answerPosX;
    public float answerPosY;
    public float answerPosZ;

    public void SetToppingInfo(int code, int size, int num, int type, float score, float x, float y, float z)//토핑의 여러가지 정보들을 Set
    {
        toppingCode = code;
        toppingSize = size;
        toppingNum = num;
        toppingType = type;
        toppingScore = score;
        answerPosX = x;
        answerPosY = y;
        answerPosZ = z;
    }

    public void Check()
    {
        isCheck = true;
    }
}

public class TartManager : MonoBehaviour
{
    [Tooltip("정답 타르트입니다.")]
    public Tart answerTart;

    [Tooltip("정답 타르트의 게임오브젝트")]
    public GameObject answerTartObj;

    public List<Dictionary<string, object>> csvData; //그냥 csv 데이터를 담고있음.

    public void InitTartTopping()
    {
        //else
        //{
        //    LogManager.Log("처음으로 읽어온게 아니야...지우고 다시 한다...");
        //    csvData.Clear();
        //    answerTart.ClearTopping();
        //    Destroy(gameObject.GetComponent<Tart>());
        //    foreach (Topping testTopping in gameObject.GetComponents<Topping>())
        //    {
        //        Destroy(testTopping);
        //    }
        //    answerTart.gameObject.SetActive(false);
        //}
    }
/// <summary>
/// 
/// </summary>
/// <param name="tartCode">타르트의 코드입니다.</param>
    public void DataLoadAndSetAnswerTart(int tartCode)
    {
        InitTartTopping();
        string finishFileName;
        finishFileName = "Tart_" + System.Convert.ToString(tartCode);
        CsvDataLoad(finishFileName);
        SetAnswerTart(tartCode);
    }

    public void CsvDataLoad(string fileName) // filename.csv데이터를 읽어옵니다. 
    {
        csvData = CsvReader.Read("TartDataFiles/" + fileName);
        LogManager.Log("OK?...");
    }
    public void SetAnswerTart(int tartCode) // 읽어온 csvData를 바탕으로 answerTart를 세팅합니다.
    {
        //임시로 쓰일 토핑 정보들
        int nowVal = 0;
        int nowCode;
        int nowSize;
        int nowNum;
        int nowType;
        float nowScore;
        float nowPosX;
        float nowPosY;
        float nowPosZ;

        while (true)
        {
            if ((int)csvData[nowVal]["TOPPING_CODE"] == 25)
            {
                LogManager.Log("임시 타르트에 모든 토핑을 추가 완료 하였습니다.");
                break;
            }

            nowCode = (int)csvData[nowVal]["TOPPING_CODE"];
            nowSize = (int)csvData[nowVal]["TOPPING_SIZE"];
            nowNum = (int)csvData[nowVal]["TOPPING_NUM"];
            nowType = (int)csvData[nowVal]["TOPPING_TYPE"];

            
            nowScore = (float)System.Convert.ToDouble(csvData[nowVal]["TOPPING_SCORE"]);
            nowPosX = (float)System.Convert.ToDouble(csvData[nowVal]["TOPPING_POS_X"]);
            nowPosY = (float)System.Convert.ToDouble(csvData[nowVal]["TOPPING_POS_Y"]);
            nowPosZ = (float)System.Convert.ToDouble(csvData[nowVal]["TOPPING_POS_Z"]);
            //nowPosX = (float)csvData[nowVal]["TOPPING_POS_X"];
            //nowPosY = (float)csvData[nowVal]["TOPPING_POS_Y"];
            //nowPosZ = (float)csvData[nowVal]["TOPPING_POS_Z"];

            //Debug.Log("nowCode = " + nowCode + " | nowVal = " + nowVal);

            //Debug.Log("now어쩌고에 데이터 할당 완료. 임시 토핑에 세팅 시작!");
            ToppingData tempTopping = new ToppingData(); //임시로 쓰일 현재 토핑

            tempTopping.SetToppingInfo(nowCode, nowSize, nowNum, nowType, nowScore, nowPosX, nowPosY, nowPosZ);

            // Debug.Log("임시 토핑 세팅 완료. 임시 타르트에 임시 토핑 추가.");

            answerTart.AddTopping(tempTopping);
            answerTart.tartCode = tartCode;

            nowVal += 1;
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
