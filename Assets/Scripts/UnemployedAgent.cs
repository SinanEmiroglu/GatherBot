using System;
using UnityEngine;

public class UnemployedAgent : BaseAgent
{
    Resource targetResource;

    public UnemployedAgent(GameObject gameObject) : base(gameObject) { }

    public override Type OnUpdate()
    {
        SetStatus();

        if (targetResource != null)
            return typeof(EmployedAgent);

        return typeof(UnemployedAgent);
    }

    public override void TriggerEnter(Collider2D other)
    {
        nest.OnInfoArrived += InfoArrivedHandler;
    }

    public override void TriggerExit(Collider2D other)
    {
        nest.OnInfoArrived -= InfoArrivedHandler;
    }

    void InfoArrivedHandler()
    {
        Recruit();
    }

    void SetStatus()
    {
        _gameObject.name = "UnemployedAgent";
        _renderer.color = Color.blue;
        movement.SetTarget = nest.transform.position;

        if (movement.IsTargetReached() && Nest.orderedResources.Count > 0)
            Recruit();
    }

    void Recruit()
    {
        targetResource = GetBestResource();
    }
}