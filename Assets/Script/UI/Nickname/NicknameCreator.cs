using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class NicknameCreator : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;

    private static Dictionary<HasNickname, FollowingNickname> _followers = new Dictionary<HasNickname, FollowingNickname>();


    public static Camera MainCamera { get; private set; }

    public static RectTransform CachedRectTransform { get; private set; }

    public static void Create(Transform target, HasNickname requester)
    {
        FollowingNickname followerPrefab = Resources.Load<FollowingNickname>(FollowingNickname.PrefabPath);
        FollowingNickname newFollower;

        if (CachedRectTransform == null)
        {
            newFollower = Instantiate(followerPrefab);
            newFollower.enabled = false;
        }
        else
        {
            newFollower = Instantiate(followerPrefab, CachedRectTransform.transform);
        }

        newFollower.Target = target;
        newFollower.Requester = requester;

        _followers[requester] = newFollower;
    }

    public static void RemoveFollower(HasNickname requester)
    {
        if (_followers.ContainsKey(requester) == false)
        {
            return;
        }

        Destroy(_followers[requester].gameObject);
        _followers.Remove(requester);
    }

    public static string GetRandom()
    {
        var part1 = new string[] { "Ge", "Me", "Ta", "Bo", "Ke", "Ra", "Ne", "Mi" };
        var part2 = new string[] { "oo", "ue", "as", "to", "ra", "me", "io", "so" };
        var part3 = new string[] { "se", "matt", "lace", "fo", "cake", "end" };

        return part1[Random.Range(0, part1.Length)] + part2[Random.Range(0, part2.Length)] + part3[Random.Range(0, part3.Length)];
    }

    protected void Awake()
    {
        CachedRectTransform = GetComponent<RectTransform>();
        MainCamera = _mainCamera;

        foreach (var follower in _followers.Values)
        {
            if (follower == null)
            {
                continue;
            }

            follower.transform.SetParent(transform);
            follower.enabled = true;
        }
    }
}