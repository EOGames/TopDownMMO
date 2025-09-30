using System;
using UnityEngine;
using UnityEngine.Serialization;

public class HeadLookAt : MonoBehaviour
{
    public Transform target;
    [FormerlySerializedAs("head")] [SerializeField] Transform headToRotate;

    private void Update()
    {
        if (target)
        {
            headToRotate.LookAt(target);
        }
    }
}
