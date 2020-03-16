using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorPickerManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Transform pickerParent;
    public Image pickerImage;
    public Image buttonImage;

    private Vector2 _rectScreenCoords;
    private bool _isPickingCircle;

    private Color32 _ringColor;

    private Color32 RingColor
    {
        get => _ringColor;
        set
        {
            if (_ringColor.r == value.r &&
                _ringColor.g == value.g &&
                _ringColor.b == value.b) return;

            _ringColor = value;
            buttonImage.color = value;
        }
    }

    public ColorChangeEvent onColorChanged;

    private void Start()
    {
        SetRectScreenCoords();
    }

    private void SetRectScreenCoords()
    {
        var corners = new Vector3[4];
        GetComponent<RectTransform>().GetWorldCorners(corners);
        _rectScreenCoords = new Vector2(corners[0].x + (corners[2].x - corners[0].x) / 2f,
            corners[0].y + (corners[2].y - corners[0].y) / 2f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPickingCircle = true;
        var delta = eventData.position - _rectScreenCoords;
        if (delta.magnitude > 70f)
        {
            var angle = Vector3.Angle(eventData.position - _rectScreenCoords, Vector3.right);
            if (eventData.position.y < _rectScreenCoords.y)
            {
                angle *= -1;
            }

            SetPickerRotation(angle);
        }
        else
        {
            OnColorButtonClick();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPickingCircle = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isPickingCircle)
        {
            var angle = Vector3.Angle(eventData.position - _rectScreenCoords, Vector3.right);
            if (eventData.position.y < _rectScreenCoords.y)
            {
                angle *= -1;
            }

            SetPickerRotation(angle);
        }
    }

    private void SetPickerRotation(float angle)
    {
        pickerParent.eulerAngles = new Vector3(0f, 0f, angle);
        pickerImage.color = ToolsUtils.GetCosineColor(angle);
        RingColor = pickerImage.color;
    }

    public void OnColorButtonClick()
    {
        if (gameObject.activeSelf)
        {
            onColorChanged?.Invoke(RingColor);
        }

        SetColorRingState(!gameObject.activeSelf);
    }

    private void SetColorRingState(bool state)
    {
        gameObject.SetActive(state);
    }
}

[Serializable]
public class ColorChangeEvent : UnityEvent<Color>
{
}