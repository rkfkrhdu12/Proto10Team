using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{


    public TMP_Text uiScore;

    [Tooltip("타르트매니저 하나 만들어서 넣어주셔야 함")]
    public TartManager tartManager; // 타르트매니저. 정답 타르트!!의 정보가 들어있습니다.

    [Tooltip("우리 팀이 만든 완성본 타르트")]
    public Tart myTart; // 우리 팀이 만든 타르트입니다.

    [Tooltip("우리 팀의 점수")]
    public int myScore; //우리 팀의 점수(완성도)입니다.

    [Header("토핑의 채점 범위")]
    [Range(0, 30)]
    public float lastCheckDis;

    [Header("이 정도 거리 차이는 정답으로 인정합니다.")]
    [Range(1, 3)]
    public float allowDis;
    private void Awake()
    {

    }


    void Update()
    {
        if (uiScore!=null)
        {
            uiScore.text = "Perfection: " + System.Convert.ToString(myScore);
        }

    }

    /// <summary>
    /// 25번 타르트 csv를 읽어오고...그렇습니다.
    /// </summary>
    public void TestTartInput()
    {
        tartManager.DataLoadAndSetAnswerTart(25);
    }

    /// <summary>
    /// 해당 타르트의 범위 바깥에 해당 토핑이 있는지 검사합니다. 
    /// </summary>
    public bool IsToppingOutOfRange(Tart nowTart, Topping nowTopping)
    {

        if (nowTopping.gameObject.transform.position.x > nowTart.limitPos1.gameObject.transform.position.x &&
   nowTopping.gameObject.transform.position.z > nowTart.limitPos1.gameObject.transform.position.z &&
   nowTopping.gameObject.transform.position.x < nowTart.limitPos2.gameObject.transform.position.x &&
   nowTopping.gameObject.transform.position.z < nowTart.limitPos2.gameObject.transform.position.z)
        {//y축의 정보는 사용하지 않음...
            return false;
        }
        else
        {
            return true;
        }

    }


    /*
     * 1. 내 타르트 토핑과 정답 타르트 토핑은 nowToppingPos라는 동일한 형식의(?) 위치 정보를 가지고 있다.
     * 2. 이것을 가지고 거리 비교를 하면 된다. 
     * 3. 현재 내 타르트 토핑은 addTopping을 하지 않았지만, 리스트에 nowToppingPos라는 위치 정보를 가지고 있다.
     * 4. 즉, 직접적으로 gameObject어쩌고를 하지 않아도 논리적으로는 비교가 가능하다?
     */


    /*
     * 1. 정답 타르트의 토핑 리스트에서 하나를 뽑아낸다.
     * 2-1. 그리고 내 타르트에서 그것과 같은 사이즈, 넘버를 가진 녀석들을 뽑아낸다.
     * 2-2. 채점 완료한 녀석들은 같은 사이즈, 넘버를 가졌더라도 이 범위에서 제외한다.
     * 3. 정답 타르트의 한 토핑과, 내 타르트에서 뽑아낸 녀석들을 비교하여 가장 가까운 녀석을 찾아낸다.
     * 4. 그 가장 가까운 녀석을 채점완료(isCheck)표시하고, 거리별 점수를 지급한다.
     * 5. 위와 같은 반복을 돈 후, 내 타르트에서 isCheck가 false인 토핑들로 감점을 부여한다.
     * ㅇㅋ 해보자.
     */

    public void Init()
    {
        myScore = 0;
        for (int i = 0; i < myTart.toppingList.Count; i++)
        {
            myTart.toppingList[i].isCheck = false; //채점 완료 표시.
        }

    }
    public void log(object l)
    {
        LogManager.Log("ScoreManager : " + l);
    }

    public void CheckScoreV2()
    {
        Init();
        Tart answerTart = tartManager.answerTart; //정답 타르트를 가져왔다.

        for (int i = 0; i < answerTart.toppingList.Count; i++)
        {
            //1. 정답 타르트의 토핑 리스트에서 토핑 하나를 뽑아낸다.
            Topping answerTopping = answerTart.toppingList[i];
            log(i + "번째 정답 토핑을 채점을 시작합니다.");

            //2. 그리고 내 타르트에서 그것과 같은 사이즈, 넘버를 가진 녀석'들'을 뽑아낸다.
            //- 저번과 같이 그냥 배열에다가 인덱스를 저장하는 것으로 해도 될듯.
            int[] sameToppingIndex = new int[24];
            int sameToppingIndexVal = 0;
            for (int j = 0; j < myTart.toppingList.Count; j++)
            {

                if (answerTopping.toppingSize == myTart.toppingList[j].toppingSize &&
                    answerTopping.toppingNum == myTart.toppingList[j].toppingNum)
                {
                    if (myTart.toppingList[j].isCheck != true) //2-2.채점 완료한 녀석들은 같은 사이즈, 넘버를 가졌더라도 이 범위에서 제외한다.
                    {
                        sameToppingIndex[sameToppingIndexVal] = j;
                        log("내 타르트의 토핑 중 " + sameToppingIndex[sameToppingIndexVal] + "번째의 토핑인" + myTart.toppingList[j].gameObject.name + "과 비교하도록 합니다.");
                        sameToppingIndexVal += 1; //하나 추가하고.

                    }

                }

            }

            log("현재 " + sameToppingIndexVal + "개의 토핑과 비교합니다.");
            //만약 같은 토핑이 발견되어지지 않았다면?
            //앞에서 다 찾았다는 뜻이기도 하니까, 위로 올려보낸다.
            if (sameToppingIndexVal == 0)
            {
                log("비교할 토핑이 없습니다. 다음 정답 토핑 채점을 시작합니다.");
                continue;
            }

            //이렇게 똑같은 녀석들을 뽑아왔다. 다음은...
            // 3. 정답 타르트의 한 토핑과, 내 타르트에서 뽑아낸 녀석들을 비교하여 가장 가까운 녀석을 찾아낸다.
            //'거리값이 가장 적은 녀석'을 찾아낸다.

            log("거리를 비교하여 가장 가까운 토핑을 찾아내겠습니다.");

            float minDis = 25252;
            int lastToppingIndex = 0; //마지막 최종 채점을 받을 토핑의 인덱스.
            for (int k = 0; k < sameToppingIndexVal; k++)
            {
                float myPosX = myTart.toppingList[sameToppingIndex[k]].gameObject.transform.localPosition.x;
                float myPosZ = myTart.toppingList[sameToppingIndex[k]].gameObject.transform.localPosition.z;

                Vector2 tempMyPos = new Vector2(myPosX, myPosZ);
                log("이 토핑의 위치는" + tempMyPos);
                Vector2 tempAnswerPos = new Vector2(answerTopping.answerPosX, answerTopping.answerPosZ);
                float compareDis = Vector2.Distance(tempAnswerPos, tempMyPos);
                log("현재 가장 가까운 거리는 " + minDis + " 입니다.");
                log(myTart.toppingList[sameToppingIndex[k]].gameObject.name + "의 거리는 " + compareDis + "입니다.");

                if (minDis > compareDis)
                {
                    log(myTart.toppingList[sameToppingIndex[k]].gameObject.name + "를 제일 가까운 토핑으로 등록합니다.");
                    minDis = compareDis;
                    lastToppingIndex = sameToppingIndex[k];

                }

            }
            log("가까운 토핑 등록이 끝났습니다. 가장 가까운 토핑은 " + myTart.toppingList[lastToppingIndex].gameObject.name + " 입니다.");
            //거리값이 작은 녀석을 찾았을 것이다. 그렇다면 이제
            // 4. 그 가장 가까운 녀석을 채점완료(isCheck)표시하고, 거리별 점수를 지급한다.
            myTart.toppingList[lastToppingIndex].isCheck = true; //채점 완료 표시.
            if (minDis > lastCheckDis) //가장 작은 녀석이 채점 범위를 넘어섰을 때.
            {
                log("정답 토핑에 비해 너무 멀리 있습니다. 0점을 지급합니다.");
                myScore += 0; //0점 지급.
            }
            else //채점 범위 내에 있을 때.
            {
                if (minDis <= allowDis) //이 정도는 만점으로 쳐주기.
                {
                    float score = answerTopping.toppingScore;
                    log("거리가 매우 가깝기 때문에 만점을 지급합니다. : " + score);
                    myScore += (int)score;
                }
                else //아닐 때.
                {
                    float temp = minDis / lastCheckDis; //0~1의 값이 나온다.
                    float temp2 = answerTopping.toppingScore * temp; //감점되어야 할 점수가 나온다.
                    float temp3 = answerTopping.toppingScore - temp2; //총 점수에서 감점한다.
                    log("거리가 애매하기 때문에 계산된 점수를 지급합니다." + temp3);
                    myScore += (int)temp3;//myScore에 추가.
                }

            }
        }

        //마지막 감점 검사!
        for (int i = 0; i < myTart.toppingList.Count; i++)
        {
            if (myTart.toppingList[i].isCheck == false)
            {
                log("상관없는 토핑이 있습니다. 감점합니다.");
                switch (myTart.toppingList[i].toppingSize)
                {
                    case 1://소형
                        myScore -= 1;
                        break;
                    case 2:
                        myScore -= 2;
                        break;
                    case 3:
                        myScore -= 4;
                        break;
                    default:
                        break;
                }
            }
        }
        if (myScore <= 0)
        {
            myScore = 0;
        }
        log("채점을 마쳤습니다. 총 점수는 " + myScore + "점 입니다.");

    }


}
