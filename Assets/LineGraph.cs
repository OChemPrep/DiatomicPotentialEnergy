using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGraph : MonoBehaviour
{
    public List<Vector3> Points;

    [SerializeField] RectTransform _content;
    [SerializeField] LineRenderer _dataLine;
    [SerializeField] LineRenderer _xAxis;
    [SerializeField] LineRenderer _yAxis;
    [SerializeField] RectTransform _pointMarker;

    RectTransform _rectTransform;

    public int PointCount => _dataLine.positionCount;

    Vector2 _markerPoint;


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
    }


    public Coroutine AnimateToData(List<Vector3> targetPoints)
    {
        Coroutine c = StartCoroutine(AnimateToPoints(targetPoints));

        Points = targetPoints;

        return c;
    }


    IEnumerator AnimateToPoints(List<Vector3> targetPoints)
    {
        float startTime = Time.time;
        float duration = 2;
        float endTime = startTime + duration;

        var initialPoints = new List<Vector3>(Points);
        var currentPoints = new List<Vector3>(Points);

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
        _markerPoint = currentPoint;

        RefreshMarkerPos();
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


    void SetPositions(List<Vector3> points)
    {
        _dataLine.SetPositions(points.ToArray());

        if(points.Count > 0)
        {
            Bounds bounds = new Bounds(points[0], Vector3.zero);

            foreach(var point in points)
            {
                bounds.Encapsulate(point);
            }

            if(bounds.size.x > 0 && bounds.size.y > 0)
            {
                Vector3 contentScale = _rectTransform.rect.size / bounds.size;
                contentScale.z = 1;
                _content.localScale = contentScale;

                Vector3 originOffset = -bounds.center * (Vector2)_content.localScale;
                originOffset.z = _content.localPosition.z;
                _content.localPosition = originOffset;

                if(bounds.min.y <= 0 && bounds.max.y >= 0)
                {
                    _xAxis.enabled = true;
                    _xAxis.SetPosition(0, new Vector3(bounds.min.x, 0, 1));
                    _xAxis.SetPosition(1, new Vector3(bounds.max.x, 0, 1));
                }
                else
                {
                    _xAxis.enabled = false;
                }

                if(bounds.min.x <= 0 && bounds.max.x >= 0)
                {
                    _yAxis.enabled = true;
                    _yAxis.SetPosition(0, new Vector3(0, bounds.min.y, 1));
                    _yAxis.SetPosition(1, new Vector3(0, bounds.max.y, 1));
                }
                else
                {
                    _yAxis.enabled = false;
                }
            }

            RefreshMarkerPos();
        }
    }


}
