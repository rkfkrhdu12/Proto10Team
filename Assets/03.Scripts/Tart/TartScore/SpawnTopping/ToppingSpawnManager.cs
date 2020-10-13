using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

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
    List<Topping> answerToppingList;

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

    // Start is called before the first frame update
    private void Awake()
    {

    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

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
                else if (answerToppingList[bonusCheck[i]] == answerToppingList[bonIndexTemp])//같으면
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
}
