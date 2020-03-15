﻿using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class PencilManager : MonoBehaviour
{
    private Camera _mainCamera;

    private Material _material;
    private Texture2D _texture;
    private IntVector2 _textureSize;

    [SerializeField] private int brushSize = 3;
    [SerializeField] Color brushColor = Color.black;

    private IntVector2 _prevDrawPosition;

    private void Awake()
    {
        _material = GetComponent<MeshRenderer>().material;
        _mainCamera = Camera.main;

        _textureSize = new IntVector2(Screen.width, Screen.height);
    }

    private void Start()
    {
        CreateClearTexture(_textureSize);
        SetUpPencilCanvas();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _prevDrawPosition = new IntVector2((int) Input.mousePosition.x, (int) Input.mousePosition.y);
        }

        if (Input.GetMouseButton(0))
        {
            IntVector2 currentDrawPosition =
                new IntVector2((int) Input.mousePosition.x, (int) Input.mousePosition.y);

            if (currentDrawPosition == _prevDrawPosition)
            {
                DrawDot(_prevDrawPosition);
            }
            else
            {
                DrawLine(_prevDrawPosition, currentDrawPosition);
                _prevDrawPosition = currentDrawPosition;
            }
        }
    }


    #region Init

    private void SetUpPencilCanvas()
    {
        transform.localScale = new Vector3(_mainCamera.orthographicSize * 2f * _mainCamera.aspect,
            _mainCamera.orthographicSize * 2f, 1f);
        _material.mainTexture = _texture;
    }

    private void CreateClearTexture(IntVector2 size)
    {
        _texture = new Texture2D(size.x, size.y, TextureFormat.RGB24, false) {filterMode = FilterMode.Point};

        Color[] whiteColors = new Color[size.x * size.y];
        for (int j = 0; j < size.y; j++)
        {
            for (int i = 0; i < size.x; i++)
            {
                whiteColors[i + j * size.x] = Color.white;
            }
        }

        _texture.SetPixels(whiteColors);
        _texture.Apply();
    }

    #endregion

    #region Draw

    private void DrawDot(IntVector2 position)
    {
        RectInt rect = PencilUtils.CalculateFillRect(position, _textureSize, brushSize);
        bool[] mask = PencilUtils.CreateDotMask(rect);
        ModifyTexture(mask, rect);
    }

    private void DrawLine(IntVector2 p1, IntVector2 p2)
    {
        RectInt rect = PencilUtils.CalculateFillRect(p1, p2, _textureSize, brushSize);
        bool[] mask = PencilUtils.CreateLineMask(p1, p2, rect, _textureSize, brushSize);

        ModifyTexture(mask, rect);
    }

    private void ModifyTexture(bool[] mask, RectInt rect)
    {
        Color[] colorsToModify = _texture.GetPixels(rect.x, rect.y, rect.width, rect.height);
        PencilUtils.ModifyColors(ref colorsToModify, mask, rect, brushColor);

        _texture.SetPixels(rect.x, rect.y, rect.width, rect.height, colorsToModify);
        _texture.Apply();
    }

    #endregion
}