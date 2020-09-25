﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Photon.Pun;
using Photon.Realtime;

public class UnitController : MonoBehaviour
{
    // 움직일 속도
    public RefData _refMoveSpeed = new RefData();
    private float _moveSpeed => _refMoveSpeed._Value;

    // 점프할 힘
    public RefData _refJumpPower = new RefData();
    private float _jumpPower => _refJumpPower._Value;

    // 현재 오브젝트가 현재 클라이언트에서 자신의 것인지 확인
    [SerializeField]
    private PhotonView _photonView;

    // 움직일때 타겟을 잡고 그쪽으로 Lerp하며 움직임
    public Transform _targetTrans = null;

    // 이동할때의 Smooth한 정도
    public float smoothTime = 5;

    // 매번 호출하는것보다 캐싱해놓는것이 조금더 빠름
    private Transform _transform;

    // Jump 코루틴을 여러개 만들지 않게 하기 위함
    Coroutine _jumpRoutine;

    // 현재 점프가 가능한 상태인지 확인
    public bool _isGround = true;

    // 점프를 시키기위해 오브젝트의 강체를 가져옴
    Rigidbody _rigid;

    // 현재 오브젝트의 바닥부분 Position
    Vector3 _objectBottomPos;

    // 점프 후 아무 오브젝트에 충돌하였는지 확인
    private RaycastHit _hitGround;
    // 충돌로 인식할 레이어마스크
    public LayerMask _HitLayerMask;

    // 매번 생성하는것보다 캐싱해놓는것이 더 좋음
    WaitForSeconds _waitTime = new WaitForSeconds(.1f);

    private void Awake()
    {
        // 현재 오브젝트가 자신의 클라이언트의 것이 아닐경우 현재 스크립트를 비활성화 후 return
        if (!_photonView.IsMine) { enabled = false; return; }

        // 현재 클라이언트의 GameManager에게 이 오브젝트를 PlayerCharacter로 넘겨줌
        GameManager.Instance.PlayerCharacter = gameObject;

        // 매번 호출보단 캐싱이 빠름
        _transform = transform;

        // 이 오브젝트의 바닥부분을 가져옴
        _objectBottomPos = transform.position;
        _objectBottomPos.y -= transform.localScale.y / 2;

        // 강체 초기화
        _rigid = GetComponent<Rigidbody>();

        _targetTrans.gameObject.SetActive(true);
        _targetTrans.SetParent(_transform.parent);
    }

    private void Start()
    {
        PlayerController pCtrl = GetComponent<PlayerController>();

        _refMoveSpeed = pCtrl.GetMoveSpeed();
        _refJumpPower = pCtrl.GetJumpPower();

        // 점프루틴이 없을 경우 시작시킴
        if (_jumpRoutine == null)
            _jumpRoutine = StartCoroutine(ActionJump());
    }

    private void Update()
    {
        if (!_photonView.IsMine) { return; }

        UpdateMove();
        UpdateRotation();
        UpdateJump();

        transform.position = Vector3.Lerp(_transform.position, _targetTrans.position, Time.deltaTime * smoothTime);
    }

    // string 을 그냥 넣는것보다 이게 더 좋을것 같다는 생각과 그렇다는 글을 어디서 본 기억이 있. 틀렸으면 지적 환영
    private readonly string _axisKeyHorizontal = "Horizontal";
    private readonly string _axisKeyVertical = "Vertical";

    // float _inputWaitTime = 0.0f;

    private void UpdateMove()
    {
        // 현재 WASD의 Input값을 가져온 후 delta값과 이동속도를 곱함.
        float moveX = Input.GetAxis(_axisKeyHorizontal) * _moveSpeed;
        float moveZ = Input.GetAxis(_axisKeyVertical) * _moveSpeed;

        Vector3 deltaPos = new Vector3(moveX, 0, moveZ);

        //if (moveX == 0 && moveZ == 0)
        //{
        //    _inputWaitTime += Time.deltaTime;
        //    if (_inputWaitTime > 1f)
        //    {
        //        _inputWaitTime = 0;
        //        _targetTrans.localPosition = Vector3.zero;
        //    }
        //}
        //else
        //{
        //    _inputWaitTime = 0;
        //}

        float dist = Vector3.Distance(_targetTrans.position, _transform.position);

        _targetTrans.localPosition += (deltaPos * Time.deltaTime);

        //// 둘의 거리가 너무 멀어졌을 경우 타겟의 거리를 좁히거나 거리가 늘어나는 정도를 줄임
        //if (dist)
        //{
        //    _targetTrans.x -= moveX * 1.25f;
        //    _targetTrans.z -= moveZ * 1.25f;
        //}
        //else if (dist.sqrMagnitude > _moveSpeed * .1f) 
        //{
        //    _targetTrans.x -= moveX * .5f;
        //    _targetTrans.z -= moveZ * .5f;
        //}
    }

    void UpdateRotation()
    {
        if (Input.GetMouseButton(1))
        {
            float horizontal = Input.GetAxis("Mouse X");

            _targetTrans.localEulerAngles += new Vector3(0, horizontal * 200.0f * Time.deltaTime, 0);
            _transform.LookAt(_targetTrans);
        }
    }

    IEnumerator ActionJump()
    {
        // 오브젝트가 꺼지지않는 이상 계속 작동
        while (gameObject.activeSelf)
        {
            // 점프를 하였으면
            if (!_isGround)
            {
                if(_rigid.velocity.y < .05f && _rigid.velocity.y > -.05f)
                {
                    yield return _waitTime;

                    if (_rigid.velocity.y < .05f && _rigid.velocity.y > -.05f)
                        _isGround = true;
                }
            }

            yield return _waitTime;
        }

        _jumpRoutine = null;
    }

    private void UpdateJump()
    {
        // 현재 점프중이 아닐때 space바를 입력시 점프
        if (Input.GetKeyDown(KeyCode.Space) && _isGround)
        {
            _isGround = false;

            _rigid.AddForce(Vector3.up * _jumpPower * 100);
        }
    }
}
