using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlightInterfaceController : MonoBehaviour
{
    [SerializeField] private RectTransform verticalLine;
    [SerializeField] private RectTransform horizontalLine;

    private float _verticalLineHeight;
    private float _horizontalLineWidth;

    [SerializeField] private RectTransform[] marks;
    private float _mainHorizontalMarkDistance;
    private float _sectionVerticalHeight;

    [SerializeField] private RectTransform heightShipIcon;
    [SerializeField] private RectTransform offsetShipIcon;

    private void Start()
    {
        _verticalLineHeight = Screen.height + verticalLine.sizeDelta.y;
        _horizontalLineWidth = Screen.width + horizontalLine.sizeDelta.x;
        print("Lines sizes: " + _verticalLineHeight + " vert, " + _horizontalLineWidth + " horiz");

        // Horizontal marks calculation
        _mainHorizontalMarkDistance = (_horizontalLineWidth / 2) * 0.8f;
        marks[0].localPosition = new Vector2(CalculateHorizontalPosition(-1), 0); // Left mark
        marks[1].localPosition = new Vector2(CalculateHorizontalPosition(1), 0); // Right mark
        marks[2].localPosition = new Vector2(CalculateHorizontalPosition(-0.5f), 0); // Left submark
        marks[3].localPosition = new Vector2(CalculateHorizontalPosition(0.5f), 0); // Right submark

        // Vertical marks calculation
        _sectionVerticalHeight = _verticalLineHeight / 11;
        marks[4].localPosition = new Vector2(0, CalculateVerticalPosition(0)); // Zero mark
        marks[5].localPosition = new Vector2(0, CalculateVerticalPosition(0.25f)); // 25% mark
        marks[6].localPosition = new Vector2(0, CalculateVerticalPosition(0.5f)); // 50% mark
        marks[7].localPosition = new Vector2(0, CalculateVerticalPosition(0.75f)); // 75% mark
    }

    public void InterfaceUpdate(float heightValue, float offsetValue)
    {
        heightShipIcon.localPosition = new Vector2(4, CalculateVerticalPosition(heightValue));
        offsetShipIcon.localPosition = new Vector2(CalculateHorizontalPosition(offsetValue), 6);
    }

    private float CalculateVerticalPosition(float value)
    {
        return (_sectionVerticalHeight - _verticalLineHeight / 2) + _sectionVerticalHeight * value * 10;
    }

    private float CalculateHorizontalPosition(float value)
    {
        if (value <= -1.25f)
            return -_horizontalLineWidth / 2;
        if (value >= 1.25f)
            return _horizontalLineWidth / 2;

        return _mainHorizontalMarkDistance * value;
    }
}