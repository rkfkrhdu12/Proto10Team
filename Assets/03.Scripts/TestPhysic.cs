using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPhysic : MonoBehaviour
{
    public LayerMask _groundLayerMask;

    Ray _ray;

    [SerializeField]
    Camera _mainCamera = null;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_mainCamera == null) { return; }


        Debug.DrawRay(transform.position, Vector3.down * .5f, Color.white);

        RaycastHit hit;
        if (Physics.Raycast(transform.position,Vector3.down, out hit, .25f, _groundLayerMask)) 
        {
            LogManager.Log("Log");
            // transform.Translate(Physics.gravity * Time.fixedDeltaTime);
        }
    }
}
