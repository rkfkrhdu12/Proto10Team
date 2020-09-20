using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScripts : MonoBehaviour
{
    Animator _ani;
    private void Start()
    {
        _ani = GetComponent<Animator>();
    }

    public bool _isOn = false;
    private void Update()
    {
        if (_isOn) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isOn = true;
            _ani.SetTrigger("Start");
        }
    }

    public void EndEvent()
    {
        gameObject.SetActive(false);
    }
}
