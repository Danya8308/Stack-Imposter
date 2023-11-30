using UnityEngine;

public class BridgeCreating : MonoBehaviour
{
    [SerializeField] private Transform _builder;
    [SerializeField] private float _distanceBetweenBoards = 1f;
    [SerializeField] private float _skinWidth = 0f;


    public Transform LastPutBoard { get; private set; }

    public void PutBoard(Transform boardPrefab)
    {
        var newBoard = Instantiate(boardPrefab);

        newBoard.position = LastPutBoard is null ? GetNewFirstBoardPosition(newBoard) : GetNewNextBoardPosition();
        newBoard.rotation = _builder.rotation;

        LastPutBoard = newBoard;
    }

    public void ResetLastPutBoard()
    {
        LastPutBoard = null;
    }

    private Vector3 GetNewFirstBoardPosition(Transform board)
    {
        Vector3 boardPosition;

        if (_builder.TryGetComponent(out Collider builderCollider) == true)
        {
            boardPosition = builderCollider.bounds.center - _builder.up * (builderCollider.bounds.extents.y + _skinWidth);
        }
        else
        {
            boardPosition = _builder.position;
        }

        if (board.TryGetComponent(out Collider boardCollider) == true)
        {
            boardPosition -= _builder.up * boardCollider.bounds.extents.y;
        }

        return boardPosition;
    }

    private Vector3 GetNewNextBoardPosition()
    {
        Vector3 boardPosition = new Vector3()
        {
            x = _builder.position.x,
            y = LastPutBoard.position.y,
            z = _builder.position.z,
        };

        Vector3 directionToNewBoard = boardPosition - LastPutBoard.position;
        float factor = (_distanceBetweenBoards / directionToNewBoard.magnitude) - 1;

        boardPosition += directionToNewBoard * factor;

        return boardPosition;
    }
}