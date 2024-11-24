using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.Universal;

public class KartController : MonoBehaviour
{

    public PlayButtonScript PlayButtonScript;
    public Transform kartModel;
    public Transform kartNormal;
    public Rigidbody sphere;

    [Header("Debugs")]
    public float speed, currentSpeed;
    float rotate, currentRotate;
    int driftDirection;
    /*float driftPower;
    int driftMode = 0;
    bool first, second, third;*/


    [Header("Bools")]
    public bool drifting;

    [Header("Parameters")]

    public float acceleration = 30f;
    public float steering = 80f;
    public float gravity = 10f;
    public LayerMask layerMask;
    public float speedMultiplier = 12f;
    public float rotateMultiplier = 4f;


    void Update()
    {
        // Empeche la voiture de tourné sur elle même car currentSpeed n'atteint jamais 0

        if (currentSpeed < 0.1)
        {
            currentSpeed = 0;
        }
          
        // bullet time 
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            float time = Time.timeScale == 1 ? .5f : 1;
            Time.timeScale = time;
        }*/

        //Follow Collider
        //transform.position = sphere.transform.position - new Vector3(0, 0.4f, 0);

        //Accelerate
        if (Input.GetButton("Vertical"))
            speed = acceleration;

        //Steer
        if (Input.GetAxis("Horizontal") != 0)
        {
            int dir = Input.GetAxis("Horizontal") > 0 ? 1 : -1;
            float amount = Mathf.Abs((Input.GetAxis("Horizontal")));
            Steer(dir, amount);
        }

        //Drift
        if (Input.GetButtonDown("Jump") && !drifting && Input.GetAxis("Horizontal") != 0)
        {
            drifting = true;
            driftDirection = Input.GetAxis("Horizontal") > 0 ? 1 : -1;
            //Input.GetAxis("Horizontal") > 0 ? 1 : -1; [ -> avant ? = if (Input.GetAxis("Horizontal") > 0) { driftDirection = 1 }, : ->> else -1 ]

        }

        if (drifting)
        {
            float control = (driftDirection == 1) ? ExtensionMethods.Remap(Input.GetAxis("Horizontal"), -1, 1, 0, 2) : ExtensionMethods.Remap(Input.GetAxis("Horizontal"), -1, 1, 2, 0);
            Steer(driftDirection, control);

            //float powerControl = (driftDirection == 1) ? ExtensionMethods.Remap(Input.GetAxis("Horizontal"), -1, 1, .2f, 1) : ExtensionMethods.Remap(Input.GetAxis("Horizontal"), -1, 1, 1, .2f);
            //driftPower += powerControl;

            
        }


        if (Input.GetButtonUp("Jump") && drifting)
        {
            Boost();
        }

        currentSpeed = Mathf.SmoothStep(currentSpeed, speed, Time.deltaTime * speedMultiplier); speed = 0f;
        currentRotate = Mathf.Lerp(currentRotate, rotate, Time.deltaTime * rotateMultiplier); rotate = 0f;

        //a) Kart
        if (!drifting)
        {
            kartModel.localEulerAngles = Vector3.Lerp(kartModel.localEulerAngles, new Vector3(0, 90 + (Input.GetAxis("Horizontal") * 15), kartModel.localEulerAngles.z), .2f);
        }
        else
        {
            float control = (driftDirection == 1) ? ExtensionMethods.Remap(Input.GetAxis("Horizontal"), -1, 1, .5f, 2) : ExtensionMethods.Remap(Input.GetAxis("Horizontal"), -1, 1, 2, .5f);
            kartModel.parent.localRotation = Quaternion.Euler(0, Mathf.LerpAngle(kartModel.parent.localEulerAngles.y, (control * 15) * driftDirection, .2f), 0);
        }

       
    }

    private void FixedUpdate()
    {
        //Forward Acceleration
        if (!drifting)
            sphere.AddForce(kartModel.transform.forward * currentSpeed, ForceMode.Acceleration);
        else
            sphere.AddForce(-kartModel.transform.right * currentSpeed, ForceMode.Acceleration);

        //Gravity
        sphere.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

        //Steering
        if (currentSpeed > 0) {
            
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y + currentRotate, 0), Time.deltaTime * 5f);
        }
        RaycastHit hitOn;
        RaycastHit hitNear;

        Physics.Raycast(transform.position + (transform.up * .1f), Vector3.down, out hitOn, 1.1f, layerMask);
        Physics.Raycast(transform.position + (transform.up * .1f), Vector3.down, out hitNear, 2.0f, layerMask);

        //Normal Rotation
        kartNormal.up = Vector3.Lerp(kartNormal.up, hitNear.normal, Time.deltaTime * 8.0f);
        kartNormal.Rotate(0, transform.eulerAngles.y, 0);

    }

    public void Boost()
    {

        drifting = false;
        // Ajout d'un boost de vitesse après le drift
    }

    public void Steer(int direction, float amount)
    {
        rotate = (steering * direction) * amount;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Building")
        {
            PlayButtonScript.LooseScreen();
        }
        
    }

}