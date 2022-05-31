using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public PlayerAnimator anim;
    public List<Renderer> renderers;
    private void Awake()
    {
        EventManager.Instance?.Subscribe("OnEnergyUpdated", OnEnergyUpdated);
    }

    #region Blink Effect
    private float _currentBlink = 0;
    private float BlinkingValue {
        get
        {
            return _currentBlink;
        } 
        set
        {
            _currentBlink = value;
            foreach (var renderer in renderers)
            {
                renderer.material.SetFloat(IsBlinking, value);
            }
        } 
    }
    private static readonly int IsBlinking = Shader.PropertyToID("_isBlinking");
    private static readonly int BlinkingSpeed = Shader.PropertyToID("_BlinkingSpeed");
    private static readonly int EmissiveColor = Shader.PropertyToID("_EmissiveColor");
    public void Blink(float duration, Action callback = null)
    {
        if (Blinking)
        {
            StopCoroutine(_blink);
        }

        _blink = StartCoroutine(BlinkingCoroutine(duration, callback));
    }
    public void Blink(float duration, float speed)
    {
        SetBlinkSpeed(speed);
        
        Blink(duration);
    }
    public void Blink(float duration, Color color)
    {
        SetBlinkColor(color);
        
        Blink(duration);
    }
    public void Blink(float duration, float speed, Color color)
    {
        SetBlinkSpeed(speed);
        SetBlinkColor(color);
        
        Blink(duration);
    }

    void SetBlinkSpeed(float speed)
    {
        foreach (var renderer in renderers)
        {
            renderer.material.SetFloat(BlinkingSpeed, speed);
        }
    }

    void SetBlinkColor(Color color)
    {
        foreach (var renderer in renderers)
        {
            renderer.material.SetColor(EmissiveColor, color);
        }
    }
    private bool Blinking { get { return _blink != null; } }
    
    private Coroutine _blink = null;
    IEnumerator BlinkingCoroutine(float duration, Action callback = null)
    {
        float transitionValue = 0.01f;
        while (BlinkingValue < 1)
        {
            BlinkingValue += transitionValue;
            yield return new WaitForSeconds(0);
        }
        BlinkingValue = 1;

        yield return new WaitForSeconds(duration);
        
        while (BlinkingValue > 0)
        {
            BlinkingValue -= transitionValue;
            yield return new WaitForSeconds(0);
        }
        BlinkingValue = 0;
        callback?.Invoke();
    }
    #endregion
    
    private static readonly int EmissionFill = Shader.PropertyToID("_EmissionFill");
    private void OnEnergyUpdated(params object[] parameters)
    {
        float current = (float) parameters[0];
        float max = (float) parameters[1];

        float range = max - current;
        float newFill = current / max;
        foreach (var renderer in renderers)
        {
            renderer.material.SetFloat(EmissionFill, newFill);
        }
    }
}
