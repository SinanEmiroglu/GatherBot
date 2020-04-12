using System;
using UnityEngine;

public class Employed : BaseStatus
{
    int loadAmount = 1;
    bool isLoaded;
    Resource targetResource;

    public Employed(GameObject gameObject) : base(gameObject) { }

    public override void OnEnable()
    {
        _renderer.color = Color.green;
        _gameObject.name = "EmployedAgent";
        _renderer.sortingOrder = 5;

        targetResource = nest.GetBestResource();
        movement.SetTarget = targetResource.transform.position;
        movement.Move();
    }

    public override Type OnUpdate()
    {
        if (movement.IsTargetReached() && targetResource.IsConsumed)
            return typeof(Unemployed);

        return typeof(Employed);
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

            targetResource = nest.GetBestResource();
            movement.SetTarget = targetResource.transform.position;
        }
    }

    void LoadResource()
    {
        _renderer.color = Color.yellow;
        targetResource.DecreaseAmount(loadAmount);
        movement.SetTarget = nest.transform.position;
        isLoaded = true;
    }

    void UnloadResource()
    {
        _renderer.color = Color.green;
        nest.ResourceAmount += loadAmount;
        isLoaded = false;
    }
}