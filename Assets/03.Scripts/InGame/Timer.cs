using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    float _remainingTime = 180;

    bool _isStart = false;

    delegate void ItemEvent();

    int _eventCount = 0;
    int[] _eventTime = new int[5];

    InGameManager _ingameMgr;

    ItemEvent OnItemEvent;

    private void Awake()
    {
        _eventTime[0] = 150;
        _eventTime[1] = 120;
        _eventTime[2] = 90;
        _eventTime[3] = 60;
        _eventTime[4] = 30;

        _ingameMgr = GameManager.Instance.InGameManager;

        OnItemEvent = new ItemEvent(OnItemEventCall);
        for (int i = 0; i < _eventTime.Length - 1; ++i)
            OnItemEvent += OnItemEventCall;
    }

    void Update()
    {
        if (!_isStart) { return; }

        _remainingTime = Mathf.Max(_remainingTime - Time.deltaTime, 0.0f);

        OnItemEvent();
    }

    void OnItemEventCall()
    {
        if ((int)_remainingTime == _eventTime[_eventCount])
        {
            Debug.Log(_eventTime[_eventCount]);

            if (_ingameMgr == null) _ingameMgr = GameManager.Instance.InGameManager;

            _ingameMgr.OnItemEvent(_eventCount++);
        }
    }
}
