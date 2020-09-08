using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ScrollController : MonoBehaviour
{
    // 스크롤바의 핸들
    [SerializeField]
    private RectTransform Handle;

    // 스크롤바의 게이지바
    [SerializeField]
    private Image Gage;

    // 해당 스크롤의 최소값
    [SerializeField]
    private int MinValue;
    // 해당 스크롤의 최대값
    [SerializeField]
    private int MaxValue;

    // 현재 스크롤의 값
    [SerializeField]
    private float _curValue;

    // Script 용 Public 변수
    [HideInInspector]
    public float Value { get { return _curValue; } }

    // 스크롤의 최소값이 1이상일 경우 -1 이상의 값을 가져 게이지바의 fillAmount를 조정할 때 쓰이는 변수
    private float _gap = 0.0f;
    // 핸들의 첫(100%)위치를 X의 변수
    private float _defaultPosX = 0.0f;
    // 핸들의 첫위치의 Vector3 변수
    private Vector3 _handlePos = Vector3.zero;

    private void Awake()
    {
        _gap = 0 - MinValue;

        if (Handle != null)
        {
            _defaultPosX = Handle.localPosition.x;
            _handlePos = Handle.localPosition;
        }
    }

    public void UpdateHandle()
    {
        float horizontal = Input.GetAxis("Mouse X");

        _curValue = Mathf.Clamp(_curValue + horizontal, MinValue, MaxValue);

        Gage.fillAmount = (_curValue + _gap) / (MaxValue + _gap);

        if (Handle != null)
        {
            _handlePos.x = -(_defaultPosX - ((_defaultPosX * 2) / Gage.fillAmount));

            Handle.localPosition = _handlePos;
        }
    }
}