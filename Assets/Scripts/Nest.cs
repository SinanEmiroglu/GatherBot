using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Nest : MonoBehaviour
{
    public int ResourceAmount;
    public TextMeshPro amountText;
    public event Action OnInfoArrived = delegate { };

    List<Resource> exploredResources = new List<Resource>();

    void OnTriggerEnter2D(Collider2D collision)
    {
        amountText.gameObject.SetActive(true);
        amountText.text = ResourceAmount.ToString();
    }

    public void SetExploredResources(List<Resource> resources)
    {
        exploredResources = exploredResources.Union(resources).ToList();
        OnInfoArrived?.Invoke();
    }

    public Resource GetBestResource()
    {
        Dictionary<float, Resource> sortedSources = new Dictionary<float, Resource>();
        float qualityIndex = 0;

        exploredResources.RemoveAll(r => r == null);

        for (int i = 0; i < exploredResources.Count; i++)
        {
            qualityIndex = exploredResources[i].Amount / exploredResources[i].Distance;
            sortedSources.Add(qualityIndex, exploredResources[i]);
        }

        return (from entry in sortedSources orderby entry.Key descending select entry.Value).Distinct().ToList().FirstOrDefault();
    }
}