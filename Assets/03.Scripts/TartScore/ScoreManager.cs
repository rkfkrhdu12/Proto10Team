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

    [Header("토핑의 채점 범위")]
    [Range(0, 30)]
    public float lastCheckDis;


    [Header("채점할 때의 배율")]
    [Range(-1, 5)]
    public float lastCheckPer = 0.1f;
    void Start()
    {
        CheckScore();
    }

    void Update()
    {

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

        //if (nowTopping.nowToppingPos.x> nowTart.limitPos1.gameObject.transform.position.x &&
        //    nowTopping.gameObject.transfo > nowTart.limitPos1.gameObject.transform.position.z &&
        //    nowTopping.gameObject.transform.position.x < nowTart.limitPos2.gameObject.transform.position.x &&
        //    nowTopping.gameObject.transform.position.x < nowTart.limitPos2.gameObject.transform.position.z)
        //{//y축의 정보는 사용하지 않음...
        //    return false;
        //}
        //else
        //{
        //    return true;
        //}


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



    public void CheckScore()
    {

        Tart answerTart = tartManager.answerTart;
        //while (true)
        //{

        // 정답 타르트의 토핑들 중 하나를 가져온다.
        //nowAnswerTopping = tartManager.answerTart.toppingList[checkVal];

        // 다음.정답 타르트 토핑(이하 정토)과 우리팀 타르트 토핑(이하 우토)을 비교해야한다.
        //비교 전, 그래도 타르트라고 인정해주는 범위를 벗어난 우토는 리스트에서 제거해야한다...
        for (int i = 0; i < myTart.toppingList.Count; i++)
        {
            if (IsToppingOutOfRange(myTart, myTart.toppingList[i])) //만약 범위 바깥에 있으면...
            {
                myTart.toppingList.RemoveAt(i); //제거.
            }
        }
        //Debug.Log("남은 갯수 : " + myTart.toppingList.Count);
        //제거를 완료했으면 이제 채점을 해야한다...
        //정토에다가 우토를 하나하나 다 대조하는 방법밖에 떠오르지 않기 때문에, 그것을 한다...
        for (int answerVal = 0; answerVal < answerTart.toppingList.Count; answerVal++)
        {
            //Debug.Log("현재 앤서타르트의 토핑 개수" + answerTart.toppingList.Count);
            int[] nowCheckArr = new int[24]; // +1의 해결책으로 나온 배열. 이 배열은 answerVal이 바뀔 때마다 초기화 된다.
            int nowCheckVal = 0; // nowCheckArr의 인덱스를 위해서 만든 변수.

            for (int myVal = 0; myVal < myTart.toppingList.Count; myVal++)
            {
                if (
                    //우선 토핑 사이즈가 같아야하고,
                    answerTart.toppingList[answerVal].toppingSize == myTart.toppingList[myVal].toppingSize &&
                    //토핑 넘도 같아야 한다...
                    answerTart.toppingList[answerVal].toppingNum == myTart.toppingList[myVal].toppingNum &&
                    //마지막으로 토핑 타입까지 같으면 일단 같은 토핑이라고 판별 완료
                    answerTart.toppingList[answerVal].toppingType == myTart.toppingList[myVal].toppingType&&
                    //진짜 진짜 마지막으로 얘를 검사한 적도 없어야함
                    myTart.toppingList[myVal].isCheck == false
                    )
                {
                    //같은 토핑이 한 3개정도 있다고 쳐보자. 방금 찾은 한 녀석 말고도 다른 녀석들을 찾으러 가야한다.
                    //다른 녀석들을 찾으러 가기 전에, 이 녀석을 간략하게라도 저장해놓을 무언가가 필요하다...
                    // +1 그래서 배열 하나로 myVal에 해당하는걸 저장해놓기로 했다. 

                    nowCheckArr[nowCheckVal] = myVal; // myVal을 저장해놓는다.
                    Debug.Log("들어간 오브젝트 이름 : " + myTart.toppingList[nowCheckArr[nowCheckVal]].gameObject.name);
                    nowCheckVal += 1;//얘도 1 더해주고...
                    Debug.Log("nowCheckVal = " + nowCheckVal);


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
                Debug.Log("최종 검사 대상 정하기의 NowCheckVal = " + nowCheckVal);
                //우타의 우토 위치를 만들어놓고
                Vector2 nowMyToppingPos =
                    new Vector2(myTart.toppingList[nowCheckArr[nowCheckVal]].gameObject.transform.position.x,
                    myTart.toppingList[nowCheckArr[nowCheckVal]].gameObject.transform.position.z);

                nowDis = Vector2.Distance(nowAnswerToppingPos, nowMyToppingPos);

                //Distance 계산하기
                if (nearDis > nowDis) // 현재 '가장 가까운 거리'보다 더 가깝다면?
                {
                    nearDis = nowDis; //갱신해주고
                    Debug.Log("nearDis = " + nearDis);
                    lastCheckVal = nowCheckArr[nowCheckVal];//얘도 이렇게 해준다.
                }

            }
            //이렇게 lastCheckVal이 정해졌으면, 진짜 마지막으로 거리에 따른 점수를 부여한다.


            //문제1 발생...
            //물론 크기상으로 1/2/3이라는 기준은 있지만, 그건 모델링할때 이야기고, 유니티에서 불러와봤자 다 똑같이 스케일은 1일 것이다.
            //일단은 3이하만 점수 주고 0.5이하는 그냥 점수 다 주는걸로...

            Debug.Log("에러 뜨기 전, myTart.toppingList.Count : " + myTart.toppingList.Count);
            if (myTart.toppingList.Count == 0)
            {
                break;
            }

            Vector2 nowLastMyToppingPos =
             new Vector2(myTart.toppingList[lastCheckVal].gameObject.transform.position.x,
              myTart.toppingList[lastCheckVal].gameObject.transform.position.z);

            float lastDis = Vector2.Distance(nowAnswerToppingPos, nowLastMyToppingPos);
            Debug.Log("LastCheckVal = " + lastCheckVal);
            Debug.Log("현재 점수를 주려고 하는 토핑 : " + myTart.toppingList[lastCheckVal].toppingSize + "|" + myTart.toppingList[lastCheckVal].toppingNum);
            Debug.Log("이름 : " + myTart.toppingList[lastCheckVal].gameObject.name + "의 distance : " + lastDis);

            //Debug.Log("내타르트토핑리스트의 카운트 : " + myTart.toppingList.Count);
            if (lastDis < lastCheckDis)//보다 안에 있냐?
            {
                myTart.toppingList[lastCheckVal].isCheck = true; //채점을 했다는 표시로 true를 해준다.
                Debug.Log("내타르트 토핑리스트 lastCheckVal의 isCheck : " + myTart.toppingList[lastCheckVal].isCheck);
                if (lastDis < 0.5)//0.5보다 안에 있냐?
                {

                    if (answerTart.toppingList[answerVal].toppingSize == myTart.toppingList[lastCheckVal].toppingSize && answerTart.toppingList[answerVal].toppingNum == myTart.toppingList[lastCheckVal].toppingNum && answerTart.toppingList[answerVal].toppingType == myTart.toppingList[lastCheckVal].toppingType)
                    {
                        Debug.Log("같음.");
                        float tempScore = answerTart.toppingList[answerVal].toppingScore;// / lastDis;
                        myScore += tempScore;
                        Debug.Log("이름 : " + myTart.toppingList[lastCheckVal].gameObject.name + "이 만점지급 되었습니다.");
                        Debug.Log("TempScore : " + tempScore);
                        Debug.Log("현재 점수 : " + myScore);
                    }
                    else
                    {
                        Debug.Log("안같음.continue");
                        Debug.Log("남아있는 녀석이 생겼습니다." + myTart.toppingList[lastCheckVal].toppingSize + "|" + myTart.toppingList[lastCheckVal].toppingNum);
                        Debug.Log("이름 : " + myTart.toppingList[lastCheckVal].gameObject.name);
                        if (myTart.toppingList[lastCheckVal].toppingSize == 1)//소형일 경우
                        {

                            myScore -= 1;
                        }
                        else if (myTart.toppingList[lastCheckVal].toppingSize == 2)//중형일 경우
                        {
                            myScore -= 2;
                        }
                        else if (myTart.toppingList[lastCheckVal].toppingSize == 3)//대형일 경우
                        {
                            myScore -= 4;
                        }

                        //continue;
                    }



                    myTart.toppingList.RemoveAt(lastCheckVal);
                    //리무브 앳...제대로 작동은 하나, 리스트 크기를 직접 건들기 때문에 오류 발생.

                    // myTart.toppingList[lastCheckVal].
                }
                else
                {

                    if (answerTart.toppingList[answerVal].toppingSize == myTart.toppingList[lastCheckVal].toppingSize && answerTart.toppingList[answerVal].toppingNum == myTart.toppingList[lastCheckVal].toppingNum && answerTart.toppingList[answerVal].toppingType == myTart.toppingList[lastCheckVal].toppingType)
                    {
                        Debug.Log("같음.");
                        Debug.Log("이름 : " + myTart.toppingList[lastCheckVal].gameObject.name + "이 분할점 지급 되었습니다.");
                        float tempScore = answerTart.toppingList[answerVal].toppingScore;// / lastDis;
                        Debug.Log("TempScore : " + tempScore);
                        myScore += tempScore;
                        Debug.Log("현재 점수 : " + myScore);
                    }
                    else
                    {
                        Debug.Log("안같음.continue");
                        Debug.Log("남아있는 녀석이 생겼습니다." + myTart.toppingList[lastCheckVal].toppingSize + "|" + myTart.toppingList[lastCheckVal].toppingNum);
                        Debug.Log("이름 : " + myTart.toppingList[lastCheckVal].gameObject.name);
                        if (myTart.toppingList[lastCheckVal].toppingSize == 1)//소형일 경우
                        {

                            myScore -= 1;
                        }
                        else if (myTart.toppingList[lastCheckVal].toppingSize == 2)//중형일 경우
                        {
                            myScore -= 2;
                        }
                        else if (myTart.toppingList[lastCheckVal].toppingSize == 3)//대형일 경우
                        {
                            myScore -= 4;
                        }
                        //continue;
                    }


                    myTart.toppingList.RemoveAt(lastCheckVal);
                }
            }
            else
            {//스코어 변동 없음.
                Debug.Log("점수 지급이 되지 않았습니다.");
                myScore += 0;
                myTart.toppingList[lastCheckVal].isCheck = true; //채점을 했다는 표시로 true를 해준다.
                myTart.toppingList.RemoveAt(lastCheckVal);
            }

        }
        //마지막으로, isCheck가 안된 녀석들(장애물같은 애들)을 구해서 완성도를 차감시키자.
        for (int i = 0; i < myTart.toppingList.Count; i++)
        {
            Debug.Log("완성도 차감 시의 isCheck : " + myTart.toppingList[i].isCheck);
            if (myTart.toppingList[i].isCheck == false)//검사가 안된 녀석이 있다면?
            {
                Debug.Log("남아있는 녀석이 생겼습니다." + myTart.toppingList[i].toppingSize + "|" + myTart.toppingList[i].toppingNum);
                Debug.Log("이름 : " + myTart.toppingList[i].gameObject.name);
                if (myTart.toppingList[i].toppingSize == 1)//소형일 경우
                {

                    myScore -= 1;
                }
                else if (myTart.toppingList[i].toppingSize == 2)//중형일 경우
                {
                    myScore -= 2;
                }
                else if (myTart.toppingList[i].toppingSize == 3)//대형일 경우
                {
                    myScore -= 4;
                }
            }
        }

        // myScore = Mathf.Clamp(myScore, 0, 100);
        // Mathf.Max(0,1) > 1 
        //      .Min(0,1) > 0

    }
    public void CheckScore_2()
    {

        Tart answerTart = tartManager.answerTart;
        //while (true)
        //{

        // 정답 타르트의 토핑들 중 하나를 가져온다.
        //nowAnswerTopping = tartManager.answerTart.toppingList[checkVal];

        // 다음.정답 타르트 토핑(이하 정토)과 우리팀 타르트 토핑(이하 우토)을 비교해야한다.
        //비교 전, 그래도 타르트라고 인정해주는 범위를 벗어난 우토는 리스트에서 제거해야한다...
        for (int i = 0; i < myTart.toppingList.Count; i++)
        {
            if (IsToppingOutOfRange(myTart, myTart.toppingList[i])) //만약 범위 바깥에 있으면...
            {
                myTart.toppingList.RemoveAt(i); //제거.
            }

        }

        Debug.Log("남은 갯수 : " + myTart.toppingList.Count);
        //제거를 완료했으면 이제 채점을 해야한다...
        //정토에다가 우토를 하나하나 다 대조하는 방법밖에 떠오르지 않기 때문에, 그것을 한다...
        for (int answerVal = 0; answerVal < answerTart.toppingList.Count; answerVal++)
        {
            int[] nowCheckArr = new int[24]; // +1의 해결책으로 나온 배열. 이 배열은 answerVal이 바뀔 때마다 초기화 된다.
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
                    Debug.Log("nowCheckVal = " + nowCheckVal);
                    Debug.Log("들어간 오브젝트 이름 : " + myTart.toppingList[myVal].gameObject.name);

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
            Debug.Log("현재 점수를 주려고 하는 토핑 : " + myTart.toppingList[lastCheckVal].toppingSize + "|" + myTart.toppingList[lastCheckVal].toppingNum);
            Debug.Log("이름 : " + myTart.toppingList[lastCheckVal].gameObject.name + "의 distance : " + lastDis);
            //Debug.Log("내타르트토핑리스트의 카운트 : " + myTart.toppingList.Count);
            if (lastDis < lastCheckDis)//보다 안에 있냐?
            {
                myTart.toppingList[lastCheckVal].isCheck = true; //채점을 했다는 표시로 true를 해준다.
                Debug.Log("내타르트 토핑리스트 lastCheckVal의 isCheck : " + myTart.toppingList[lastCheckVal].isCheck);
                if (lastDis < 0.5)//0.5보다 안에 있냐?
                {
                    Debug.Log("이름 : " + myTart.toppingList[lastCheckVal].gameObject.name + "이 만점지급 되었습니다.");

                    //myTart.toppingList.RemoveAt(lastCheckVal);
                    //리무브 앳...제대로 작동은 하나, 리스트 크기를 직접 건들기 때문에 오류 발생.

                    // myTart.toppingList[lastCheckVal].
                }
                else
                {
                    Debug.Log("이름 : " + myTart.toppingList[lastCheckVal].gameObject.name + "이 분할점 지급 되었습니다.");
                    myScore += answerTart.toppingList[answerVal].toppingScore / lastDis; //나눠서 지급
                                                                                         // myTart.toppingList.RemoveAt(lastCheckVal);
                }
            }
            else
            {//스코어 변동 없음.
                Debug.Log("점수 지급이 되지 않았습니다.");
                myScore += 0;
                myTart.toppingList[lastCheckVal].isCheck = true; //채점을 했다는 표시로 true를 해준다.
                                                                 //  myTart.toppingList.RemoveAt(lastCheckVal);

            }


        }
        //마지막으로, isCheck가 안된 녀석들(장애물같은 애들)을 구해서 완성도를 차감시키자.
        for (int i = 0; i < myTart.toppingList.Count; i++)
        {
            Debug.Log("완성도 차감 시의 isCheck : " + myTart.toppingList[i].isCheck);
            if (myTart.toppingList[i].isCheck == false)//검사가 안된 녀석이 있다면?
            {
                Debug.Log("남아있는 녀석이 생겼습니다." + myTart.toppingList[i].toppingSize + "|" + myTart.toppingList[i].toppingNum);
                Debug.Log("이름 : " + myTart.toppingList[i].gameObject.name);
                if (myTart.toppingList[i].toppingSize == 1)//소형일 경우
                {

                    myScore -= 1;
                }
                else if (myTart.toppingList[i].toppingSize == 2)//중형일 경우
                {
                    myScore -= 2;
                }
                else if (myTart.toppingList[i].toppingSize == 3)//대형일 경우
                {
                    myScore -= 4;
                }
            }
        }

        // myScore = Mathf.Clamp(myScore, 0, 100);
        // Mathf.Max(0,1) > 1 
        //      .Min(0,1) > 0

    }

}
