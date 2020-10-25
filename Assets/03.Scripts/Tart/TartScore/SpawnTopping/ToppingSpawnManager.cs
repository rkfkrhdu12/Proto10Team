using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

using System.Linq;

public class ToppingSpawnManager : MonoBehaviour
{

    /* 설계.
     * 1. 정답 타르트의 토핑리스트를 가져옴. (answerTart-> toppingList)
     * 2. 우선, 가져온 토핑 리스트를 랜덤(위치)으로 배치함
     * 3. 다 배치했으면, 토핑리스트에 있는 토핑들을 70% 정도 배치를 함.
     * 4. 6개의 불필요한 토핑들을 배치한다. 
     */

    // 토핑의 최대 갯수 : 25개(0~24)
    // 변동폭의 최대 갯수 : 12개(25-필요최소갯수13 = 12)
    // 랜덤 : 6개
    // 총 배치해야할 스폰 포인트 : 25+12+6 = 43개

    [Header("ToppingSpawnPoint (43)")]
    public GameObject[] toppingSpawnPoint;

    public GameObject testObj;

    [Header("타르트 매니저를 넣어주세요.")]
    public TartManager tartManager;

    [Header("타르트 세팅 매니저를 넣어주세요.")]
    public TartSettingManager tartSettingManager;

    /// <summary>
    /// 정답 타르트의 토핑리스트.
    /// </summary>
    List<ToppingData> answerToppingList;

    /// <summary>
    /// 정답 타르트의 토핑 갯수.
    /// </summary>
    int toppingCount;

    /// <summary>
    /// 해당 스폰 포인트가 사용 중인가(이미 배치되었는가) 체크.
    /// </summary>
    bool[] pointCheck;

    int[] bonusCheck;
    /// <summary>
    /// 필요 토핑을 제외한, 70%의 보너스 토핑들의 갯수.
    /// </summary>
    int toppingBonusCount;
    
    public void InitAndGoSpawnTopping()
    {
        Init();
        StartCoroutine(SpawnTopping());
    }
    /// <summary>
    /// 이것저것 초기화하고 설정함
    /// </summary>
    public void Init()
    {
        log(testObj.transform.childCount);
        for (int i = 0; i < testObj.transform.childCount; i++)
        {
            toppingSpawnPoint[i] = testObj.transform.GetChild(i).gameObject;
        }
        //정답 타르트의 토핑리스트를 가져옴.
        answerToppingList = tartManager.answerTart.toppingList;

        //정답 타르트의 토핑 갯수.
        toppingCount = answerToppingList.Count;

        toppingBonusCount = 0;

        pointCheck = new bool[43];
        for (int i = 0; i < pointCheck.Length; i++) //초기화
        {
            pointCheck[i] = false;
        }
    }
    /// <summary>
    /// 사용 가능한 스폰 포인트를 0부터 42까지의 수로 리턴해줌.
    /// </summary>
    public int ReturnPossibleSpawonPoint()
    {

        while (true)
        {
            int returnInt = Random.Range(0, 43);
            if (pointCheck[returnInt] == true)//만약 이미 사용중이라면
            {
                continue;
            }
            else
            {
                return returnInt;
            }
        }
    }

    public void log(object l)
    {
        LogManager.Log("TartSpawnManager : " + l);
    }
    
