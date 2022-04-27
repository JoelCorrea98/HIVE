﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Utilities_RadialProgressBar : MonoBehaviour
{
    public Image radialImage;

    [SerializeField] private float maxValue = 1;
    [SerializeField] private float minValue = 0;
    [SerializeField] private float currentValue = 1;

    [SerializeField] private float animSpeed = 4f;

    [SerializeField] protected UnityEvent onFull = null;
    [SerializeField] protected UnityEvent onValueChanged = null;
    [SerializeField] protected UnityEvent onEmpty = null;

    public float MaxValue { get { return maxValue; } }
    public float MinValue { get { return minValue; } }

    private float targetValue;
    private bool animating;
    private float lastSavedValue;
    private System.Action onFinished;

    public void FixedUpdate()
    {
        if (animating)
        {
            currentValue = Mathf.Lerp(currentValue, targetValue, animSpeed * Time.deltaTime);
            bool endCondition = currentValue >= targetValue - 0.1f;

            if (currentValue > targetValue)
            {
                endCondition = currentValue <= targetValue + 0.1f;
            }

            if (endCondition)
            {
                currentValue = targetValue;
                animating = false;
                currentValue = Mathf.Clamp(currentValue, minValue, maxValue);
                onFinished?.Invoke();
            }
            
            UpdateBar();
        }
    }

    public void SetRange(float minValue, float maxValue, bool instant = true)
    {
        this.minValue = minValue;
        SetMaxValue(maxValue, instant);
        if (instant)
        {
            currentValue = minValue;
        }
        else
        {
            SetValue(minValue);
        }
        
        UpdateBar();
    }
     
    public void SetMaxValue(float newMaxValue, bool instant = true)
    {
        float difference =  newMaxValue - maxValue;
        if(gameObject.name == "HP Bar")
        {
            Debug.Log("Hp update");
        }
        if (instant)
        {
            currentValue += difference;
        }
        else
        {
            SetValue(currentValue + difference);
        }

        maxValue = newMaxValue;
        
        UpdateBar();
    }

    public void SetValue(float newValue, System.Action onFinished = null)
    {
        newValue = Mathf.Clamp(newValue, minValue, maxValue);

        if (newValue != lastSavedValue)
        {
            targetValue = newValue;
            lastSavedValue = newValue;
            animating = true;
            this.onFinished = onFinished;
            onValueChanged?.Invoke();
        }

        if(newValue == maxValue)
        {
            onFull?.Invoke();
        }

        if (newValue == minValue)
        {
            onEmpty?.Invoke();
        }

        UpdateBar();
    }
    public void SetValueInstant(float newValue)
    {
        newValue = Mathf.Clamp(newValue, minValue, maxValue);

        if (newValue != lastSavedValue)
        {
            targetValue = newValue;
            currentValue = newValue;
            lastSavedValue = newValue;
            onValueChanged?.Invoke();
        }

        if(newValue == maxValue)
        {
            onFull?.Invoke();
        }

        if (newValue == minValue)
        {
            onEmpty?.Invoke();
        }

        UpdateBar();
    }
    private void UpdateBar()
    {
        float range = maxValue - minValue;
        float newFill = (currentValue - minValue) / range;
        radialImage.fillAmount = newFill;
    }
}

