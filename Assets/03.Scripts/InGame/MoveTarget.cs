using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class MoveTarget : MonoBehaviour
{
    [SerializeField]
    private LayerMask _groundLayerMask;

    public float _distance = 2;

    public float GetGroundPointY()
    {
        float returnVector = 0.0f;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1, _groundLayerMask))
        {
            if (hit.distance < _distance)
                returnVector = hit.point.y;
        }

        return returnVector;
    }

    public bool IsGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, _distance, _groundLayerMask))
        {
            LogManager.Log(hit.transform.gameObject.name);

            return true;
        }
        else
            return false;
    }

    private void Update()
    {
        Vector3 targetPos = transform.position;
        targetPos.y -= _distance;

        Debug.DrawLine(transform.position, targetPos);

        if (IsGround()) return;

        transform.Translate(Physics.gravity * Time.deltaTime);
    }
}
