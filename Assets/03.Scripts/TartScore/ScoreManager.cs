using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Tooltip("타르트매니저 하나 만들어서 넣어주셔야 함")]
    public TartManager tartManager; // 타르트매니저. 정답 타르트!!의 정보가 들어있습니다.

    public Tart myTart; // 우리 팀이 만든 타르트입니다.

    public int myScore; //우리 팀의 점수(완성도)입니다.

    void Start()
    {
        TestTartInput();
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

        if (nowTopping.gameObject.transform.position.x>nowTart.limitPos1.gameObject.transform.position.x&&
            nowTopping.gameObject.transform.position.z>nowTart.limitPos1.gameObject.transform.position.z&&
            nowTopping.gameObject.transform.position.x<nowTart.limitPos2.gameObject.transform.position.x&&
            nowTopping.gameObject.transform.position.x<nowTart.limitPos2.gameObject.transform.position.z)
        {//y축의 정보는 사용하지 않음...
            return false;
        }
        else
        {
            return true;
        }
        
    }

    public void MarkScore()
    {

    }
    
}
