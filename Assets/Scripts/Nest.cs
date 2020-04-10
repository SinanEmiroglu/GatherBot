using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Nest : MonoBehaviour
{
    public int ResourceAmount;
    public event Action<List<Resource>> OnInfoArrived = delegate { };

    List<Resource> exploredResources = new List<Resource>();

    public void SetExploredResources(List<Resource> resources)
    {
        exploredResources = exploredResources.Union(resources).ToList();
        OnInfoArrived?.Invoke(exploredResources);
    }

    public Stack<Resource> GetOrderedResources()
    {
        Dictionary<float, Resource> sortedSources = new Dictionary<float, Resource>();
        float qualityIndex = 0;

        for (int i = 0; i < exploredResources.Count; i++)
        {
            qualityIndex = exploredResources[i].Amount / exploredResources[i].Distance;

            sortedSources.Add(qualityIndex, exploredResources[i]);
        }

        var resources = (from entry in sortedSources orderby entry.Key descending select entry.Value).Distinct().ToList();

        return new Stack<Resource>(resources);
    }
}