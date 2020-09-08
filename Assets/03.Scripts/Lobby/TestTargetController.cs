using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTargetController : MonoBehaviour
{
    //포톤 들어간 애들이 작동하지 않아서 그냥 UnitController에서 조작부분만 빼옴

    Vector3 _targetPos;

    [SerializeField, Range(3, 20)]
    private float _moveSpeed = 7;

    Vector3 _curVelocity = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        _targetPos.x += Input.GetAxis("Horizontal") * Time.fixedDeltaTime * _moveSpeed;
        _targetPos.y = transform.position.y;
        _targetPos.z += Input.GetAxis("Vertical") * Time.fixedDeltaTime * _moveSpeed;

        transform.position = Vector3.SmoothDamp(transform.position, _targetPos, ref _curVelocity, 0.1f);
    }
}
