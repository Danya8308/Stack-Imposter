using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private Transform _transform;


    protected void Awake()
    {
        _transform = transform;
    }

    protected void Update()
    {
        if (_target == null)
        {
            return;
        }

        FollowPosition();
        FollowRotation();
    }

    private void FollowPosition()
    {
        _transform.position = Vector3.Lerp(
            _transform.position, _target.position, 1f);
    }

    private void FollowRotation()
    {
        _transform.rotation = Quaternion.LookRotation(
            _target.forward, _target.up);
    }
}