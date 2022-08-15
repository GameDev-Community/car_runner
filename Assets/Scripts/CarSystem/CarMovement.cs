using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("deprecated", true)]
public class CarMovement : MonoBehaviour
{
    //[SerializeField] private Rigidbody rigidbody;
    //[SerializeField] private float maxSpeed;
    //[SerializeField] private float acceleration;
    //[SerializeField] private bool isGrounded = true;
    //[Range(0.2f, 1)] [SerializeField] private float accelerationCurve = 0.2f;
    //[SerializeField] private float roadBorderX;
    //[SerializeField] private float moveSidewaysSpeed = 200f;

    //[Header("Debug options")]
    //[SerializeField] private bool canMoveForward = true;
    //[SerializeField] private bool canMoveSideways = true;

    //private Game.Core.Racer _racer_prtp;

    //private void Awake()
    //{
    //    _racer_prtp = FindObjectOfType<Game.Core.Racer>();
    //}

    //private void FixedUpdate()
    //{
    //    if (canMoveForward)
    //        MoveForward();
    //    if (canMoveSideways)
    //        MoveSideways();
    //    //Debug.Log(rigidbody.velocity);
    //}

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.DrawWireSphere(transform.position + Vector3.down * 0.5f, 0.1f);
    //}

    //private void MoveForward()
    //{
    //    isGrounded = Physics.Raycast(rigidbody.position, transform.up * -1f, 0.5f);

    //    //float accel = _racer_prtp.ProcessStatValue(acceleration);
    //    float accel = acceleration;
    //    Vector3 movement = transform.forward * accel * (isGrounded ? 1.0f : 0.0f);

    //    // forward movement
    //    bool wasOverMaxSpeed = rigidbody.velocity.magnitude >= maxSpeed;

    //    // if over max speed, cannot accelerate faster.
    //    if (wasOverMaxSpeed) movement *= 0.0f;

    //    Vector3 newVelocity = rigidbody.velocity + movement * Time.fixedDeltaTime;
    //    newVelocity.y = rigidbody.velocity.y;

    //    //  clamp max speed if we are on ground
    //    if (isGrounded && !wasOverMaxSpeed)
    //    {
    //        newVelocity = Vector3.ClampMagnitude(newVelocity, maxSpeed);
    //    }

    //    rigidbody.velocity = newVelocity;
    //    // normalize angular velocity
    //    if (!isGrounded)
    //    {
    //        float angle = Vector3.Angle(Vector3.up, transform.up);

    //        Vector3 currentAngularVelocity = rigidbody.angularVelocity;
    //        currentAngularVelocity.x = 1.2f * angle * Time.fixedDeltaTime;
    //        rigidbody.angularVelocity = currentAngularVelocity;
    //    }
    //}

    //private void MoveSideways()
    //{
    //    int direction = 0;
    //    Vector3 newVelocity = rigidbody.velocity;
    //    if (isGrounded)
    //    {
    //        if (Input.GetKey(KeyCode.A))
    //        {
    //            direction = -1;
    //        }
    //        else if (Input.GetKey(KeyCode.D))
    //        {
    //            direction = 1;
    //        }
    //    }

    //    if (transform.position.x < -roadBorderX)
    //    {
    //        if (!isGrounded || direction == -1)
    //        {
    //            direction = 0;
    //        }
    //    }
    //    else if (transform.position.x > roadBorderX)
    //    {
    //        if (!isGrounded || direction == 1)
    //        {
    //            direction = 0;
    //        }
    //    }
    //    else if (!isGrounded)
    //        return;

    //    newVelocity.x = Mathf.Lerp(newVelocity.x, 8f * direction, Time.fixedDeltaTime * moveSidewaysSpeed);
    //    rigidbody.velocity = newVelocity;
    //}
}
