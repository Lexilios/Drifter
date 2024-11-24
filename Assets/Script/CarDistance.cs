using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarDistance : MonoBehaviour
{
    CountdownTimer timer;
    KartController CarController;
    Text TextDistance;
    float currentDistance = 0f;
    [SerializeField] float currentQuota = 10f;
    [SerializeField] float multiplierQuota = 2f;
    [SerializeField] float nextQuotaBase = 5f;
    [SerializeField] float timerMultiplier = 20f;

    private void Start()
    {
        CarController = FindAnyObjectByType<KartController>();
        TextDistance = GetComponent<Text>();
        timer = FindAnyObjectByType<CountdownTimer>();
    }

    private void Update()
    {
        currentDistance += CarController.currentSpeed / 3500f;
        TextDistance.text = currentDistance.ToString("F2") + "/" + currentQuota + "Km";
        if (currentDistance >= currentQuota)
        {
            multiplierQuota++;
            currentQuota += nextQuotaBase + (nextQuotaBase * multiplierQuota);
            timer.timeRemaining += timerMultiplier;
        }
    }

}
