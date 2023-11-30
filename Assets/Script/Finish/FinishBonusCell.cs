using System;
using UnityEngine;

public class FinishBonusCell : MonoBehaviour
{
    public event Action<FinishBonusCell> OnPassed;

    [SerializeField] private int _coinsFactor;
    [SerializeField] private bool _isLast;
    [SerializeField] private Transform _characterTarget;
    [SerializeField] private Transform _cameraLocation;


    public int CoinsFactor => _coinsFactor;

    public bool IsLast => _isLast;

    public Transform CharacterTarget => _characterTarget;

    public Transform CameraLocation => _cameraLocation;

    public bool IsPassed { get; private set; }

    protected void OnTriggerEnter(Collider other)
    {
        if (IsPassed == true || other.TryGetComponent(out Character winner) == false)
        {
            return;
        }

        IsPassed = true;
        OnPassed?.Invoke(this);

        SystemMessenger.Send($"x{_coinsFactor}!");
    }
}