    private IEnumerator SpawnTopping()
    {
        for (int i = 0; i < toppingCount; i++)
        {
            //토핑 오브젝트를 가져옴
            GameObject nowToppingObject = tartSettingManager.ReturnTopping(answerToppingList[i].toppingSize, answerToppingList[i].toppingNum, answerToppingList[i].toppingType);

            int topIndexTemp = ReturnPossibleSpawonPoint(); // 랜덤 위치 배정

            //토핑 오브젝트를 해당 위치에 복사
            GameObject tempToppingObj = Instantiate(nowToppingObject,
                                                    new Vector3(toppingSpawnPoint[topIndexTemp].transform.position.x, toppingSpawnPoint[topIndexTemp].transform.position.y + 100, toppingSpawnPoint[topIndexTemp].transform.position.z),
                                                    nowToppingObject.transform.rotation,transform);


            pointCheck[topIndexTemp] = true; // 해당 포인트 체크를 true(사용 중)으로 함.
            log(nowToppingObject.name + " 복사.");
            //yield return null;
            yield return new WaitForSecondsRealtime(0.05f);
        }
        log("타르트에 필요한 토핑 스폰 완료. 70% 스폰 시작");

        toppingBonusCount = (int)((float)toppingCount *0.7f); // 보너스 토핑의 개수를 정함.
        log("보너스 토핑의 개수 : " + toppingBonusCount);


        //보너스 체크 배열은 현재 보너스로 스폰된 토핑들의 'toppingList 시절 인덱스'을 가지고 있습니다.
        bonusCheck = new int[toppingBonusCount];
        for (int i = 0; i < bonusCheck.Length; i++)
        {
            bonusCheck[i] = -1;
        }

        yield break;

        //보너스 토핑 배치
        while (true)
        {
            if (toppingBonusCount <=0)
            {
                log("토핑 보너스 카운트 : " +toppingBonusCount +" break 합니다.");
                break;
            }

            int bonIndexTemp = Random.Range(0, answerToppingList.Count);
            bool isPossibleTopping = true;

            for (int i = 0; i < bonusCheck.Length; i++)
            {
                if (i == 0)//첫번째 루프는 무조건 그냥 배치.
                {
                    break;
                }

                if (bonusCheck[i] == -1)//오류 해결을 위해)
                {
                    break;
                }
                else if (answerToppingList[bonusCheck[i]].thisObject == answerToppingList[bonIndexTemp].thisObject)//같으면
                {
                    isPossibleTopping = false;
                }
            }

            if (isPossibleTopping)
            {
                //isPossibleTopping = true;
                GameObject nowToppingObject = tartSettingManager.ReturnTopping(answerToppingList[bonIndexTemp].toppingSize,
                    answerToppingList[bonIndexTemp].toppingNum,
                    answerToppingList[bonIndexTemp].toppingType);

                int topIndexTemp = ReturnPossibleSpawonPoint(); // 랜덤 위치 배정

                log("배정할 위치 :" + topIndexTemp);
                //토핑 오브젝트를 해당 위치에 복사
                GameObject tempToppingObj = Instantiate(nowToppingObject,
        new Vector3(toppingSpawnPoint[topIndexTemp].transform.position.x, toppingSpawnPoint[topIndexTemp].transform.position.y, toppingSpawnPoint[topIndexTemp].transform.position.z),
        nowToppingObject.transform.rotation);
                pointCheck[topIndexTemp] = true; // 해당 포인트 체크를 true(사용 중)으로 함.
                toppingBonusCount -= 1;
                log("보너스 토핑 배치 하나.");
                yield return new WaitForSecondsRealtime(0.05f);
                continue;

            }
            else
            {
                continue;
            }

        }
        log("보너스 토핑 배치 완료.");

        log("장애물 토핑 배치를 시작합니다.");

        for (int i = 0; i < 6; i++)
        {
            int tempSize = Random.Range(1, 4);
            int tempNum = 0;

            switch (tempSize)
            {
                case 1:
                    tempNum = Random.Range(1, 7);
                    break;
                case 2:
                    tempNum = Random.Range(1, 7);
                    break;
                case 3:
                    tempNum = Random.Range(1, 6);
                    break;

                default:
                    break;
            }
            int tempType = 0;


            GameObject nowToppingObject = tartSettingManager.ReturnTopping(tempSize, tempNum, tempType);

            int topIndexTemp = ReturnPossibleSpawonPoint(); // 랜덤 위치 배정

            log("장애물 토핑을 배정할 위치 :" + topIndexTemp);
            //토핑 오브젝트를 해당 위치에 복사
            GameObject tempToppingObj = Instantiate(nowToppingObject,
                                                    new Vector3(toppingSpawnPoint[topIndexTemp].transform.position.x, toppingSpawnPoint[topIndexTemp].transform.position.y, toppingSpawnPoint[topIndexTemp].transform.position.z),
                                                    nowToppingObject.transform.rotation);

        }

        log("장애물 토핑도 배치 완료. 스폰 완료!");
    }

    public int myScore;

    public float lastCheckDis = 2.0f;
    public float allowDis = 1.118f;

    public float CheckScoreV2(Tart myTart)
    {
        Init();
        Tart answerTart = tartManager.answerTart; //정답 타르트를 가져왔다.

        if (answerTart == null)
        {
            LogManager.Log("앤서타르트 없음,");
        }
        else
        {
            for (int i = 0; i < answerTart.toppingList.Count; i++)
            {
                //1. 정답 타르트의 토핑 리스트에서 토핑 하나를 뽑아낸다.
                ToppingData answerTopping = answerTart.toppingList[i];
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
                            log("내 타르트의 토핑 중 " + sameToppingIndex[sameToppingIndexVal] + "번째의 토핑인" + myTart.toppingList[j].thisObject.name + "과 비교하도록 합니다.");
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
                    float myPosX = myTart.toppingList[sameToppingIndex[k]].thisObject.transform.localPosition.x;
                    float myPosZ = myTart.toppingList[sameToppingIndex[k]].thisObject.transform.localPosition.z;

                    Vector2 tempMyPos = new Vector2(myPosX, myPosZ);
                    log("이 토핑의 위치는" + tempMyPos);
                    Vector2 tempAnswerPos = new Vector2(answerTopping.answerPosX, answerTopping.answerPosZ);
                    float compareDis = Vector2.Distance(tempAnswerPos, tempMyPos);
                    log("현재 가장 가까운 거리는 " + minDis + " 입니다.");
                    log(myTart.toppingList[sameToppingIndex[k]].thisObject.name + "의 거리는 " + compareDis + "입니다.");

                    if (minDis > compareDis)
                    {
                        log(myTart.toppingList[sameToppingIndex[k]].thisObject.name + "를 제일 가까운 토핑으로 등록합니다.");
                        minDis = compareDis;
                        lastToppingIndex = sameToppingIndex[k];
                    }
                }
                log("가까운 토핑 등록이 끝났습니다. 가장 가까운 토핑은 " + myTart.toppingList[lastToppingIndex].thisObject.name + " 입니다.");
                //거리값이 작은 녀석을 찾았을 것이다. 그렇다면 이제
                // 4. 그 가장 가까운 녀석을 채점완료(isCheck)표시하고, 거리별 점수를 지급한다.
                myTart.toppingList[lastToppingIndex].Check(); //채점 완료 표시.
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
        }

        if (myScore <= 0)
        {
            myScore = 0;
        }
        log("채점을 마쳤습니다. 총 점수는 " + myScore + "점 입니다.");
        return myScore;
    }
}
