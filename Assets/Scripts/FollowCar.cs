using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCar : MonoBehaviour
{
    public Vector3 offset;
    public float followSpeed = 10;
    public float lookSpeed = 10;

    private Transform _carToFollow;
    private void FixedUpdate()
    {
        if (_carToFollow == null)
        {
            return;
        }
        LookAtTarget();
        MoveTowardTarget();
    }
    private void LookAtTarget()
    {
        var lookDirection = _carToFollow.position - transform.position;
        var lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation,lookSpeed * Time.deltaTime);
    }

    private void MoveTowardTarget()
    {
        var targetPos = _carToFollow.position +
                            _carToFollow.forward * offset.z +
                            _carToFollow.right * offset.x +
                            _carToFollow.up * offset.y;
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
    }

    public void SetCar(Transform carToFollow)
    {
        _carToFollow = carToFollow;
    }
}
