using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Tooltip("타르트매니저 하나 만들어서 넣어주셔야 함")]
    public TartManager tartManager; // 타르트매니저. 정답 타르트!!의 정보가 들어있습니다.

    [Tooltip("우리 팀이 만든 완성본 타르트")]
    public Tart myTart; // 우리 팀이 만든 타르트입니다.

    [Tooltip("우리 팀의 점수")]
    public float myScore; //우리 팀의 점수(완성도)입니다.

    void Start()
    {
        TestTartInput();
        CheckScore();
    }

    void Update()
    {

    }

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
            nowTopping.gameObject.transform.position.x < nowTart.limitPos2.gameObject.transform.position.z)
        {//y축의 정보는 사용하지 않음...
            return false;
        }
        else
        {
            return true;
        }

    }



    public void CheckScore()
    {

        Tart answerTart = tartManager.answerTart;
        //while (true)
        //{

            //정답 타르트의 토핑들 중 하나를 가져온다.
            //nowAnswerTopping = tartManager.answerTart.toppingList[checkVal];

            //다음. 정답 타르트 토핑(이하 정토)과 우리팀 타르트 토핑(이하 우토)을 비교해야한다.
            //비교 전, 그래도 타르트라고 인정해주는 범위를 벗어난 우토는 리스트에서 제거해야한다...
            for (int i = 0; i < myTart.toppingList.Count; i++)
            {
                if (IsToppingOutOfRange(myTart, myTart.toppingList[i])) //만약 범위 바깥에 있으면...
                {
                    myTart.toppingList.RemoveAt(i); //제거.
                }

            }

            //제거를 완료했으면 이제 채점을 해야한다...
            //정토에다가 우토를 하나하나 다 대조하는 방법밖에 떠오르지 않기 때문에, 그것을 한다...
            for (int answerVal = 0; answerVal < answerTart.toppingList.Count; answerVal++)
            {
                int[] nowCheckArr = new int[23]; // +1의 해결책으로 나온 배열. 이 배열은 answerVal이 바뀔 때마다 초기화 된다.
                int nowCheckVal = 0; // nowCheckArr의 인덱스를 위해서 만든 변수.

                for (int myVal = 0; myVal < myTart.toppingList.Count; myVal++)
                {
                    if (
                        //우선 토핑 사이즈가 같아야하고,
                        answerTart.toppingList[answerVal].toppingSize == myTart.toppingList[myVal].toppingSize &&
                        //토핑 넘도 같아야 한다...
                        answerTart.toppingList[answerVal].toppingNum == myTart.toppingList[myVal].toppingNum &&
                        //마지막으로 토핑 타입까지 같으면 일단 같은 토핑이라고 판별 완료
                        answerTart.toppingList[answerVal].toppingType == myTart.toppingList[myVal].toppingType &&
                        //진짜 진짜 마지막으로 얘를 검사한 적도 없어야함
                        myTart.toppingList[myVal].isCheck == false
                        )
                    {
                        //같은 토핑이 한 3개정도 있다고 쳐보자. 방금 찾은 한 녀석 말고도 다른 녀석들을 찾으러 가야한다.
                        //다른 녀석들을 찾으러 가기 전에, 이 녀석을 간략하게라도 저장해놓을 무언가가 필요하다...
                        // +1 그래서 배열 하나로 myVal에 해당하는걸 저장해놓기로 했다. 

                        nowCheckArr[nowCheckVal] = myVal; // myVal을 저장해놓는다.
                        nowCheckVal += 1;//얘도 1 더해주고...

                    }

                }// 이 for문으로 인해 nowCheckArr에는 내 타르트의 토핑 리스트 '인덱스'가 3개정도 들어가게 되었다.
                //이제 이 들어있는 무언가를 정타에 대입을 해봐야한다.
                //기획서에 따르면, 거리를 비교해서 가장 가까운걸 대입시켜야한다.

                //그리하여, 현재 비교해야할 정답 타르트 토핑 하나의 위치를 저장해놓는다. 
                Vector2 nowAnswerToppingPos =
                    new Vector2(answerTart.toppingList[answerVal].answerPosX,
                    answerTart.toppingList[answerVal].answerPosZ);

                //또, 거리가 가장 가까운걸 골라내기 위한 변수를 만든다.
                float nearDis = 252525;
                float nowDis = 0;

                int lastCheckVal = 0; // 결과로 나온 우타의 우토 인덱스...'최종적인 검사 대상'을 정하기 위해.

                for (int i = 0; i < nowCheckVal; i++)
                {
                    //우타의 우토 위치를 만들어놓고
                    Vector2 nowMyToppingPos =
                        new Vector2(myTart.toppingList[nowCheckArr[nowCheckVal]].gameObject.transform.position.x,
                        myTart.toppingList[nowCheckArr[nowCheckVal]].gameObject.transform.position.z);

                    nowDis = Vector2.Distance(nowAnswerToppingPos, nowMyToppingPos);

                    //Distance 계산하기
                    if (nearDis > nowDis) // 현재 '가장 가까운 거리'보다 더 가깝다면?
                    {
                        nearDis = nowDis; //갱신해주고
                        lastCheckVal = nowCheckArr[nowCheckVal];//얘도 이렇게 해준다.
                    }

                }
                //이렇게 lastCheckVal이 정해졌으면, 진짜 마지막으로 거리에 따른 점수를 부여한다.

                //문제1 발생...
                //물론 크기상으로 1/2/3이라는 기준은 있지만, 그건 모델링할때 이야기고, 유니티에서 불러와봤자 다 똑같이 스케일은 1일 것이다.
                //일단은 3이하만 점수 주고 0.5이하는 그냥 점수 다 주는걸로...
                Vector2 nowLastMyToppingPos =
                 new Vector2(myTart.toppingList[lastCheckVal].gameObject.transform.position.x,
                  myTart.toppingList[lastCheckVal].gameObject.transform.position.z);

                float lastDis = Vector2.Distance(nowAnswerToppingPos, nowLastMyToppingPos);

                if (lastDis<3)//3보다 안에 있냐?
                {
                    if (lastDis < 0.5)//0.5보다 안에 있냐?
                    {

                        myScore += answerTart.toppingList[answerVal].toppingScore; //제대로 지급!
                    }
                    else
                    {

                        myScore += answerTart.toppingList[answerVal].toppingScore / lastDis; //나눠서 지급

                    }
                }
                else
                {//스코어 변동 없음.
                    myScore += 0;

                }
                myTart.toppingList[lastCheckVal].isCheck = true; //채점을 했다는 표시로 true를 해준다.

            }
        //마지막으로, isCheck가 안된 녀석들(장애물같은 애들)을 구해서 완성도를 차감시키자.
        for (int i = 0; i < myTart.toppingList.Count; i++)
        {
            if (myTart.toppingList[i].isCheck==false)//검사가 안된 녀석이 있다면?
            {
                if (myTart.toppingList[i].toppingSize ==0)//소형일 경우
                {
                    myScore -= 1;
                }
                else if (myTart.toppingList[i].toppingSize ==1)//중형일 경우
                {
                    myScore -= 2;
                }
                else if (myTart.toppingList[i].toppingSize ==2)//대형일 경우
                {
                    myScore -= 4;
                }
            }
        }

        //}

    }

}
