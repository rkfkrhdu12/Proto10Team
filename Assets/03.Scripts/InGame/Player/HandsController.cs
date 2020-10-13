using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsController : MonoBehaviour
{
    PlayerController.eHandState _prevHandState = PlayerController.eHandState.Default;
    PlayerController.eHandState curHandState;

    GameObject _catchingObject = null;
    Vector3 _collidePoint = Vector3.zero;
    List<GameObject> _catchedToppings = new List<GameObject>();
    bool _isCatch = false;

    static readonly string _toppingTag = "Topping";

    [SerializeField]
    PlayerController _pCtrl;

    [SerializeField]
    UnitController _uCtrl = null;

    Camera _camera = null;

    private void Start()
    {
        _camera = GameManager.Instance.InGameManager.CameraManager.Camera;
    }

    private void Update()
    {
        curHandState = _pCtrl.CurHandState;

        if (curHandState == PlayerController.eHandState.Catch)
        {
            if (_catchingObject == null) { return; }
            if (_camera == null)
                _camera = GameManager.Instance.InGameManager.CameraManager.Camera;

            Vector3 MoveDelta = _uCtrl.MoveDelta * Time.deltaTime;

            LogManager.Log(_collidePoint.ToString());

            _catchingObject.transform.parent.transform.position = _pCtrl.transform.position;

            var defaultEulerAngle = _uCtrl.transform.eulerAngles;
            var cameraEulerAngle = _camera.transform.eulerAngles;

            var eulerAngles = new Vector3(defaultEulerAngle.x, cameraEulerAngle.y, defaultEulerAngle.z);

            _catchingObject.transform.position += MoveDelta;
            _catchingObject.transform.position = new Vector3(_catchingObject.transform.position.x,
                                                             transform.position.y,
                                                             _catchingObject.transform.position.z);
            _catchingObject.transform.parent.eulerAngles = eulerAngles;
        }
    }

    private void LateUpdate()
    {
        _prevHandState = curHandState;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (curHandState == PlayerController.eHandState.Catch)
        {
            if(other.CompareTag(_toppingTag))
            {
                _catchedToppings.Add(other.gameObject);
                if (_catchingObject == null)
                {
                    _catchingObject = _catchedToppings[0];

                    Rigidbody rigid = _catchingObject.GetComponent<Rigidbody>();
                    rigid.constraints = RigidbodyConstraints.FreezePosition;

                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, _catchingObject.transform.position, out hit, 2.0f))
                    {
                        _collidePoint = hit.point;
                    }

                    _catchingObject.transform.parent.transform.position = transform.position;
                    _catchingObject.transform.localPosition = Vector3.zero;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if (curHandState == PlayerController.eHandState.Catch)
        {
            if (other.CompareTag(_toppingTag))
            {
                GameObject curTopping = other.gameObject;
                if (_catchedToppings.Contains(curTopping))
                {
                    Rigidbody rigid = _catchingObject.GetComponent<Rigidbody>();
                    rigid.constraints = RigidbodyConstraints.None;

                    _catchedToppings.Remove(curTopping);
                }
                if (_catchingObject == curTopping && _catchedToppings.Count != 0)
                {
                    _catchingObject = _catchedToppings[0];
                }
            }
        }
    }
}
