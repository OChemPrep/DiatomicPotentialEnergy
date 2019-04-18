using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationController : MonoBehaviour
{
    [SerializeField] LineGraph _graph;


    void Start()
    {
        _graph.IncludeOrigin = true;
        _graph.SetPoints(CalculatePotentialEnergyPoints(Element.Argon, Element.Argon, 350f, 1000f, _graph.PointCount));

        Vector2 minimumPoint = new Vector2(0, float.PositiveInfinity);
        foreach(var point in _graph.Points)
        {
            if(point.y < minimumPoint.y)
            {
                minimumPoint = point;
            }
        }

        _graph.SetMarkerPos(minimumPoint);

        //StartCoroutine(DemoGraphAnimation());
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


    List<Vector2> GetRandomData()
    {
        var pointCount = _graph.PointCount;
        var points = new List<Vector2>(pointCount);

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


    List<Vector2> CalculatePotentialEnergyPoints(Element a, Element b, float minRadius, float maxRadius, int numPoints)
    {
        var values = new List<Vector2>(numPoints);

        var sigma = ElementHelper.GetVanDerWaalRadius(a) + ElementHelper.GetVanDerWaalRadius(b); // (pm)
        float epsilon = 0.997f;// (kJ/mol) Value for Argon+Argon interaction. TODO: Make this dependent on the parameter elements.

        float radius = minRadius;
        float radiusDelta = (maxRadius - minRadius) / (numPoints - 1);
        
        for(int i = 0; i < numPoints; i++)
        {
            values.Add(new Vector2(radius, CalculatePotentialEnergy(sigma, epsilon, radius)));
            radius += radiusDelta;
        }

        return values;
    }


    float CalculatePotentialEnergy(Element a, Element b, float radius)
    {
        var sigma = ElementHelper.GetVanDerWaalRadius(a) + ElementHelper.GetVanDerWaalRadius(b); // (pm)
        float epsilon = 0.997f;// (kJ/mol) Value for Argon+Argon interaction. TODO: Make this dependent on the parameter elements.

        return CalculatePotentialEnergy(sigma, epsilon, radius);
    }


    float ArgonArgonPotential(float r)
    {
        return CalculatePotentialEnergy(3.4f, 0.997f, r);
    }


    float CalculatePotentialEnergy(float sigma, float epsilon, float radius)
    {
        // Equation taken from https://en.wikipedia.org/wiki/Lennard-Jones_potential

        float s_r = sigma / radius;
        float sr6 = s_r * s_r * s_r * s_r * s_r * s_r;

        float V = 4 * epsilon * ((sr6 * sr6) - sr6);

        return V;
    }
    
}
