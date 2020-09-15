using System.Collections;
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
    public Vector3 _targetPos;

    // 이동할때의 Smooth한 정도
    public float smoothTime = 5;

    // 매번 호출하는것보다 캐싱해놓는것이 조금더 빠름
    private Transform _transform;

    // Jump 코루틴을 여러개 만들지 않게 하기 위함
    Coroutine _jumpRoutine;

    // 현재 점프가 가능한 상태인지 확인
    bool _isJump = false;

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

        // 타겟 Position을 내 Position로 초기화
        _targetPos = _transform.position;

        // 이 오브젝트의 바닥부분을 가져옴
        _objectBottomPos = transform.position;
        _objectBottomPos.y -= transform.localScale.y / 2;

        // 강체 초기화
        _rigid = GetComponent<Rigidbody>();
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

    private void FixedUpdate()
    {
        if (!_photonView.IsMine) { return; }

        UpdateMove();
        UpdateJump();

        transform.position = Vector3.Lerp(_transform.position, _targetPos, Time.fixedDeltaTime * smoothTime);
    }

    // string 을 그냥 넣는것보다 이게 더 좋을것 같다는 생각과 그렇다는 글을 어디서 본 기억이 있음 틀렸으면 지적 환영
    private readonly string _axisKeyHorizontal = "Horizontal";
    private readonly string _axisKeyVertical = "Vertical";

    private void UpdateMove()
    {
        // 현재 WASD의 Input값을 가져온 후 delta값과 이동속도를 곱함.
        float moveX = Input.GetAxis(_axisKeyHorizontal) * Time.fixedDeltaTime * _moveSpeed;
        float moveZ = Input.GetAxis(_axisKeyVertical) * Time.fixedDeltaTime * _moveSpeed;

        // 타겟의 Position에 더하거나 초기화를 해준 후
        _targetPos.x += moveX;
        _targetPos.y = _transform.position.y;
        _targetPos.z += moveZ;

        Vector3 dist = _targetPos - _transform.position;

        // 둘의 거리가 너무 멀어졌을 경우 타겟의 거리를 좁히거나 거리가 늘어나는 정도를 줄임
        if (dist.sqrMagnitude > _moveSpeed * .3f)
        {
            _targetPos.x -= moveX * 1.25f;
            _targetPos.z -= moveZ * 1.25f;
        }
        else if (dist.sqrMagnitude > _moveSpeed * .1f) 
        {
            _targetPos.x -= moveX * .5f;
            _targetPos.z -= moveZ * .5f;
        }
    }

    IEnumerator ActionJump()
    {
        // 오브젝트가 꺼지지않는 이상 계속 작동
        while (gameObject.activeSelf)
        {
            // 점프를 하였으면
            if (!_isJump)
            {
                // 해당 오브젝트의 바닥에서 아래쪽으로 레이를 쏨
                Ray ray = new Ray(_objectBottomPos, Vector3.down);

                // 바닥이 될수 있는 레이어마스크에 걸린다면
                if (Physics.Raycast(ray, out _hitGround, 2.5f, _HitLayerMask))
                {
                    // 점프상태 아님
                    LogManager.Log("HitGround" + _hitGround.transform.gameObject.name);

                    _isJump = true;
                }
            }
            yield return _waitTime;
        }

        _jumpRoutine = null;
    }

    private void UpdateJump()
    {
        // 현재 점프중이 아닐때 space바를 입력시 점프
        if (Input.GetKeyDown(KeyCode.Space) && _isJump)
        {
            _isJump = false;

            _rigid.AddForce(Vector3.up * _jumpPower);
        }
    }
}
