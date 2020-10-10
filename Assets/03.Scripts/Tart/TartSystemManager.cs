using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TartSystemManager : MonoBehaviour
{
    /*TartSystemManager : 
     * TartManager, TartSettingManager, ScoreManager 등을 통합(?)하지는 않았고...
     * 그냥 한꺼번에 쓰기 위해서 만든 녀석...
     */

    public int teamRedScore;
    public int teamBlueScore;

    public Tart teamRedTart;
    public Tart teamBlueTart;

    public TartManager tartManager;
    public TartSettingManager tartSettingManager;
    public ToppingSpawnManager toppingSpawnManager;
    public ScoreManager scoreManager;

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
        Debug.Log("CheckBothScoreRed : " + teamRedScore);

        scoreManager.myTart = teamBlueTart;
        CheckScore();
        teamBlueScore = scoreManager.myScore;
        Debug.Log("CheckBothScore : " + teamBlueScore);

        Debug.Log(" TeamRed : " + teamRedScore + " | TeamBlue : " + teamBlueScore);

    }
    void Start()
    {

    }

    void Update()
    {

    }
}
