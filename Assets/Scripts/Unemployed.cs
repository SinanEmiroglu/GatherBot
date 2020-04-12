using System;
using UnityEngine;

public class Unemployed : BaseStatus
{
    Resource targetResource;

    public Unemployed(GameObject gameObject) : base(gameObject) { }
    public override void OnEnable() => SetStatus();

    public override Type OnUpdate()
    {
        if (targetResource != null)
        {
            if (!targetResource.IsConsumed)
                return typeof(Employed);
        }

        return typeof(Unemployed);
    }

    public override void TriggerEnter(Collider2D other)
    {
        if (other.gameObject == nest.gameObject)
        {
            nest.OnInfoArrived += InfoArrivedHandler;
            _renderer.sortingOrder = 1;
            Recruit();
        }
    }

    public override void TriggerExit(Collider2D other)
    {
        if (other.gameObject == nest.gameObject)
            nest.OnInfoArrived -= InfoArrivedHandler;
    }

    void InfoArrivedHandler()
    {
        Recruit();
    }

    void SetStatus()
    {
        movement.SetTarget = nest.transform.position;
        _gameObject.name = "UnemployedAgent";
        _renderer.color = Color.red;

        Debug.Log("<color=red>Unemployed: </color>Waiting for the new information in the nest.");
    }

    void Recruit() 
    {
        if (nest.GetBestResource() == null)
            return;

        targetResource = nest.GetBestResource(); 
    }
}