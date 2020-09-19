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

    private GameObject _traceObject;

    RectTransform _canvasRectTrs = null;
    Camera _camera = null;

    PhotonView _pView = null;

    InGameManager _inGameMgr = null;

    private void Start()
    {
        _inGameMgr = GameManager.Instance.InGameManager;

        if (_canvasRectTrs == null)
        {
            _canvasRectTrs = _inGameMgr.Canvas.GetComponent<RectTransform>();
        }

        if (_camera == null)
        {
            _camera = _inGameMgr.CameraManager.Camera;
        }

        if(_pView == null)
        {
            _pView = _inGameMgr.PView;
        }
    }

    public void Init(PlayerController pCtrl)
    {
        _nameBox.text = pCtrl._pView.Owner.NickName;

        _traceObject = pCtrl.gameObject;

        if (_canvasRectTrs == null)
        {
            _canvasRectTrs = _inGameMgr.Canvas.GetComponent<RectTransform>();
        }

        if (_camera == null)
        {
            _camera = _inGameMgr.CameraManager.Camera;
        }

        if (_pView == null)
        {
            _pView = _inGameMgr.PView;
        }
    }

    //[PunRPC]
    void Disable()
    {
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
        else if(_traceObject != null)
        {
            if (!_traceObject.activeSelf)
            {
                Disable();
                return;
            }
        }

        var screenPos = Camera.main.WorldToScreenPoint(_traceObject.transform.position);

        if (screenPos.z < 0.0f) { screenPos *= -1.0f; }

        Vector2 localPos;
            
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTrs, screenPos, _camera, out localPos);

        if (localPos == null) { return; }

        localPos.y += 100.0f;

        transform.localPosition = localPos;
    }
}
