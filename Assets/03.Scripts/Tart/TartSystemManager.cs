using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
public class TartSystemManager : MonoBehaviour
{
    /*TartSystemManager : 
     * TartManager, TartSettingManager, ScoreManager 등을 통합(?)하지는 않았고...
     * 그냥 한꺼번에 쓰기 위해서 만든 녀석...
     */
    public GameObject answerTartObj;

    public GameObject answerTartRealPos;
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

    private Vector3 tartManagerMovePos;
    private Vector3 answerTartMoveRealPos;

    [Header("랜덤으로 나올 타르트의 개수")]
    public int tartVal;
    private void Awake()
    {
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
    }
    void Start()
    {
        Init();
    }

    public void RandomChoiceOfTart()
    {
        DontDestroyOnLoad(teamRedTart.gameObject);
        DontDestroyOnLoad(teamBlueTart.gameObject);
        tartManager.gameObject.transform.position = Vector3.zero;
        answerTartObj.transform.position = Vector3.zero;
        gameObject.transform.position = Vector3.zero;
        int randVal = Random.Range(1, tartVal+1);
        tartManager.DataLoadAndSetAnswerTart(randVal);
        tartSettingManager.AnswerTartSetting();
        tartManager.gameObject.transform.position = new Vector3(answerTartRealPos.transform.position.x, answerTartRealPos.transform.position.y + 1f, answerTartRealPos.transform.position.z);
        answerTartObj.transform.position= new Vector3(answerTartRealPos.transform.position.x, answerTartRealPos.transform.position.y, answerTartRealPos.transform.position.z);
    }

    public void SceneAndTartMove()
    {

        PhotonNetwork.LoadLevel(3);
 
        tartManager.gameObject.transform.position = new Vector3(answerTartRealPos.transform.position.x, answerTartRealPos.transform.position.y + 1f, answerTartRealPos.transform.position.z);
        answerTartObj.transform.position = new Vector3(answerTartRealPos.transform.position.x, answerTartRealPos.transform.position.y, answerTartRealPos.transform.position.z);

    }
    public void SpawnToppings()
    {
        toppingSpawnManager.InitAndGoSpawnTopping();
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
