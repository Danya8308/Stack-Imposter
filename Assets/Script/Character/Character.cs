using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour, ISubjectToGravity
{
    public static event Action<Character> OnSpawn;

    public event Action OnWin;
    public event Action OnDie;
    public event Action<int> OnTakeBoard;
    public event Action OnTakeStatEffect;

    [SerializeField] private ModifiableCharacterStats _stats;

    [Space(5f)]

    [SerializeField] private Gravity _gravity;
    [SerializeField] private VerticalGrid _boardStorage;
    [SerializeField] private CharacterSkinChanger _skinChanger;

    [Header("Skills:")]

    [SerializeField] private Jump _jump;
    [SerializeField] private BridgeCreating _bridgeCreating;

    [HideInInspector] public bool SecondLive;


    public CharacterStats CurrentStats => _stats.Current;

    public int BoardCount => _boardStorage == null ? 0 : _boardStorage.Count;

    public bool OnSurface => CachedCharacterController.isGrounded;

    public bool IsJumping => _jump.Active;

    public bool IsWinner { get; private set; }

    public Transform LastPutBoard => _bridgeCreating.LastPutBoard;

    // MonoComponents:
    public CharacterController CachedCharacterController { get; private set; }

    protected void Awake()
    {
        CachedCharacterController = GetComponent<CharacterController>();
        _skinChanger = GetComponent<CharacterSkinChanger>();
    }

    protected void OnEnable()
    {
        OnSpawn?.Invoke(this);
        _gravity.WhenCheckingUseGravityCapability += CheckUseGravityCapability;
    }

    protected void OnDisable()
    {
        _gravity.WhenCheckingUseGravityCapability -= CheckUseGravityCapability;
    }

    public void RotateOnHorizontal(float value)
    {
        transform.rotation *= Quaternion.Euler(transform.up * value);
    }

    public void Move(Vector3 localDirection)
    {
        if (_jump.Active == true)
        {
            return;
        }

        Shift(localDirection * Time.deltaTime * CurrentStats.Speed);
    }

    public void Jump()
    {
        _jump.Launch(this);
    }

    public void StopJump()
    {
        _jump.Stop();
    }

    public bool TryPutBoard()
    {
        if (_boardStorage.IsEmpty == true || GetHitUnderMe().collider is not null)
        {
            return false;
        }

        _bridgeCreating.PutBoard(_skinChanger.Board);
        _boardStorage.RemoveLast();

        return true;
    }

    public void TakeBoards(int count)
    {
        _boardStorage.Add(_skinChanger.Board, count);
        OnTakeBoard?.Invoke(count);
    }

    public void TakeBoard()
    {
        _boardStorage.Add(_skinChanger.Board);
        OnTakeBoard?.Invoke(1);
    }

    public void ResetLastPutBoard()
    {
        _bridgeCreating.ResetLastPutBoard();
    }

    public void TakeStatEffectWhile(CharacterStats effect, Func<bool> condition)
    {
        _stats.TakeStatEffectWhile(this, effect, condition);
        OnTakeStatEffect?.Invoke();
    }

    public void Win()
    {
        IsWinner = true;

        Destroy(_boardStorage.gameObject);

        OnWin?.Invoke();
    }

    public void Die()
    {
        if (IsWinner == true)
        {
            return;
        }

        OnDie?.Invoke();

        if (SecondLive == true)
        {
            SecondLive = false;
        }
        else
        {
            DieEvent();
        }
    }

    public void ShiftToTarget(Vector3 target)
    {
        Shift(target - transform.position);
    }

    public void Shift(Vector3 localDirection)
    {
        if (localDirection == Vector3.zero)
        {
            return;
        }

        Vector3 directionRelativeCharacter = transform.TransformDirection(localDirection);

        CachedCharacterController.Move(directionRelativeCharacter);
    }

    public RaycastHit GetHitUnderMe()
    {
        Transform characterTransform = transform;
        RaycastHit hit;

        Vector3 checkBoxPosition = characterTransform.position + CachedCharacterController.center;

        Vector3 checkBoxHalfSize = CachedCharacterController.bounds.extents * 0.3f;
        checkBoxHalfSize.y = 0f;

        Physics.BoxCast(checkBoxPosition, checkBoxHalfSize, -characterTransform.up, out hit);

        return hit;
    }

    protected virtual void DieEvent()
    {
        Destroy(gameObject);
    }

    private bool CheckUseGravityCapability()
    {
        return _jump.Active == false;
    }
}