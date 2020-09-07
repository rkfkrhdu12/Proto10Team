﻿using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ButtonPro : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler,
    IPointerClickHandler, IPointerUpHandler
{
    [Serializable]
    public class FuncEvent
    {
        public UnityEvent onEnter;

        public UnityEvent onExit;

        public UnityEvent onDown;

        public UnityEvent onPress;

        public UnityEvent onUp;

        public UnityEvent onClick;
    }

    #region Enum

    private enum BtnState
    {
        NORMAL,
        PRESS,
        SELECT
    }

    #endregion

    #region Show Inspector

    [Header("Default Image")]
    [SerializeField, Tooltip("기본 상태일 때")]
    private Sprite normalImage;

    [Header("Option")]
    [SerializeField, Tooltip("마우스가 닿았을 때")]
    private Sprite reachImage;

    [SerializeField, Tooltip("눌렸을 때")]
    private Sprite pressImage;

    [SerializeField, Tooltip("선택되었을 때")]
    private Sprite selectImage;

    [Space]
    public FuncEvent onButtonEvent;

    #endregion

    #region Hide Inspector

    //이미지 변수
    private Image image;

    //버튼의 상태에 대한 변수
    private BtnState _btnState = BtnState.NORMAL;

    //그룹화가 형태인 버튼인지 아닌지 처리
    [HideInInspector]
    public ButtonGroup _buttonGroup;

    //그룹화로 만들시, 선택권이 있는 버튼
    [HideInInspector]
    public bool isSelected;

    //버튼을 누르고 있는 중인지 체크
    private bool isButtonDown;

    //버튼에 마우스가 닿고 있는지 체크
    private bool isButtonReach;

    #endregion

    private void Awake()
    {
        image = GetComponent<Image>();

        // if (normalImage == null)
        //     throw new NullReferenceException("NormalImage is required.");
    }

    private void Start()
    {
        if (normalImage != null)
            image.sprite = normalImage;
    }

    //버튼을 누름
    public void OnPointerDown(PointerEventData eventData)
    {
        //왼쪽 마우스를 누른 경우에만 해당
        if (eventData.button != PointerEventData.InputButton.Left) return;

        //누르고 있는 중 처리
        onPressButton();

        isButtonDown = true;

        //버튼을 누르는 최초, 동작합니다.
        onButtonEvent.onDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isButtonDown = false;
        onButtonEvent.onUp?.Invoke();
    }

    private void Update()
    {
        //선택 상태가 아닐경우 : return
        if (!isSelected)
        {
            if (!isButtonDown)
            {
                onNotSelectButton();

                if (isButtonReach)
                    if (reachImage != null)
                        image.sprite = reachImage;
            }
        }
        else
        {
            if (!isButtonDown)
            {
                //그룹이 할당이 되어 있지 않을 때, 버튼을 땟을 시 : Not Select
                if (_buttonGroup == null)
                    onNotSelectButton();
                else
                {
                    if (selectImage != null)
                        image.sprite = selectImage;
                }
            }
        }

        if (isButtonDown)
            onButtonEvent.onPress?.Invoke();
    }

    //버튼을 땜
    public void OnPointerClick(PointerEventData eventData)
    {
        //왼쪽 마우스를 누른 경우에만 해당
        if (eventData.button != PointerEventData.InputButton.Left) return;

        //할당이 되어 있다면, 몇번째 버튼이 선택됬는지 버튼 그룹에게 알려준다.
        if (_buttonGroup != null)
        {
            //현재 선택된 버튼이 자신임을 변경한다.
            _buttonGroup.SelectedNumber = ButtonGroup.ButtonSearch(_buttonGroup.buttonPros, this);

            //데이터 변경 처리
            _buttonGroup.notifyDataSetChanged();
        }

        //그룹이 할당이 되어 있지 않을 때, 버튼을 땟을 시 : Not Select
        if (_buttonGroup == null)
            onNotSelectButton();
        else
        {
            isSelected = true;

            if (selectImage != null)
                image.sprite = selectImage;
        }

        //버튼을 땟을 시, 동작합니다.
        onButtonEvent.onClick?.Invoke();
    }

    //버튼 영역에 닿음
    public void OnPointerEnter(PointerEventData eventData)
    {
        isButtonReach = true;
        onButtonEvent.onEnter?.Invoke();
    }

    //버튼 영역에서 나감
    public void OnPointerExit(PointerEventData eventData)
    {
        isButtonReach = false;
        onButtonEvent.onExit?.Invoke();
    }

    /// <summary>
    /// 누르고 있는 상태로 전환합니다.
    /// </summary>
    public void onPressButton()
    {
        if (pressImage != null)
            image.sprite = pressImage;

        _btnState = BtnState.PRESS;
    }

    public void onSelectedButton()
    {
        isSelected = true;

        if (selectImage != null)
            image.sprite = selectImage;

        _btnState = BtnState.SELECT;
    }

    /// <summary>
    /// 해당 버튼을 일반 상태로 전환합니다.
    /// </summary>
    public void onNotSelectButton()
    {
        //풀림
        if (normalImage != null)
            image.sprite = normalImage;

        _btnState = BtnState.NORMAL;

        //버튼 그룹이 할당됬을 시 : Not Select
        if (_buttonGroup != null)
            isSelected = false;
    }

    public void Reset()
    {
        isButtonDown = false;
        isButtonReach = false;
    }
}