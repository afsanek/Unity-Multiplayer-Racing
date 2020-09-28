using UnityEngine;
using UnityEngine.Networking;

public class CarMovement : MonoBehaviour
{
    //private float _currentBrakeForce;
    private float _currentSteeringAngle;
    private float _horizontalInput;
    private float _verticalInput;
    private bool  _isBraking;
    private bool  _isDoneBraking;
    private bool  _isDrifting;
    private bool  _startOverAfterBrake;
    
    //[0] right, [1] left of front. [2] right, [3] left of back
    [SerializeField] private WheelCollider[] wheelsColliders;
    [SerializeField] private Transform[] wheelsTransforms;
    
    [SerializeField] private float motorForce;
    [SerializeField] private float brakeForce;
    [SerializeField] private float maxSteeringAngle;

    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";

    
    //better to use late update here and in camera to avoid jitter
    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheelsPos();
        
    }
    private void GetInput()
    {
        _horizontalInput = Input.GetAxis(Horizontal);
        _verticalInput = Input.GetAxis(Vertical);
        _isBraking = Input.GetKeyDown(KeyCode.Space);
        _isDoneBraking = Input.GetKeyUp(KeyCode.Space);
        _isDrifting = Input.GetKeyDown(KeyCode.Q);
    }
    private void HandleMotor()
    {
        wheelsColliders[0].motorTorque = _verticalInput * motorForce;
        wheelsColliders[1].motorTorque = _verticalInput * motorForce;
        //_currentBrakeForce = _isBraking ? brakeForce : 0f;
        if (_isBraking)
        {
            ApplyBraking(brakeForce);
        }

        if (_isDoneBraking) // || !_isBraking)
        {
            ApplyBraking(0.0f);
            wheelsColliders[0].motorTorque = _verticalInput * motorForce * 1.2f;
            wheelsColliders[1].motorTorque = _verticalInput * motorForce * 1.2f;
        }
    }
    private void ApplyBraking(float currentBrakeForce)
    {
        foreach (var wheel in wheelsColliders)
        {
            wheel.brakeTorque = currentBrakeForce;
        }
    }
    private void HandleSteering()
    {
        //drifting need improvement !!!
        if (_isDrifting)
        {
            print("here");
            DriftingMode(2, 3.5f);
            DriftingMode(3, 3.5f);
        }
        else
        {
            DriftingMode(2, 0.2f);
            DriftingMode(3, 0.2f);
        }
        _currentSteeringAngle = maxSteeringAngle * _horizontalInput;
        wheelsColliders[0].steerAngle = _currentSteeringAngle;
        wheelsColliders[1].steerAngle = _currentSteeringAngle;
    }
    private void DriftingMode(int wheelIndex,float amount)
    {
        var frictionCurve = wheelsColliders[wheelIndex].sidewaysFriction;
        frictionCurve.extremumSlip = amount;
        wheelsColliders[wheelIndex].sidewaysFriction = frictionCurve;
    }
    private void UpdateWheelsPos()
    {
        for(var i=0;i<wheelsTransforms.Length;i++)
        {
            Vector3 position;
            Quaternion rotation;
            wheelsColliders[i].GetWorldPose(out position,out rotation);
            wheelsTransforms[i].position = position;
            wheelsTransforms[i].rotation = rotation;
        }
    }
}
/*--simple car code
        transform.Translate(new Vector3(0,0,VerticallInput) * (speed * Time.deltaTime));
        if (VerticallInput != 0)
        {
            transform.Rotate(Vector3.up,Time.deltaTime * HorizontalInput * turnSpeed);
        }
        
        //transform.Translate(Vector3.forward * (Time.deltaTime * speed ));
        */