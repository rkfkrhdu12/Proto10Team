using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

using TMPro;

public class Timer : MonoBehaviour
{
    float _remainingTime = 190;

    bool _isStart = false;

    [SerializeField]
    TMP_Text _timeText = null;

    int _eventCount = 0;
    int[] _eventTime = new int[_eventCountCountDown + _eventCountRecipe + _eventCountItem + _eventCountFever + _eventCountTimeOut];
    /// <summary>
    /// _eventTime[_eventCount]
    /// </summary>
    int _curEventTime { get { return _eventCount >= _eventTime.Length ? 0 : _eventTime[_eventCount]; } }

    const int _eventCountCountDown = 4;
    const int _eventCountRecipe = 1;
    const int _eventCountItem = 5;
    const int _eventCountFever = 1;
    const int _eventCountTimeOut = 1;

    InGameManager _ingameMgr;

    delegate void ItemEvent();
    Queue<ItemEvent> ItemEvents = new Queue<ItemEvent>();

    [SerializeField]
    InGameUIController _uiContoller = null;

    TartSystemManager _tartManager;

    public void OnStart()
    {
        LogManager.Log("Timer On !");

        _remainingTime = 184;
        _isStart = true;
        _tartManager = TartSystemManager.Instance;

        _tartManager.RandomChoiceOfTart();
    }

    private void Awake()
    {
        int count = -1;
        _eventTime[++count] = 183;
        _eventTime[++count] = 182;
        _eventTime[++count] = 181;
        _eventTime[++count] = 180;
        _eventTime[++count] = 170;
        _eventTime[++count] = 120;
        _eventTime[++count] = 90;
        _eventTime[++count] = 60;
        _eventTime[++count] = 30;
        _eventTime[++count] = 20;
        _eventTime[++count] = 0;

        ItemEvent OnEvent = new ItemEvent(OnEventCallCountDown);
        for (int i = 0; i < _eventCountCountDown; ++i)
            ItemEvents.Enqueue(OnEvent);

        ItemEvents.Enqueue(OnEventCallRecipe);

        OnEvent = new ItemEvent(OnEventCallItem);
        for (int i = 0; i < _eventCountItem; ++i)
            ItemEvents.Enqueue(OnEvent);

        ItemEvents.Enqueue(OnEventCallFever);
        ItemEvents.Enqueue(OnEventCallTimeOut);
    }

    private void Start()
    {
        _ingameMgr = GameManager.Instance.InGameManager;
    }

    void Update()
    {
        if ((!_isStart || _ingameMgr.IsTimeOut)) { return; }

        _remainingTime = Mathf.Max(_remainingTime - Time.deltaTime, 0.0f);
        _timeText.text = ((int)_remainingTime).ToString();

        if ((int)_remainingTime == _curEventTime && _eventCount < _eventTime.Length)
        {
            Debug.Log(_curEventTime);

            (ItemEvents.Dequeue())();
        }
    }

    void OnEventCallCountDown()
    {
        if (_uiContoller == null) { return; }

        if (_curEventTime == 180)
        {
            _tartManager.SpawnToppings();
            _uiContoller.OnStartUI();
        }
        else
            _uiContoller.OnCountDownUI();

        ++_eventCount;
    }

    void OnEventCallRecipe()
    {
        LogManager.Log("OnEventCallRecipe");

        ++_eventCount;
    }

    void OnEventCallItem()
    {
        if (_ingameMgr == null) _ingameMgr = GameManager.Instance.InGameManager;

        _ingameMgr.OnItemEvent();

        ++_eventCount;
    }

    void OnEventCallFever()
    {
        if (_ingameMgr == null) _ingameMgr = GameManager.Instance.InGameManager;
        if (_uiContoller == null) { return; }

        _uiContoller.OnFeverUI();

        _ingameMgr.OnFever();
        ++_eventCount;
    }

    void OnEventCallTimeOut()
    {
        if (_ingameMgr == null) _ingameMgr = GameManager.Instance.InGameManager;
        if (_uiContoller == null) { return; }

        _uiContoller.OnTimeOutUI();
        _ingameMgr.OnTimeOut();

        _tartManager.SceneAndTartFix();
        ++_eventCount;
    }
}
