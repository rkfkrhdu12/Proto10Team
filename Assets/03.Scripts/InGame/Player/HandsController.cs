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

    private void Update()
    {
        curHandState = _pCtrl.CurHandState;

        if (curHandState == PlayerController.eHandState.Catch)
        {
            if (_catchingObject == null) { return; }

        }
        else 
        {
            if (_catchingObject != null)
            {
                _catchingObject.transform.SetParent(null);

                _catchedToppings.Remove(_catchingObject);

                _catchingObject.transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
                if (_catchedToppings.Count != 0)
                {
                    _catchingObject = _catchedToppings[0];
                }
                else
                {
                    _catchingObject = null;
                }
            }
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
            if (other.CompareTag(_toppingTag))
            {
                LogManager.Log(other.name);

                _catchedToppings.Add(other.transform.parent.gameObject);
                if (_catchingObject == null)
                {
                    _catchingObject = _catchedToppings[0];
                    _catchingObject.transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = true;

                    _catchingObject.transform.SetParent(transform);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.CompareTag(_toppingTag))
        //{
        //    GameObject curTopping = other.gameObject;
        //    if (_catchedToppings.Contains(curTopping))
        //    {
        //        _catchingObject.transform.SetParent(null);

        //        LogManager.Log(_catchingObject.name);
        //        _catchedToppings.Remove(curTopping);
        //    }

        //    if (_catchingObject == curTopping && _catchedToppings.Count != 0)
        //    {
        //        _catchingObject = _catchedToppings[0];
        //    }
        //}
    }
}
