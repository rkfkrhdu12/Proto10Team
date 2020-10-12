using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Photon.Pun;
using Photon.Realtime;

public class UnitController : MonoBehaviour
{
    public void OnHandDefault()
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

    public void OnHandCatch()
    {
        SphereCollider[] handsCollider = new SphereCollider[2];
        handsCollider[0] = _pCtrl.LeftHandObj.GetComponent<SphereCollider>();
        handsCollider[1] = _pCtrl.RightHandObj.GetComponent<SphereCollider>();

        for (int i = 0; i < 2; ++i)
        {
            handsCollider[i].enabled = true;
            Vector3 defaultEulerAngle = handsCollider[i].transform.localEulerAngles;

            Vector3 curHandleEulerAngle = new Vector3(defaultEulerAngle.x, 50 * (i == 1 ? -1 : 1), defaultEulerAngle.z);

            handsCollider[i].transform.localEulerAngles = curHandleEulerAngle;
        }
    }

    public void Init()
    {
        LogManager.Log("Init : " + gameObject.GetPhotonView().name);

        // 점프루틴이 없을 경우 시작시킴
        if (_jumpRoutine == null)
            _jumpRoutine = StartCoroutine(ActionJump());

        _targetTrans.gameObject.SetActive(true);
        _targetTrans.SetParent(_transform.parent);

        _targetTrans.localPosition = _transform.localPosition;

        _isInit = true;
    }

    #region Variable

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

    // 점프 중인가
    bool _isJumping = false;

    // 매번 생성하는것보다 캐싱해놓는것이 더 좋음
    WaitForSeconds _waitTime = new WaitForSeconds(.1f);

    WaitForSeconds _jumpTime = new WaitForSeconds(0.5f);

    // 해당 UnitCtrl를 컨트롤 하는 PlayerController
    PlayerController _pCtrl = null;

    CameraManager _cameraMgr = null;

    // string 을 그냥 넣는것보다 이게 더 좋을것 같다는 생각과 그렇다는 글을 어디서 본 기억이 있. 틀렸으면 지적 환영
    private readonly string _axisKeyHorizontal = "Horizontal";
    private readonly string _axisKeyVertical = "Vertical";

    bool _isInit = false;
    #endregion

    #region Monobehaviour Function
    private void Awake()
    {
        // 현재 오브젝트가 자신의 클라이언트의 것이 아닐경우 현재 스크립트를 비활성화 후 return
        if (!_photonView.IsMine) { enabled = false; return; }

        // 현재 클라이언트의 GameManager에게 이 오브젝트를 PlayerCharacter로 넘겨줌
        GameManager.Instance.PlayerCharacter = gameObject;

        // 매번 호출보단 캐싱이 빠름
        _transform = transform;
    }

    private void Start()
    {
        // 초기화
        _cameraMgr = GameManager.Instance.CameraManager;

        _pCtrl = GetComponent<PlayerController>();

        _refMoveSpeed = _pCtrl.GetMoveSpeed();
        _refJumpPower = _pCtrl.GetJumpPower();
    }
    private void Update()
    {
        if (!_photonView.IsMine || !_isInit) { return; }

        UpdateJump();
    }

    private void FixedUpdate()
    {
        if (!_photonView.IsMine || !_isInit) { return; }

        UpdateMove();
        UpdateRotation();
        // UpdateJump();

        transform.position = Vector3.Lerp(_transform.position, _targetTrans.position, Time.fixedDeltaTime * smoothTime);
    }
    #endregion

    #region Private Function

    private void UpdateMove()
    {
        // 현재 WASD의 Input값을 가져온 후 delta값과 이동속도를 곱함.
        float moveX = Input.GetAxis(_axisKeyHorizontal) * _moveSpeed,
              moveZ = Input.GetAxis(_axisKeyVertical) * _moveSpeed;

        Vector3 deltaPos = (moveX * _targetTrans.right) + (moveZ * _targetTrans.forward);

        Vector2 playerPos = new Vector2(_transform.position.z,   _transform.position.x);
        Vector2 targetPos = new Vector2(_targetTrans.position.z, _targetTrans.position.x);

        //float dist = Vector2.Distance(playerPos, targetPos);
        //if (dist > _moveSpeed * .2f)
        //{
        //    _targetTrans.localPosition = new Vector3(_transform.localPosition.x,
        //                                             _transform.localPosition.y, 
        //                                             _transform.localPosition.z);
        //}
        //else
        //    _targetTrans.localPosition += deltaPos * Time.fixedDeltaTime;

        _targetTrans.localPosition = new Vector3(_targetTrans.localPosition.x + deltaPos.x * Time.deltaTime,
                                                 _transform.localPosition.y   + deltaPos.y * Time.deltaTime,
                                                 _targetTrans.localPosition.z + deltaPos.z * Time.deltaTime);
    }

    void UpdateRotation()
    {
        if (Input.GetMouseButton(1))
        {
            Vector3 cameraEulerAngle = _cameraMgr.transform.eulerAngles;
            Vector3 defaultEulerAngle = _transform.eulerAngles;

            _transform.eulerAngles = _targetTrans.eulerAngles = new Vector3(defaultEulerAngle.x, cameraEulerAngle.y, defaultEulerAngle.z);
        }
    }

    private IEnumerator ActionJump()
    {
        // 오브젝트가 꺼지지않는 이상 계속 작동
        while (gameObject.activeSelf)
        {
            yield return _waitTime;

            // 점프를 하였으면
            if (_isJumping)
            {
                yield return _jumpTime;

                if (Mathf.Approximately(_rigid.velocity.y, 0))
                    _isJumping = false;
            }

        }

        _jumpRoutine = null;
    }

    [SerializeField]
    Rigidbody _rigid;

    private void UpdateJump()
    {
        // 현재 점프중이 아닐때 space바를 입력시 점프
        if (Input.GetKeyDown(KeyCode.Space) && !_isJumping)
        {
            //_isGround = false;
            _isJumping = true; 
            
            _rigid.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
        }
    }
    #endregion
}
