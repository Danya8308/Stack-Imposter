using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PointCreator : MonoBehaviour
{
    private Camera _camera;
    private Transform _pointsParent;
    private List<Transform> _points = new List<Transform>();


    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0) == true || Input.GetMouseButton(0) == true)
        {
            CreateNewPoint();
        }

        DisplayPoints();
    }

    private void CreateNewPoint()
    {
        if (_pointsParent is null)
        {
            _pointsParent = new GameObject("Points").transform;
            _pointsParent.position = Vector3.zero;
        }

        Ray screenRay = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Physics.Raycast(screenRay, out hit);

        string newPointName = _points.Count.ToString();
        Transform newPoint = new GameObject(newPointName).transform;

        newPoint.SetParent(_pointsParent);

        _points.Add(newPoint);

        newPoint.position = hit.point;
    }

    private void DisplayPoints()
    {
        foreach (var point in _points)
        {
            Debug.DrawRay(point.position, point.up * 10f, Color.red);
        }
    }
}