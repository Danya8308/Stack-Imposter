using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ISubjectToGravity))]
public class Gravity : MonoBehaviour
{
    public event Func<bool> WhenCheckingUseGravityCapability;

    [SerializeField] private float _force = 0.06f;
    [SerializeField] private float _acceleration = 0.02f;

    private IEnumerator _fallCoroutine;

    private ISubjectToGravity _object;

    private float _currentFallRate;


    private bool CanFall => _object.OnSurface == false && WhenCheckingUseGravityCapability?.Invoke() == true;

    private void Awake()
    {
        _object = GetComponent<ISubjectToGravity>();
    }

    private void Update()
    {
        if (_fallCoroutine == null && CanFall == true)
        {
            Enable();
        }
    }

    public void Enable()
    {
        _fallCoroutine = FallCoroutine();
        StartCoroutine(_fallCoroutine);
    }

    public void Disable()
    {
        if (_fallCoroutine != null)
        {
            StopCoroutine(_fallCoroutine);
        }
    }

    private IEnumerator FallCoroutine()
    {
        _currentFallRate = _force;

        while (CanFall == true)
        {
            _object.Shift(Vector3.down * _currentFallRate);

            _currentFallRate += _acceleration;

            yield return new WaitForFixedUpdate();
        }

        _fallCoroutine = null;
    }
}