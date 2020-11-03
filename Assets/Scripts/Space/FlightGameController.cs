using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class FlightGameController : MonoBehaviour
{
    private enum ShipDirVertical
    {
        Up,
        Down,
        Middle
    };

    private enum ShipDirHorizontal
    {
        Left,
        Right,
        Middle
    };

    [SerializeField] private Transform shipTransform;

    // Wind moves and rotate horizontally from left to right
    // Plus is ->   Minus is <-
    [Space] [SerializeField] private float wind;

    // Decreases vertical acceleration
    [SerializeField] private float gravitation = 9;

    // Deceases rotation acceleration
    [Range(0, 1)] [SerializeField] private float airResistance = 0.1f;
    [SerializeField] private float horizontalStabilizationK = 0.01f;

    [Space] [SerializeField] private float rotationSpeedCurrent;
    [SerializeField] private float verticalSpeedCurrent;
    [SerializeField] private float horizontalSpeedCurrent;

    [Space] [Range(-10, 100)] [SerializeField]
    private float verticalPath; // In percentage

    [SerializeField] private float currentHeight;
    [SerializeField] private float maxHeight = 10000;
    [SerializeField] private float currentOffset;
    [SerializeField] private float maxOffset = 200;

    [Space] [SerializeField] private float rotationAccelerationInput;
    [SerializeField] private float shipAccelerationInput;

    private void FixedUpdate()
    {
        RotationProcessing();
        MovementProcessing();

        verticalPath = (currentHeight / maxHeight) * 100;
    }

    private void RotationProcessing()
    {
        // Main control
        var horizontalAxis = Input.GetAxis("Horizontal");
        rotationSpeedCurrent +=
            horizontalAxis * rotationAccelerationInput * (1 - airResistance) * Time.fixedDeltaTime;

        // Rotation from wind
        // If ship targeted up wind will increase rotation acceleration
        // Else it'll decrease acceleration
        float windAccel = 0;
        var shipDir = ShipTargetDirectionVertical();
        if (shipDir == ShipDirVertical.Up)
            windAccel = wind * Time.fixedDeltaTime;
        else if (shipDir == ShipDirVertical.Down)
            windAccel = -wind * Time.deltaTime;
        rotationSpeedCurrent += windAccel;

        shipTransform.Rotate(new Vector3(0, 0, -rotationSpeedCurrent * Time.fixedDeltaTime));
    }

    private void MovementProcessing()
    {
        var shipEulerRotation = Quaternion.Angle(shipTransform.rotation, Quaternion.identity);
        var shipRotation = shipEulerRotation * Mathf.PI / 180;

        verticalSpeedCurrent +=
            (shipAccelerationInput * Mathf.Cos(shipRotation) - gravitation) * Time.fixedDeltaTime * (1 - airResistance);

        var shipDir = ShipTargetDirectionHorizontal();
        if (shipDir == ShipDirHorizontal.Left)
            horizontalSpeedCurrent -= shipAccelerationInput * Mathf.Sin(shipRotation) * Time.fixedDeltaTime *
                                      (1 - airResistance);
        else if (shipDir == ShipDirHorizontal.Right)
            horizontalSpeedCurrent += shipAccelerationInput * Mathf.Sin(shipRotation) * Time.fixedDeltaTime *
                                      (1 - airResistance);

        currentHeight += verticalSpeedCurrent * Time.fixedDeltaTime;
        currentOffset += horizontalSpeedCurrent * Time.fixedDeltaTime;
        //
        horizontalSpeedCurrent *= 1 - horizontalStabilizationK;
    }

    private ShipDirVertical ShipTargetDirectionVertical()
    {
        var shipRotation = shipTransform.rotation.eulerAngles.z;
        if (shipRotation < 90 || shipRotation > 270)
            return ShipDirVertical.Up;
        if (shipRotation > 90 && shipRotation < 270)
            return ShipDirVertical.Down;
        return ShipDirVertical.Middle;
    }

    private ShipDirHorizontal ShipTargetDirectionHorizontal()
    {
        var shipRotation = shipTransform.rotation.eulerAngles.z;
        if (shipRotation < 180 && shipRotation != 0)
            return ShipDirHorizontal.Left;
        if (shipRotation > 180 && shipRotation != 360)
            return ShipDirHorizontal.Right;
        return ShipDirHorizontal.Middle;
    }
}