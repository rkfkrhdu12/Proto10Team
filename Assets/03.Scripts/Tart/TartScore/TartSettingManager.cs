using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

public class TartSettingManager : MonoBehaviour
{
    [Header("ScoreManager를 넣어주세요.")]
    public ScoreManager scoreManager;

    [Header("사이즈에 맞춰 토핑 프리팹을 넣자!")]
    public GameObject[] smallSizeToppings;
    public GameObject[] midSizeToppings;
    public GameObject[] largeSizeToppings;

    [Header("젤리빈 프리팹")]
    public GameObject[] jellyBeans;

    [Header("오류용 오브젝트")]
    public GameObject errorTopping;

    private void Awake()
    {
        //scoreManager.TestTartInput();
        //AnswerTartSetting();

    }
    void Start()
    {

    }


    public void InitAnswerTartSetting()
    {
        if (scoreManager.tartManager.answerTart.gameObject.transform.childCount==0)
        {
            LogManager.Log("처음 세팅하는 타르트");
        }
        else
        {
            LogManager.Log("처음 세팅하는 타르트가 아님!!");
            for (int i = 0; i < scoreManager.tartManager.answerTart.gameObject.transform.childCount; i++)
            {
                Destroy(scoreManager.tartManager.answerTart.gameObject.transform.GetChild(i).gameObject);
            }
        }

    }

    /// <summary>
    /// 정답 타르트의 토핑들을 세팅(오브젝트 배치)합니다.
    /// </summary>
    public void AnswerTartSetting()
    {
        InitAnswerTartSetting();
        Tart answerTart;
        answerTart = scoreManager.tartManager.answerTart;//비어있는(?) 정답 타르트를 가져온다.

        for (int i = 0; i < answerTart.toppingList.Count; i++)//오브젝트 배치를 위해 for문을 돌린다.
        {
            // Debug.Log("현재 i : " + i);
            //  Debug.Log("현재 토핑사이즈 : " + answerTart.toppingList[i].toppingSize);
            //  Debug.Log("현재 토핑넘 : " + answerTart.toppingList[i].toppingNum);
            GameObject nowToppingObj = ReturnTopping(answerTart.toppingList[i].toppingSize,
             answerTart.toppingList[i].toppingNum,
             answerTart.toppingList[i].toppingType);

            //복제
            GameObject tempToppingObj = Instantiate(nowToppingObj,
                new Vector3(answerTart.toppingList[i].answerPosX * 1.5f,
                            answerTart.toppingList[i].answerPosY * 1.5f,
                            answerTart.toppingList[i].answerPosZ * 1.5f),
                nowToppingObj.transform.rotation,scoreManager.tartManager.answerTart.gameObject.transform);
            LogManager.Log("리지드바디 삭제.");
            Destroy(tempToppingObj.transform.GetChild(0).gameObject.GetComponent<Rigidbody>());


           // tempToppingObj.transform.SetParent(scoreManager.tartManager.answerTart.gameObject.transform);

        }
    }

    /// <summary>
    /// 토핑 게임 오브젝트를 반환한다.
    /// </summary>
    /// <param name="topSize">소=1,중=2,대=3 </param>
    /// <param name="topNum">토핑의 종류</param>
    /// <param name="topType">토핑별 타입</param>
    /// <returns>GameObject</returns>
    public GameObject ReturnTopping(int topSize, int topNum, int topType)
    {

        //+ 토핑 타입(회전각도 같은거)는 아직 구별하지 않아도 된다고 판단하여
        //if문 하나로 그냥 단순하게 작성함.

        if (topSize == 1) //소형 사이즈 구간
        {
            if (topNum == 4)//젤리빈일 경우
            {
                if (topType == 0)
                {
                    return smallSizeToppings[topNum - 1];
                }
                else
                {
                    return jellyBeans[topType - 1];
                }
            }
            else
            {
                return smallSizeToppings[topNum - 1];
            }

        }
        else if (topSize == 2) //중형 사이즈 구간
        {
            return midSizeToppings[topNum - 1];
        }
        else if (topSize == 3) //대형 사이즈 구간
        {
            return largeSizeToppings[topNum - 1];
        }
        else
        {
            LogManager.Log("topSize는 1,2,3 중 하나여야 합니다. csv를 확인하라구~");
            return errorTopping;
        }
    }
}
