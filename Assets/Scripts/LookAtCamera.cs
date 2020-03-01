using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Transform target = Camera.main.transform;

    void Update()
    {
        transform.LookAt(target);
    }
}
