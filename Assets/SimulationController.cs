using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationController : MonoBehaviour
{
    [SerializeField] LineGraph _graph;
    [SerializeField] AxisDragHandler AxisDragHandler;
    [SerializeField] AtomController LeftAtom;
    [SerializeField] AtomController RightAtom;


    void Awake()
    {
        AxisDragHandler.DistanceChanged += OnDistanceChanged;
        LeftAtom.Refreshed += RefreshGraph;
        RightAtom.Refreshed += RefreshGraph;
    }


    private void OnDistanceChanged(float distance)
    {
        float radius = distance;
        float energy = CalculatePotentialEnergy(LeftAtom.Element, RightAtom.Element, radius);

        _graph.SetMarkerPos(new Vector3(radius, energy));
    }


    void Start()
    {
        RefreshGraph();

        //StartCoroutine(DemoGraphAnimation());
        //StartCoroutine(DemoMarker());
    }


    IEnumerator DemoMarker()
    {

        while(true)
        {
            yield return AnimateMark();
            yield return new WaitForSeconds(2);
        }
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


    private void RefreshGraph()
    {
        _graph.IncludeOrigin = true;
        Element a = LeftAtom.Element;
        Element b = RightAtom.Element;
        float sigma = CalculateSigma(a, b);

        var points = CalculatePotentialEnergyPoints(a, b, sigma * 0.95f, 1000f, _graph.PointCount);
        if(_graph.Bounds.size.x == 0 || _graph.PointCount == 0)
            _graph.SetPoints(points);
        else
            _graph.AnimateToData(points);

        Vector2 minimumPoint = new Vector2(0, float.PositiveInfinity);
        foreach(var point in points)
        {
            if(point.y < minimumPoint.y)
            {
                minimumPoint = point;
            }
        }

        _graph.SetMarkerPos(minimumPoint);
    }


    List<Vector2> CalculatePotentialEnergyPoints(Element a, Element b, float minRadius, float maxRadius, int numPoints)
    {
        var values = new List<Vector2>(numPoints);

        var sigma = CalculateSigma(a, b);
        float epsilon = BondHelper.GetDiatomicBondStrength(a, b);

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
        var sigma = CalculateSigma(a, b);

        float epsilon = BondHelper.GetDiatomicBondStrength(a, b);

        return CalculatePotentialEnergy(sigma, epsilon, radius);
    }


    float CalculateSigma(Element a, Element b)
    {
        var sigma = ElementHelper.GetVanDerWaalRadius(a) + ElementHelper.GetVanDerWaalRadius(b); // (pm)

        return sigma;
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
