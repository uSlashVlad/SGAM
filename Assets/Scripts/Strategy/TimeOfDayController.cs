using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOfDayController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Gradient skyGradient;
    [SerializeField] private float timeK = 1; // 1 is 1 day per second, 0.5 is 0.5 day per second and so on
    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime * timeK;
        cam.backgroundColor = skyGradient.Evaluate(Mathf.Clamp01(_timer));
        if (_timer > 1) _timer = 0;
    }
}