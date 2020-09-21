using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

using Photon.Pun;

public class CharacterUI : MonoBehaviour
{
    [SerializeField]
    public TMP_Text _nameBox;

    [SerializeField]
    private GameObject _traceObject;

    RectTransform _canvasRectTrs = null;
    Camera _camera = null;

    PhotonView _pView = null;

    InGameManager _inGameMgr = null;

    private readonly static Vector3 offset = new Vector3(0, 50, 0);

    private void Start()
    {
        // 혹시 null 일 경우 새로 초기화
        if (_inGameMgr == null)         _inGameMgr = GameManager.Instance.InGameManager;
        if (_canvasRectTrs == null)     _canvasRectTrs = _inGameMgr.Canvas.GetComponent<RectTransform>();
        if (_camera == null)            _camera = _inGameMgr.CameraManager.Camera;
        if (_pView == null)             _pView = _inGameMgr.PView;
    }

    public void Init(PlayerController pCtrl)
    {
        if (pCtrl == null) { gameObject.SetActive(false); return; }

        // TextBox속의 닉네임 설정
        _nameBox.text = pCtrl._pView.Owner.NickName;

        // 따라다닐 오브젝트 설정
        _traceObject = pCtrl.HeadObj;

        // 혹시 null 일 경우 새로 초기화
        if (_inGameMgr == null)         _inGameMgr = GameManager.Instance.InGameManager;
        if (_canvasRectTrs == null)     _canvasRectTrs = _inGameMgr.Canvas.GetComponent<RectTransform>();
        if (_camera == null)            _camera = _inGameMgr.CameraManager.Camera;
        if (_pView == null)             _pView = _inGameMgr.PView;
    }

    [PunRPC]
    void Disable()
    {
        // 유저가 나가거나 UI가 사라져야할 경우
        if (string.IsNullOrWhiteSpace(_nameBox.text)) { return; }

        _traceObject = null;
        _nameBox.text = "";

    }

    public void LateUpdate()
    {
        if (_canvasRectTrs == null && _camera == null) { return; }

        if (_traceObject == null)
        {
            Disable();
            return;
        } // { _inGameMgr.PView.RPC("Disable", RpcTarget.AllBuffered); return; }
        else if (_traceObject != null)
        {
            if (!_traceObject.activeSelf)
            {
                Disable();
                return;
            }
        }

        var screenPos = _camera.WorldToScreenPoint(_traceObject.transform.position);

        transform.localPosition = screenPos + offset;
    }
}
