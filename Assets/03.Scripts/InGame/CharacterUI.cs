using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class CharacterUI : MonoBehaviour
{
    [SerializeField]
    public TMP_Text _nameBox;

    Dictionary<eTeam, Color> _teamColorBox;

    private GameObject _traceObject;

    RectTransform _canvasRectTrs = null;
    Camera _Camera = null;


    private void Awake()
    {
        _teamColorBox.Add(eTeam.Red, new Color(255, 67, 0));
        _teamColorBox.Add(eTeam.Blue, new Color(14, 0, 255));
    }

    public void Init(PlayerController pCtrl)
    {
        _nameBox.text = pCtrl._pView.name;
        _nameBox.color = _teamColorBox[pCtrl.Team];

        _traceObject = pCtrl.gameObject;

        if(_canvasRectTrs == null)
        {
            _canvasRectTrs = GameManager.Instance.InGameManager.Canvas.GetComponent<RectTransform>();
        }

        if (_Camera == null)
        {
            _Camera = GameManager.Instance.InGameManager.CameraManager.Camera;
        }
    }

    public void LateUpdate()
    {
        if (_canvasRectTrs == null && _Camera == null) { return; }

        var screenPos = Camera.main.WorldToScreenPoint(transform.position);

        if (screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTrs, screenPos, _Camera, out localPos);

        // localPos.x += 3;
        // localPos.y += 80;

        transform.localPosition = localPos;
    }
}
