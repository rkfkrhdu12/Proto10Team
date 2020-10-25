using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tart : MonoBehaviour
{
    [Tooltip("타르트별 코드입니다.")]
    public int tartCode;

    GameObject testObj;
    [Tooltip("해당 타르트의 토핑들의 리스트 입니다....")]
    public List<ToppingData> toppingList = new List<ToppingData>();

    [Header("그래도 타르트라고 인정이 되는 범위를 정합니다.")]
    [Tooltip("왼쪽 아래.")]
    public GameObject limitPos1;
    [Tooltip("오른쪽 위.")]
    public GameObject limitPos2;

    public GameObject toppingGroupObj;

    private void Awake()
    {
        toppingList.Clear();
    }

    public bool isFixed;
    public void Init()
    {
        isFixed = false;

    }

    public void InputToppingToChildObject()
    {

        //if (toppingList[0] != null)
        //{
        //    for (int i = 0; i < toppingList.Count; i++)
        //    {
        //        toppingList[i].transform.SetParent(toppingGroupObj.transform);
        //        toppingList[i].GetComponent<Rigidbody>().isKinematic = true;
        //    }
        //    isFixed = true;
        //}
       
        // 가비지컬렉터 생성 X. 속도 비슷하대
        //foreach(Topping i in toppingList)
        //{
        //    i.transform.SetParent(toppingGroupObj.transform);
        //    i.GetComponent<Rigidbody>().isKinematic = true;
        //}
 

    }
    public void AddTopping(ToppingData topping) //토핑을 추가합니다.
    {
        if (!isFixed)
        {
            toppingList.Add(topping);

        }
    }

    public void RemoveTopping(ToppingData topping)
    {
        if (!isFixed)
        {
            toppingList.Remove(topping);
        }
    }

    public void ClearTopping()
    {
        toppingList.Clear();
    }

    readonly string _ToppingTagKey = "Topping";

    private void OnTriggerEnter(Collider other)
    {
        //Topping top = other.GetComponent<Topping>();
        //if (top != null)
        //{ // 토핑

        //}
        if (other.CompareTag(_ToppingTagKey) && !isFixed)
        { //지정된 태그의 오브젝트만 True
            AddTopping(other.gameObject.GetComponent<Topping>().Data);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_ToppingTagKey) && !isFixed)
        {
            RemoveTopping(other.gameObject.GetComponent<Topping>().Data);
        }
    }

}
