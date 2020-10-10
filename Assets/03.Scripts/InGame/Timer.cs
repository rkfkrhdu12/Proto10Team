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
    int[] _eventTime = new int[_eventCountRecipe + _eventCountItem + _eventCountFever + _eventCountTimeOut];
    /// <summary>
    /// _eventTime[_eventCount]
    /// </summary>
    int _curEventTime { get { return _eventCount >= _eventTime.Length ? 0 : _eventTime[_eventCount]; } }

    const int _eventCountRecipe = 1;
    const int _eventCountItem = 5;
    const int _eventCountFever = 1;
    const int _eventCountTimeOut = 1;

    InGameManager _ingameMgr;

    delegate void ItemEvent();
    Queue<ItemEvent> ItemEvents = new Queue<ItemEvent>();

    public void OnStart()
    {
        LogManager.Log("Timer On !");

        _remainingTime = 190;
        _isStart = true;
    }

    private void Awake()
    {
        int count = -1;
        _eventTime[++count] = 180;
        _eventTime[++count] = 150;
        _eventTime[++count] = 120;
        _eventTime[++count] = 90;
        _eventTime[++count] = 60;
        _eventTime[++count] = 30;
        _eventTime[++count] = 20;
        _eventTime[++count] = 0;

        ItemEvents.Enqueue(OnEventCallRecipe);

        ItemEvent OnItemEvent = new ItemEvent(OnEventCallItem);
        for (int i = 0; i < _eventCountItem; ++i)
            ItemEvents.Enqueue(OnItemEvent);

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

        // Debug
        if (Input.GetKeyDown(KeyCode.F9))
            GameManager.Instance.PlayerCharacter.GetComponent<PhotonView>().RPC("OnDebugMode", RpcTarget.AllBuffered);
    }

    void OnEventCallRecipe()
    {
        LogManager.Log("OnEventCallRecipe");

        ++_eventCount;
    }

    void OnEventCallItem()
    {
        if (_ingameMgr == null) _ingameMgr = GameManager.Instance.InGameManager;

        LogManager.Log("OnEventCallItem");

        _ingameMgr.OnItemEvent(_eventCount);

        ++_eventCount;
    }

    void OnEventCallFever()
    {
        if (_ingameMgr == null) _ingameMgr = GameManager.Instance.InGameManager;

        LogManager.Log("OnEventCallFever");

        _ingameMgr.OnFever();
        ++_eventCount;
    }

    void OnEventCallTimeOut()
    {
        if (_ingameMgr == null) _ingameMgr = GameManager.Instance.InGameManager;

        LogManager.Log("OnEventCallTimeOut");

        _ingameMgr.OnTimeOut();
        ++_eventCount;
    }
}
