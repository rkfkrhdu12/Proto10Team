using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TartSystemManager : MonoBehaviour
{
    /*TartSystemManager : 
     * TartManager, TartSettingManager, ScoreManager 등을 통합(?)하지는 않았고...
     * 그냥 한꺼번에 쓰기 위해서 만든 녀석...
     */

    public int TeamRedScore;
    public int TeamBlueScore;

    public Tart TeamRedTart;
    public Tart TeamBlueTart;

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

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
