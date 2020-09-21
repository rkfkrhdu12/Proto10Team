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

    [Header("오류용 오브젝트")]
    public GameObject errorTopping;

    // Start is called before the first frame update

    private void Awake()
    {
        scoreManager.TestTartInput();
        AnswerTartSetting();

    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 정답 타르트의 토핑들을 세팅(오브젝트 배치)합니다.
    /// </summary>
    public void AnswerTartSetting()
    {

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
                new Vector3(answerTart.toppingList[i].answerPosX,
                answerTart.toppingList[i].answerPosY,
                answerTart.toppingList[i].answerPosZ),
                nowToppingObj.transform.rotation);
         

            //아래 주석이 통하지 않는 이유:
            //타르트 매니저 자체에 Tart컴포넌트가 생성되어있기 때문에.
            //Tart의 게임오브젝트를 불러오면...당연히 타르트 매니저가 불러와진다...
            
            //Topping tempTopping = tempToppingObj.GetComponent<Topping>();
            //if (tempTopping)
            //{

            //}
            //tempTopping.toppingSize = answerTart.toppingList[i].toppingSize;
            //tempTopping.toppingNum = answerTart.toppingList[i].toppingNum;
            //tempTopping.toppingScore = answerTart.toppingList[i].toppingScore;
            //tempTopping.toppingType = answerTart.toppingList[i].toppingType;
            //tempTopping.answerPosX = answerTart.toppingList[i].answerPosX;
            //tempTopping.answerPosY= answerTart.toppingList[i].answerPosY;
            //tempTopping.answerPosZ = answerTart.toppingList[i].answerPosZ;

            tempToppingObj.transform.SetParent(scoreManager.tartManager.answerTart.gameObject.transform);

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
            return smallSizeToppings[topNum - 1];
        }
        else if (topSize ==2) //중형 사이즈 구간
        {
            return midSizeToppings[topNum - 1];
        }
        else if (topSize ==3) //대형 사이즈 구간
        {
            return largeSizeToppings[topNum - 1];
        }
        else
        {
            Debug.Log("topSize는 1,2,3 중 하나여야 합니다. csv를 확인하라구~");
            return errorTopping;
        }
    }
}
