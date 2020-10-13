using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class TartSystemManager : MonoBehaviour
{
    /*TartSystemManager : 
     * TartManager, TartSettingManager, ScoreManager 등을 통합(?)하지는 않았고...
     * 그냥 한꺼번에 쓰기 위해서 만든 녀석...
     */
    public GameObject answerTartObj;

    public GameObject answerTartRealPos;
    public GameObject redTeamTartMovePos;
    public GameObject blueTeamTartMovePos;

    private static TartSystemManager _instance;

    public static TartSystemManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(TartSystemManager)) as TartSystemManager;

                if (_instance == null)
                    LogManager.Log("없어....");
            }
            return _instance;
        }
    }
    public int teamRedScore;
    public int teamBlueScore;

    public Tart teamRedTart;
    public Tart teamBlueTart;

    public TartManager tartManager;
    public TartSettingManager tartSettingManager;
    public ToppingSpawnManager toppingSpawnManager;
    public ScoreManager scoreManager;

    private int endingSceneIndex;
    //private Vector3 tartManagerMovePos;
    //private Vector3 answerTartMoveRealPos;



    [Header("랜덤으로 나올 타르트의 개수")]
    public int tartVal;
    private void Awake()
    {
        Init();
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void Init()
    {

        if (answerTartRealPos == null)
        {
            LogManager.Log("answerTartRealPos 없음");
        }

        if (teamRedTart == null)
        {
            teamRedTart = GameObject.FindGameObjectWithTag("TeamRedTart").GetComponent<Tart>();
        }
        if (teamBlueTart == null)
        {
            teamBlueTart = GameObject.FindGameObjectWithTag("TeamBlueTart").GetComponent<Tart>();
        }

        if (answerTartObj == null)
        {
            answerTartObj = GameObject.FindGameObjectWithTag("AnswerTart");
        }

    }
    void Start()
    {
        endingSceneIndex = UnityEngine.SceneManagement.SceneManager.GetSceneByName("EndingScene").buildIndex;
        //LogManager.Log(endingSceneIndex);
        DontDestroyOnLoad(teamRedTart.gameObject);
        DontDestroyOnLoad(teamBlueTart.gameObject);
        Init();
        //StartCoroutine(TartSystemTest());
    }

    public IEnumerator TartSystemTest()
    {
        RandomChoiceOfTart();
        LogManager.Log("타르트 세팅 완료...2초 뒤 씬 이동.");
        yield return new WaitForSecondsRealtime(10f);
        SceneAndTartFix();
        //LogManager.Log("이동 완료. 2초 뒤 정답 타르트 위치 이동한다...");
        //yield return new WaitForSecondsRealtime(2f);
        //AnswerTartMove();
        //LogManager.Log("이동 완료. 팀 타르트 위치 이동한다...");
        //TeamTartMove();
        //LogManager.Log("이동 완료. 어떰??");

    }
    public void RandomChoiceOfTart()
    {

        //tartManager.gameObject.transform.position = Vector3.zero;
        //answerTartObj.transform.position = Vector3.zero;
        //gameObject.transform.position = Vector3.zero;
        //int randVal = Random.Range(1, tartVal + 1);
        int randVal = 2;
        tartManager.DataLoadAndSetAnswerTart(randVal);
        tartSettingManager.AnswerTartSetting();
        AnswerTartMove();

    }

    public void SceneAndTartFix()
    {
        teamRedTart.InputToppingToChildObject();
        teamBlueTart.InputToppingToChildObject();
        PhotonNetwork.LoadLevel(3);
    }

    public void SpawnToppings()
    {
        toppingSpawnManager.InitAndGoSpawnTopping();
    }
    public void TeamTartMove()
    {
        Vector3 redPos = redTeamTartMovePos.transform.position;
        Vector3 bluePos = blueTeamTartMovePos.transform.position;
        teamRedTart.transform.position = new Vector3(redPos.x, redPos.y, redPos.z);
        teamBlueTart.transform.position = new Vector3(bluePos.x, bluePos.y, bluePos.z);
    }
    public void AnswerTartMove()
    {
        if (answerTartRealPos == null)
        {
            LogManager.Log("정답 타르트 위치가 업슴.");
            answerTartRealPos = GameObject.FindGameObjectWithTag("AnswerTartPos");
        }
        tartManager.gameObject.transform.position = new Vector3(answerTartRealPos.transform.position.x, answerTartRealPos.transform.position.y + 1f, answerTartRealPos.transform.position.z);
        answerTartObj.transform.position = new Vector3(answerTartRealPos.transform.position.x, answerTartRealPos.transform.position.y, answerTartRealPos.transform.position.z);

    }
    public void CheckScore()
    {

        scoreManager.CheckScoreV2();
    }

    public void CheckBothScore()
    {

        teamRedScore = 0;
        teamBlueScore = 0;
        scoreManager.myTart = teamRedTart;
        CheckScore();
        teamRedScore = scoreManager.myScore;
        LogManager.Log("CheckBothScoreRed : " + teamRedScore);

        scoreManager.myTart = teamBlueTart;
        CheckScore();
        teamBlueScore = scoreManager.myScore;
        LogManager.Log("CheckBothScore : " + teamBlueScore);

        LogManager.Log(" TeamRed : " + teamRedScore + " | TeamBlue : " + teamBlueScore);

    }

}
