using System;
using UnityEngine;

public class MyCam : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 2f, -5f);
    [SerializeField] private float smoothSpeed = 5f;

    private void LateUpdate()
    {
        if (!_target) return;

        // Always stay behind the player in the player's forward direction
        Vector3 desiredPosition = _target.position - (_target.forward * offset.z);
        desiredPosition.y = _target.position.y + offset.y; // Keep height offset
        
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(_target.position + Vector3.up * offset.y * 0.5f);
    }
}