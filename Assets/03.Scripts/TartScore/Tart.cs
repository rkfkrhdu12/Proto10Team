using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tart : MonoBehaviour
{
    [Tooltip("타르트별 코드입니다.")]
    public int tartCode;

    GameObject testObj;
    [Tooltip("해당 타르트의 정답 토핑들의 리스트 입니다.")]
    public List<Topping> toppingList = new List<Topping>();


    [Tooltip("인게임 플레이 시에 사용되는 \n게임 오브젝트! 타르트에 들어있는\n게임 오브젝트! 토핑들의 리스트 입니다.")]
    public List<GameObject> toppingObjList = new List<GameObject>();


    [Header("그래도 타르트라고 인정이 되는 범위를 정합니다.")]    
    [Tooltip("왼쪽 아래.")]
    public GameObject limitPos1;
    [Tooltip("오른쪽 위.")]
    public GameObject limitPos2;

    public void AddTopping(Topping topping) //토핑을 추가합니다.
    {
        toppingList.Add(topping);
    }

    

}
