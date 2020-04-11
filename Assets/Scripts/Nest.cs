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
    public static List<Resource> orderedResources = new List<Resource>();

    void OnTriggerEnter2D(Collider2D collision)
    {
        amountText.gameObject.SetActive(true);
        amountText.text = ResourceAmount.ToString();
    }

    public void SetExploredResources(List<Resource> resources)
    {
        orderedResources = orderedResources.Union(resources).ToList();
        orderedResources.RemoveAll(r => r == null);

        Dictionary<float, Resource> sortedSources = new Dictionary<float, Resource>();
        float qualityIndex = 0;

        for (int i = 0; i < orderedResources.Count; i++)
        {
            qualityIndex = orderedResources[i].Amount / orderedResources[i].Distance;
            sortedSources.Add(qualityIndex, orderedResources[i]);
        }

        orderedResources = (from entry in sortedSources orderby entry.Key descending select entry.Value).Distinct().ToList();
        OnInfoArrived?.Invoke();
    }
}