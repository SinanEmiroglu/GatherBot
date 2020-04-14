using System;
using UnityEngine;

public class Employed : BaseStatus
{
    int loadAmount = 1;
    bool isLoaded;
    Resource targetResource;

    #region Overridden Methods
    public Employed(GameObject gameObject) : base(gameObject) { }

    public override void OnEnable()
    {
        _renderer.color = Color.white;
        _gameObject.name = "EmployedAgent";
        _renderer.sortingOrder = 5;

        ChaseResource();
    }

    public override Type OnUpdate()
    {
        if (targetResource == null)
            return typeof(Unemployed);

        if (movement.IsTargetReached() && targetResource.IsConsumed)
            return typeof(Unemployed);

        return typeof(Employed);
    }

    public override void TriggerEnter(Collider2D other)
    {
        if (other.GetComponent<Resource>() != null)
            if (targetResource == other.GetComponent<Resource>())
                LoadResource();

        if (other.gameObject == nest.gameObject)
        {
            if (isLoaded)
                UnloadResource();

            ChaseResource();
        }
    }
    #endregion

    void ChaseResource()
    {
        targetResource = nest.GetBestResource();
        if (targetResource != null)
            movement.SetTarget = targetResource.transform.position;

        movement.Move();
    }

    void LoadResource()
    {
        _renderer.color = Color.yellow;
        targetResource.DecreaseAmount(loadAmount);
        movement.SetTarget = nestPosition;
        _renderer.sortingOrder = 1;
        isLoaded = true;

        Debug.Log("<color=yellow>Employed (loaded): </color>Going to the nest to unload " + targetResource.name);
    }

    void UnloadResource()
    {
        _renderer.color = Color.white;
        nest.ResourceAmount += targetResource.Quality * loadAmount;
        _renderer.sortingOrder = 5;
        isLoaded = false;

        Debug.Log("<color=white>Employed (empty): </color>Going to gather " + targetResource.name);
    }
}