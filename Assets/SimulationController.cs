using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationController : MonoBehaviour
{
    [SerializeField] LineGraph _graph;


    void Start()
    {
        StartCoroutine(DemoGraphAnimation());
    }


    IEnumerator DemoGraphAnimation()
    {
        _graph.Points = GetRandomData();
        _graph.SetMarkerPos(_graph.Points[0]);
        _graph.SetMarkVisibility(false);

        while(true)
        {
            var points = GetRandomData();

            yield return _graph.AnimateToData(points);

            _graph.SetMarkVisibility(true);
            yield return AnimateMark();
            _graph.SetMarkVisibility(false);
        }
    }


    IEnumerator AnimateMark()
    {
        float startTime = Time.time;
        float duration = 2;
        float endTime = startTime + duration;

        while(Time.time < endTime)
        {
            yield return null;

            float t = (Time.time - startTime) / duration;

            int index = Mathf.FloorToInt(Mathf.Lerp(0, _graph.Points.Count - 1, t));

            var currentPoint = _graph.Points[index];
            _graph.SetMarkerPos(currentPoint);
        }
    }


    List<Vector3> GetRandomData()
    {
        var pointCount = _graph.PointCount;
        var points = new List<Vector3>(pointCount);

        float initialX = Random.Range(-200, 200);
        float dy = 0;
        float y = Random.Range(-200, 200);

        for(int i = 0; i < pointCount; i++)
        {
            points.Add(new Vector2(i - initialX, y));
            dy += 2 * Random.value - 1;
            y += dy;
        }

        return points;
    }


    float ArgonArgonPotential(float r)
    {
        return CalculatePotentialEnergy(3.4f, 0.997f, r);
    }
    

    float CalculatePotentialEnergy(float sigma, float epsilon, float radius)
    {
        // Equation taken from https://en.wikipedia.org/wiki/Lennard-Jones_potential

        float s_Over_r = sigma / radius;

        float V = 4 * epsilon * (Mathf.Pow(s_Over_r, 12) - Mathf.Pow(s_Over_r, 6));

        return V;
    }
}
