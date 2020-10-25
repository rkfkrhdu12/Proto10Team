using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class Topping : MonoBehaviour
{
    [SerializeField]
    public ToppingData Data;

    public int toppingCode { get { return Data.toppingCode; } set { Data.toppingCode = value; } }
    public int toppingSize { get { return Data.toppingSize; } set { Data.toppingSize = value; } }

    public int toppingNum { get { return Data.toppingNum; } set { Data.toppingNum = value; } }

    public int toppingType { get { return Data.toppingType; } set { Data.toppingType = value; } }

    public float toppingScore { get { return Data.toppingScore; } set { Data.toppingScore = value; } }

    public bool isCheck { get { return Data.isCheck; } set { Data.isCheck = value; } }

    public float answerPosX { get { return Data.answerPosX; } set { Data.answerPosX = value; } }
    public float answerPosY { get { return Data.answerPosY; } set { Data.answerPosY = value; } }
    public float answerPosZ { get { return Data.answerPosZ; } set { Data.answerPosZ = value; } }

    [Tooltip("현재 토핑의 위치입니다. Update에서 계속 업데이트됨.")]
    public Vector2 nowToppingPos;

    //private void Awake()
    //{
    //    isCheck = false;
    //}

    private void Update()
    {
        nowToppingPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
    }

    public void SetToppingInfo(int code, int size, int num, int type, float score, float x, float y, float z)//토핑의 여러가지 정보들을 Set
    {
        Data.thisObject = gameObject;

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
