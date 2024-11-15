using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float defaultMoveSpeed;
    [SerializeField] private float MoveSpeed = 50;
    [SerializeField] public float MaxSpeed = 15;
    [SerializeField] public float Drag = 0.98f;
    [SerializeField] public float SteerAngle = 20;
    [SerializeField] public float Traction = 1;

    private Vector3 MoveForce;

    public Rigidbody rb;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();

    }



    private void FixedUpdate()
    {
        // Move
        MoveForce += transform.forward * MoveSpeed * Input.GetAxis("Vertical") * Time.fixedDeltaTime;
        rb.velocity = MoveForce * 50 * Time.fixedDeltaTime;
    }



    void Update()
    {



        // Steer
        float steerInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * steerInput * MoveForce.magnitude * SteerAngle * Time.deltaTime);

        // Drag
        MoveForce *= Drag;

        // Max speed limit
        MoveForce = Vector3.ClampMagnitude(MoveForce, MaxSpeed);

        // Traction
        MoveForce = Vector3.Lerp(MoveForce.normalized, transform.forward, Traction * Time.deltaTime) * MoveForce.magnitude;

    }
}