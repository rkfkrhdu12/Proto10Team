using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Topping : MonoBehaviour
{
    [Tooltip("토핑의 코드")]
    public int toppingCode;
    [Tooltip("토핑의 사이즈. 0 = 소 , 1 = 중, 2 = 대")]
    public int toppingSize;

    [Tooltip("사이즈에 따른 토핑의 종류. 문서 참고...")]
    public int toppingNum;

    [Tooltip("각 토핑별로 있을 수 있는 타입들...문서 참고")]
    public int toppingType;

    [Tooltip("토핑이 가지고 있는 점수(완성도)")]
    public float toppingScore;

    public bool isCheck; //채점 때 사용함. 검사가 완료된 토핑인지?

    //정답 토핑일 경우에만 사용함...
    public Vector3 answerPos;
    public float answerPosX;
    public float answerPosY;
    public float answerPosZ;

    [Tooltip("현재 토핑의 위치입니다. Update에서 계속 업데이트됨.")]
    public Vector2 nowToppingPos;

    private void Awake()
    {
        isCheck = false;
    }

    private void Update()
    {
        nowToppingPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
    }

    public void SetToppingInfo(int code, int size, int num, int type, float score, float x, float y, float z)//토핑의 여러가지 정보들을 Set
    {        

        toppingCode = code;
        toppingSize = size;
        
        toppingNum = num;
        
        toppingType = type;

        toppingScore = score;

        answerPosX = x;
        answerPosY = y;
        answerPosZ = z;
    }


}
