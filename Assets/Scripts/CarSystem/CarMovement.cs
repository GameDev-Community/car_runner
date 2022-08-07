using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private bool isGrounded = true;
    [Range(0.2f, 1)] public float AccelerationCurve;
    const float k_NullInput = 0.01f;

    void FixedUpdate()
    {
        MoveVehicle(true);
    }

    void MoveVehicle(bool accelerate)
    {
        float accelInput = (accelerate ? 1.0f : 0.0f);

        // manual acceleration curve coefficient scalar
        float accelerationCurveCoeff = 5;
        float currentSpeed = rigidbody.velocity.magnitude;
        float accelRampT = currentSpeed / maxSpeed;
        float multipliedAccelerationCurve = AccelerationCurve * accelerationCurveCoeff;
        float accelRamp = Mathf.Lerp(multipliedAccelerationCurve, 1, accelRampT * accelRampT);

        float finalAcceleration = acceleration * accelRamp;

        Vector3 fwd = transform.forward;
        Vector3 movement = fwd * accelInput * finalAcceleration * (isGrounded ? 1.0f : 0.0f);

        // forward movement
        bool wasOverMaxSpeed = currentSpeed >= maxSpeed;

        // if over max speed, cannot accelerate faster.
        if (wasOverMaxSpeed) movement *= 0.0f;

        Vector3 newVelocity = rigidbody.velocity + movement * Time.fixedDeltaTime;
        newVelocity.y = rigidbody.velocity.y;

        //  clamp max speed if we are on ground
        if (isGrounded && !wasOverMaxSpeed)
        {
            newVelocity = Vector3.ClampMagnitude(newVelocity, maxSpeed);
        }

        rigidbody.velocity = newVelocity;

        // normalize angular velocity
        Vector3 rotation = transform.rotation.eulerAngles;
        int direction = rotation.x > 0 ? -1 : 1;
        Debug.Log(rotation);
        if (Mathf.Abs(rotation.x) > 10f)
        {
            Vector3 currentAngularVelocity = rigidbody.angularVelocity;
            currentAngularVelocity.x = 200f * direction;
            rigidbody.angularVelocity = currentAngularVelocity;
        }
    }
}
