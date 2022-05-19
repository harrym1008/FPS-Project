using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardRotation : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.eulerAngles = Camera.main.transform.eulerAngles/* + new Vector3(0, 180, 0)*/  ;
    }
}
