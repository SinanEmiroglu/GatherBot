using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Employed : Bee
{
    public override int Size { get; }
    public override HashSet<Resource> FoodSourcesInMemory { get; set; }
    private Movement movement;
    private Resource bestFoodSource;
    private void Awake() => movement = GetComponent<Movement>();
    public override void Performed(Bee dancingBee)
    {
        FoodSourcesInMemory.Concat(dancingBee.FoodSourcesInMemory);
        movement.SetTarget = GetBestSourceTarget();
    }
    private Vector3 GetBestSourceTarget()
    {
        SortedList<Resource, float> sortedSources = new SortedList<Resource, float>();
        float highestQuality = 0;

        foreach (var source in FoodSourcesInMemory)
        {
            highestQuality = source.Amount / source.Distance;

            sortedSources.Add(source, highestQuality);
        }
        bestFoodSource = sortedSources.OrderByDescending(s => s.Value).FirstOrDefault().Key;

        return bestFoodSource.transform.position;
    }
}