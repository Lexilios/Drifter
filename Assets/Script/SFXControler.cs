using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SFXControler : MonoBehaviour
{
    KartController CarController;
    AudioSource AudioSource;

    public void Start()
    {
        CarController = FindAnyObjectByType<KartController>();
        AudioSource = GetComponent<AudioSource>();

    }
    public void Update()
    {
        AudioSource.pitch = (CarController.currentSpeed / 20f) + 1f;

    }


}
