using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TartSystemManager : MonoBehaviour
{
    /*TartSystemManager : 
     * TartManager, TartSettingManager, ScoreManager 등을 통합(?)하지는 않았고...
     * 그냥 한꺼번에 쓰기 위해서 만든 녀석...
     */
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
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {

    }

    void Update()
    {

    }

    public void RandomChoiceOfTart()
    {
        int randVal = Random.Range(1, 5);
        tartManager.DataLoadAndSetAnswerTart(randVal);
        tartSettingManager.AnswerTartSetting();
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
