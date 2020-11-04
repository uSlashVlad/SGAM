using UnityEngine;

public class FlightGameController : MonoBehaviour
{
    // Directional enums for some functions associated with ship direction
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

    /// Contains ship object's transform from scene
    [SerializeField] private Transform shipTransform;

    /// Wind moves and rotate horizontally from left to right
    /// Plus is to right   Minus is to left
    [Space] [SerializeField] private float startWind;

    /// Decreases vertical acceleration
    [SerializeField] private float gravitation = 9;

    /// Decreases rotation acceleration and ship speed
    [Range(0, 1)] [SerializeField] private float startAirResistance = 0.1f;

    // Variables for storing current ship status
    [Space] [SerializeField] private float rotationSpeedCurrent;
    [SerializeField] private float verticalSpeedCurrent;
    [SerializeField] private float horizontalSpeedCurrent;
    [SerializeField] private float currentWind;
    [SerializeField] private float currentAirResistance;

    /// Passed path. Starts from zero
    [Space] [Range(-0.1f, 1)] [SerializeField]
    private float verticalPath;

    // Variables for vertical path and offset
    [SerializeField] private float currentHeight;
    [SerializeField] private float maxHeight = 10000;
    [SerializeField] private float currentOffset;
    [SerializeField] private float maxOffset = 200;

    // User input
    [Space] [SerializeField] private float rotationAccelerationInput;
    [SerializeField] private float shipAccelerationInput;

    // Sky color
    [Space] [SerializeField] private Camera cam;
    [SerializeField] private Gradient skyGradient;

    [Space] [SerializeField] private FlightInterfaceController interfaceController;

    private void FixedUpdate()
    {
        RotationProcessing();
        MovementProcessing();
        LateCalculations();

        interfaceController.InterfaceUpdate(
            heightValue: verticalPath,
            offsetValue: currentOffset / maxOffset
        );
    }

    private void RotationProcessing()
    {
        // Main control
        var horizontalAxis = Input.GetAxis("Horizontal");
        rotationSpeedCurrent += horizontalAxis * rotationAccelerationInput * (1 - currentAirResistance) *
                                Time.fixedDeltaTime;

        // Rotation from wind
        // If ship targeted up wind will increase rotation acceleration
        // Else it'll decrease acceleration
        float windAccel = 0;
        var shipDir = ShipTargetDirectionVertical();
        if (shipDir == ShipDirVertical.Up)
            windAccel = currentWind * Time.fixedDeltaTime;
        else if (shipDir == ShipDirVertical.Down)
            windAccel = -currentWind * Time.deltaTime;
        rotationSpeedCurrent += windAccel;

        shipTransform.Rotate(new Vector3(0, 0, -rotationSpeedCurrent * Time.fixedDeltaTime));
    }

    private void MovementProcessing()
    {
        var shipEulerRotation = Quaternion.Angle(shipTransform.rotation, Quaternion.identity);
        var shipRotation = shipEulerRotation * Mathf.PI / 180;

        verticalSpeedCurrent +=
            (shipAccelerationInput * Mathf.Cos(shipRotation) - gravitation) * Time.fixedDeltaTime *
            (1 - currentAirResistance);

        var shipDir = ShipTargetDirectionHorizontal();
        if (shipDir == ShipDirHorizontal.Left)
            horizontalSpeedCurrent -= shipAccelerationInput * Mathf.Sin(shipRotation) * Time.fixedDeltaTime *
                                      (1 - currentAirResistance);
        else if (shipDir == ShipDirHorizontal.Right)
            horizontalSpeedCurrent += shipAccelerationInput * Mathf.Sin(shipRotation) * Time.fixedDeltaTime *
                                      (1 - currentAirResistance);

        currentHeight += verticalSpeedCurrent * Time.fixedDeltaTime;
        currentOffset += horizontalSpeedCurrent * Time.fixedDeltaTime;
        //
        horizontalSpeedCurrent *= 1 - (currentAirResistance / 10);
    }

    private void LateCalculations()
    {
        verticalPath = currentHeight / maxHeight;
        var reversedProgress = 1 - Mathf.Clamp01(verticalPath);
        currentWind = startWind * reversedProgress;
        currentAirResistance = startAirResistance * reversedProgress;
        cam.backgroundColor = skyGradient.Evaluate(Mathf.Clamp01(verticalPath));
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