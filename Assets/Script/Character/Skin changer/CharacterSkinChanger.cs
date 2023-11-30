using UnityEngine;

public class CharacterSkinChanger : MonoBehaviour
{
    [SerializeField] private Transform _bodyLocation;

    [SerializeField] private Transform _body;

    [SerializeField] private Transform _board;


    public Transform Body => _body;

    public Transform Board => _board;

    public void SetBody(int value)
    {
        Transform skin = GetSkin("Character", value);

        if (skin is null)
        {
            return;
        }

        if (_body is not null)
        {
            Destroy(Body.gameObject);
        }

        _body = Instantiate(skin, _bodyLocation.position, _bodyLocation.rotation, _bodyLocation);
    }

    public void SetBoard(int value)
    {
        Transform skin = GetSkin("Board", value);

        if (skin is not null)
        {
            _board = skin;
        }
    }

    private Transform GetSkin(string type, float value)
    {
        return Resources.Load<Transform>($"Skin/{type}/{value}/Body");
    }
}