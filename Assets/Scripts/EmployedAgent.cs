using System;
using UnityEngine;

[DisallowMultipleComponent]
public class EmployedAgent : BaseAgent
{
    int loadAmount = 1;
    bool isLoaded;
    Resource targetResource;

    public EmployedAgent(GameObject gameObject) : base(gameObject) { }

    public override void OnEnable()
    {
        _gameObject.name = "EmployedAgent";
        _renderer.color = Color.green;
        Debug.Log(orderedResources.Count);
        targetResource = GetBestResource();
        movement.SetTarget = targetResource.transform.position;
        movement.Move();
    }

    public override Type OnUpdate()
    {
        if (orderedResources.Count <= 0)
            return typeof(UnemployedAgent);

        return typeof(EmployedAgent);
    }

    public override void TriggerEnter(Collider2D other)
    {
        if (other.GetComponent<Resource>() != null)
        {
            if (targetResource == other.GetComponent<Resource>())
            {
                LoadResource(targetResource);
            }
        }

        if (other.gameObject == nest.gameObject)
        {
            UnloadResource();

            GetBestResource();
        }
    }

    void LoadResource(Resource currentResource)
    {
        movement.SetTarget = nest.transform.position;
        currentResource.DecreaseAmount(loadAmount);
        isLoaded = true;
    }

    void UnloadResource()
    {
        if (isLoaded)
        {
            var newTarget = GetBestResource();
            if (targetResource != newTarget)
                Abandon();
            else
            {
                nest.ResourceAmount += loadAmount;
                isLoaded = false;

                movement.SetTarget = targetResource.transform.position;
            }
        }
    }

    void Abandon()
    {
        targetResource = GetBestResource();
    }

    Resource GetBestResource()
    {
        return orderedResources.Peek();
    }
}