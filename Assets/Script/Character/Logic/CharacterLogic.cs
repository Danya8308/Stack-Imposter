using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterLogic : MonoBehaviour, IShiftable
{
    [SerializeField] private int _minBoardCountForStatEffect = 6;
    [SerializeField] private CharacterStats _boardEffect = new CharacterStats(7f, 0f, 0f, 0f);

    private bool _canJump = false;
    private bool _speedBufIsActive;
    private List<Board> _boardCounter = new List<Board>();


    public Character CachedCharacter { get; private set; }

    protected void Awake()
    {
        CachedCharacter = GetComponent<Character>();
    }

    protected virtual void FixedUpdate()
    {
        UpdateBoardCounter();
        UpdateJumpCapability();

        if (CachedCharacter.IsJumping == true)
        {
            CachedCharacter.ResetLastPutBoard();

            return;
        }

        if (CheckSurface() == true || TryPutBoard() == true || TryJump() == true)
        {
            return;
        }

        if (CachedCharacter.IsJumping == false)
        {
            CachedCharacter.Die();
        }
    }

    protected virtual void OnEnable()
    {
        CachedCharacter.OnWin += Disable;
    }

    protected virtual void OnDisable()
    {
        CachedCharacter.OnWin -= Disable;
    }

    protected void OnControllerColliderHit(ControllerColliderHit hit)
    {
        UpdateLastPutBoard();

        if (hit.transform.TryGetComponent(out Character collision) == true)
        {
            CollisionWithCharacter(collision);
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Board board) == false)
        {
            return;
        }

        board.Disappear();
        CachedCharacter.TakeBoard();
    }

    public void Shift(Vector3 direction)
    {
        CachedCharacter.Shift(direction);
    }

    protected virtual void CollisionWithCharacter(Character collision)
    {
        
    }

    private bool TryJump()
    {
        if (_canJump == true)
        {
            CachedCharacter.Jump();
            _canJump = false;

            return true;
        }

        return false;
    }

    private bool TryPutBoard()
    {
        if (CachedCharacter.IsJumping == true)
        {
            return false;
        }

        return CachedCharacter.TryPutBoard();
    }

    private void UpdateBoardCounter()
    {
        RaycastHit hitUnderMe = CachedCharacter.GetHitUnderMe();

        if (hitUnderMe.transform is null)
        {
            return;
        }

        if (hitUnderMe.transform.TryGetComponent(out Board board) == true)
        {
            AddToBoardCounter(board);
        }
        else
        {
            ResetBoardCounter();
        }
    }

    private void UpdateLastPutBoard()
    {
        RaycastHit hitUnderMe = CachedCharacter.GetHitUnderMe();

        if (hitUnderMe.transform is not null && CachedCharacter.LastPutBoard != hitUnderMe.transform)
        {
            CachedCharacter.ResetLastPutBoard();
        }
    }

    private void UpdateJumpCapability()
    {
        if (_canJump == false && CheckSurface() == true)
        {
            _canJump = true;
        }
        else if (CachedCharacter.IsJumping == true)
        {
            _canJump = false;
        }
    }

    private void AddToBoardCounter(Board board)
    {
        foreach (var innerBoard in _boardCounter)
        {
            if (innerBoard == board)
            {
                return;
            }
        }

        _boardCounter.Add(board);

        if (_boardCounter.Count < _minBoardCountForStatEffect || _speedBufIsActive == true)
        {
            return;
        }

        _speedBufIsActive = true;
        CachedCharacter.TakeStatEffectWhile(_boardEffect, () => _speedBufIsActive);
    }

    private void ResetBoardCounter()
    {
        _boardCounter.Clear();

        if (_speedBufIsActive == false)
        {
            return;
        }

        _speedBufIsActive = false;
    }

    private bool CheckSurface()
    {
        RaycastHit hitUnderMe = CachedCharacter.GetHitUnderMe();
        return hitUnderMe.collider != null;
    }

    private void Disable()
    {
        enabled = false;
    }
}