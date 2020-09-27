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

    // 해당 UnitCtrl를 컨트롤 하는 PlayerController
    PlayerController _pCtrl = null;

    CameraManager _cameraMgr = null;

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
        // 초기화
        _cameraMgr = GameManager.Instance.CameraManager;

        _pCtrl = GetComponent<PlayerController>();

        _refMoveSpeed = _pCtrl.GetMoveSpeed();
        _refJumpPower = _pCtrl.GetJumpPower();

        // 점프루틴이 없을 경우 시작시킴
        if (_jumpRoutine == null)
            _jumpRoutine = StartCoroutine(ActionJump());
    }

    private void Update()
    {
        if (!_photonView.IsMine) { return; }

        UpdateMonobehaviour();

        UpdateMove();
        UpdateRotation();
        UpdateJump();

        transform.position = Vector3.Lerp(_transform.position, _targetTrans.position, Time.deltaTime * smoothTime);
    }

    // string 을 그냥 넣는것보다 이게 더 좋을것 같다는 생각과 그렇다는 글을 어디서 본 기억이 있. 틀렸으면 지적 환영
    private readonly string _axisKeyHorizontal = "Horizontal";
    private readonly string _axisKeyVertical = "Vertical";

    public enum eHandState
    {
        Default,
        Catch,
    }

    eHandState _curHandState = eHandState.Default;
    public eHandState CurHandState { get { return _curHandState; } }
    
    // Test
    private void OnHandDefault()
    {
        SphereCollider[] handsCollider = new SphereCollider[2];
        handsCollider[0] = _pCtrl.LeftHandObj.GetComponent<SphereCollider>();
        handsCollider[1] = _pCtrl.RightHandObj.GetComponent<SphereCollider>();

        for (int i = 0; i < 2; ++i)
        {
            handsCollider[i].enabled = false;
            Vector3 defaultEulerAngle = handsCollider[i].transform.localEulerAngles;
            handsCollider[i].transform.localEulerAngles = new Vector3(defaultEulerAngle.x, 0, defaultEulerAngle.z);
        }
    }

    private void OnHandCatch()
    {
        SphereCollider[] handsCollider = new SphereCollider[2];
        handsCollider[0] = _pCtrl.LeftHandObj.GetComponent<SphereCollider>();
        handsCollider[1] = _pCtrl.RightHandObj.GetComponent<SphereCollider>();

        for (int i = 0; i < 2; ++i)
        {
            handsCollider[i].enabled = true;
            Vector3 defaultEulerAngle = handsCollider[i].transform.localEulerAngles;
            handsCollider[i].transform.localEulerAngles = new Vector3(defaultEulerAngle.x, 50 * (i * -1), defaultEulerAngle.z);
        }
    }

    private void UpdateMonobehaviour()
    {
        if (Input.GetMouseButton(0) && _curHandState != eHandState.Catch)
        {
            // 마우스 왼클릭을 누르고 있음
            // 잡기 모드
            _curHandState = eHandState.Catch;
            OnHandCatch();
        }
        else if (!Input.GetMouseButton(0) && _curHandState != eHandState.Default)
        {   // 마우스 왼클릭을 안 누르고 있음
            // 기본 모드
            _curHandState = eHandState.Default;
            OnHandDefault();
        }
    }

    private void UpdateMove()
    {
        // 현재 WASD의 Input값을 가져온 후 delta값과 이동속도를 곱함.
        float moveX = Input.GetAxis(_axisKeyHorizontal) * _moveSpeed;
        float moveZ = Input.GetAxis(_axisKeyVertical) * _moveSpeed;

        Vector3 deltaPos = new Vector3(moveX, 0, moveZ);

        float dist = Vector3.Distance(_targetTrans.position, _transform.position);

        _targetTrans.localPosition += (deltaPos * Time.deltaTime);
    }

    void UpdateRotation()
    {
         _cameraMgr.transform.forward
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
