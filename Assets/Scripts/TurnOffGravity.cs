using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffGravity : MonoBehaviour
{
    Rigidbody rgbd;

    void Start()
    {
        rgbd = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rgbd.useGravity = false;
    }
}
