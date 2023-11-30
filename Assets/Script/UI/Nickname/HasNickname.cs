using UnityEngine;

public class HasNickname : MonoBehaviour
{
    [SerializeField] private Transform _location;


    protected void Awake()
    {
        NicknameCreator.Create(_location, this);
    }

    public virtual string Get()
    {
        return "Zero";
    }
}