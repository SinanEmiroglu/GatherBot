using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Onlooker : Bee
{
    [SerializeField] private int unitSize = 5;
    [SerializeField] private int loadSize = 1;

    public override HashSet<Resource> FoodSourcesInMemory { get; set; } = new HashSet<Resource>();
    public override int Size => unitSize;

    private Movement movement;
    private Resource bestFoodSource;

    private void Awake() => movement = GetComponent<Movement>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Resource>() != null)
        {
            if (bestFoodSource == other.GetComponent<Resource>())
            {
                GatherNectar(other.GetComponent<Resource>());
            }
        }

        if (other.gameObject == Game.Nest.gameObject)
        {
            //Game.instance.Register(this);
            UnloadNectar();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Game.Nest)
        {
            //Game.instance.Unregister(this);
        }
    }

    public override void Performed(Bee dancingBee)
    {
        FoodSourcesInMemory.UnionWith(dancingBee.FoodSourcesInMemory);
        Employ();
    }

    private void Employ()
    {
        movement.SetTarget = GetBestSourceToTarget();
    }
    bool isLoaded;
    private void GatherNectar(Resource currentSource)
    {
        if (!FoodSourcesInMemory.Contains(currentSource))
        {
            currentSource.DecreaseAmount(loadSize);
            isLoaded = true;
           // movement.ReturnNest();
        }
    }

    private void UnloadNectar()
    {
        if (isLoaded)
        {
            Game.Nest.ResourceAmount += loadSize;
            movement.SetTarget = bestFoodSource.transform.position;
            isLoaded = false;
        }
    }

    private Vector3 GetBestSourceToTarget()
    {
        SortedList<Resource, float> sortedSources = new SortedList<Resource, float>();
        float highestQuality = 0;

        foreach (var source in FoodSourcesInMemory)
        {
            highestQuality = source.Amount / source.Distance;

            sortedSources.Add(source, highestQuality);
            Debug.Log(sortedSources.Count);
        }
        bestFoodSource = sortedSources.OrderByDescending(s => s.Value).FirstOrDefault().Key;

        return bestFoodSource.transform.position;
    }
}