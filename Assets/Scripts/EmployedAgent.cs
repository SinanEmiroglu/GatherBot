using System;
using UnityEngine;

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

        targetResource = GetBestResource();
        movement.SetTarget = targetResource.transform.position;
        movement.Move();
    }

    public override Type OnUpdate()
    {
        if (targetResource == null)
            return typeof(UnemployedAgent);

        if (Nest.orderedResources.Count <= 0)
            return typeof(UnemployedAgent);

        return typeof(EmployedAgent);
    }

    public override void TriggerEnter(Collider2D other)
    {
        if (other.GetComponent<Resource>() != null)
        {
            if (targetResource == other.GetComponent<Resource>())
                LoadResource();
        }

        if (other.gameObject == nest.gameObject)
        {
            if (isLoaded)
                UnloadResource();
        }
    }

    void LoadResource()
    {
        targetResource.DecreaseAmount(loadAmount);
        movement.SetTarget = nest.transform.position;
        isLoaded = true;
    }

    void UnloadResource()
    {
        nest.ResourceAmount += loadAmount;
        movement.SetTarget = GetBestResource().transform.position;
        isLoaded = false;
    }
}