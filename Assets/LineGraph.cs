using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGraph : MonoBehaviour
{
    public List<Vector2> Points;

    [SerializeField] RectTransform _content;
    [SerializeField] LineRenderer _dataLine;
    [SerializeField] LineRenderer _xAxis;
    [SerializeField] LineRenderer _yAxis;
    [SerializeField] RectTransform _pointMarker;

    RectTransform _rectTransform;

    public int PointCount => _dataLine.positionCount;

    Vector2 _markerPoint;

    public bool IncludeOrigin { get; set; } = false;

    private Bounds _bounds;
    public Bounds Bounds
    {
        get { return _bounds; }
        set { _bounds = value; RefreshBounds(); }
    }


    void Awake()
    {
        _rectTransform = transform as RectTransform;
        _xAxis.positionCount = 2;
        _yAxis.positionCount = 2;
    }


    void Start()
    {
        var pointCount = Mathf.FloorToInt(_rectTransform.rect.width / 2);
        _dataLine.positionCount = pointCount;

        for(int i = 0; i < pointCount; i++)
        {
            Points.Add(Vector2.zero);
        }
    }


    public Coroutine AnimateToData(List<Vector2> targetPoints)
    {
        Coroutine c = StartCoroutine(AnimateToPoints(targetPoints));

        Points = targetPoints;

        return c;
    }


    IEnumerator AnimateToPoints(List<Vector2> targetPoints)
    {
        float startTime = Time.time;
        float duration = 2;
        float endTime = startTime + duration;

        var initialPoints = new List<Vector2>(Points);
        var currentPoints = new List<Vector2>(Points);

        while(Time.time < endTime)
        {
            yield return null;

            float t = (Time.time - startTime) / duration;

            for(int i = 0; i < currentPoints.Count; i++)
            {
                currentPoints[i] = Vector3.Lerp(initialPoints[i], targetPoints[i], t);
            }

            SetPositions(currentPoints);
        }
    }


    internal void SetMarkerPos(Vector3 currentPoint)
    {
        if(currentPoint.sqrMagnitude < float.MaxValue)
        {
            _markerPoint = currentPoint;

            RefreshMarkerPos();
        }
    }


    void RefreshMarkerPos()
    {
        var worldPoint = _content.TransformPoint(_markerPoint);
        _pointMarker.anchoredPosition = transform.InverseTransformPoint(worldPoint);
    }


    public void SetMarkVisibility(bool visible)
    {
        _pointMarker.gameObject.SetActive(visible);
    }


    public void SetPoints(List<Vector2> points)
    {
        Points = points;

        SetPositions(Points);
    }


    void SetPositions(List<Vector2> points)
    {
        _dataLine.positionCount = points.Count;

        if(points.Count > 0)
        {
            for(int i = 0; i < points.Count; i++)
            {
                _dataLine.SetPosition(i, points[i]);
            }

            RefreshBounds();

            RefreshMarkerPos();
        }
    }


    void RefreshBounds()
    {
        var points = Points;

        //if(_bounds.size.x == 0)
        {
            _bounds = new Bounds(points[0], Vector2.zero);

            if(IncludeOrigin)
            {
                _bounds.Encapsulate(Vector2.zero);
            }

            foreach(var point in points)
            {
                _bounds.Encapsulate(point);
            }
        }


        if(_bounds.size.x > 0 && _bounds.size.y > 0)
        {
            Vector3 contentScale = _rectTransform.rect.size / _bounds.size;
            contentScale.z = 1;
            _content.localScale = contentScale;

            Vector3 originOffset = -_bounds.center * (Vector2)_content.localScale;
            originOffset.z = _content.localPosition.z;
            _content.localPosition = originOffset;

            if(_bounds.min.y <= 0 && _bounds.max.y >= 0)
            {
                _xAxis.enabled = true;
                _xAxis.SetPosition(0, new Vector3(_bounds.min.x, 0, 1));
                _xAxis.SetPosition(1, new Vector3(_bounds.max.x, 0, 1));
            }
            else
            {
                _xAxis.enabled = false;
            }

            if(_bounds.min.x <= 0 && _bounds.max.x >= 0)
            {
                _yAxis.enabled = true;
                _yAxis.SetPosition(0, new Vector3(0, _bounds.min.y, 1));
                _yAxis.SetPosition(1, new Vector3(0, _bounds.max.y, 1));
            }
            else
            {
                _yAxis.enabled = false;
            }
        }
    }

}
