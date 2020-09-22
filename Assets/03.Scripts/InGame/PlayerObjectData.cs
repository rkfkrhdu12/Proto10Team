using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectData : MonoBehaviour
{
    [SerializeField]
    private GameObject _head;
    [SerializeField]
    private GameObject _leftHand;
    [SerializeField]
    private GameObject _rightHand;

    public GameObject Head      { get { return _head; } }
    public GameObject LeftHand  { get { return _leftHand; } }
    public GameObject RightHand { get { return _rightHand; } }

    public void Active() => gameObject.SetActive(true);
}
