using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreWheelController : MonoBehaviour
{
    public static Action<float> SetScoreWheelValue;

    private const float _animationDuration = 2f;

    private const float _rangeMin = -90f;
    private const float _rangeMax = 90f;

    private float _relativeValue;
    private float _value;
    private Coroutine _setValueSmooth;

    [SerializeField] private Transform _arrow;

    private void OnEnable()
    {
        SetScoreWheelValue += OnSetScoreWheelValue;
    }

    private void OnDisable()
    {
        SetScoreWheelValue -= OnSetScoreWheelValue;
    }

    private void OnSetScoreWheelValue(float relativeValue)
    {
        if (relativeValue == _relativeValue)
        {
            return;
        }

        if (relativeValue == 1f)
        {
            _arrow.localRotation = Quaternion.Euler(0f, _rangeMax, 0f);
            _value = _rangeMax;

            _relativeValue = relativeValue;
            return;
        }

        if (_setValueSmooth != null)
        {
            StopCoroutine(_setValueSmooth);
            _setValueSmooth = null;
        }

        var range = _rangeMax - _rangeMin;
        var progress = range * relativeValue;
        var targetValue = _rangeMin + progress;

        _relativeValue = relativeValue;
        _setValueSmooth = StartCoroutine(SetValueSmooth(targetValue));

        //_arrow.localRotation = Quaternion.Euler(0f, rotationY, 0f);
    }

    private IEnumerator SetValueSmooth(float value)
    {
        var startValue = _value;
        var timestampStart = Time.time;
        var progress = 0f;

        do
        {
            yield return new WaitForEndOfFrame();
            progress = Mathf.Min((Time.time - timestampStart) / _animationDuration, 1f);

            var smoothValue = Mathf.SmoothStep(startValue, value, progress);
            
            _arrow.localRotation = Quaternion.Euler(0f, smoothValue, 0f);
            _value = smoothValue;
        } while (progress < 1f);

        _setValueSmooth = null;
    }
}
