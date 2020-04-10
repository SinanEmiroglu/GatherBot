using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class UnemployedAgent : BaseAgent
{
    public UnemployedAgent(GameObject gameObject) : base(gameObject) {
        orderedResources = new Stack<Resource>();
    }

    public override Type OnUpdate()
    {
        SetAgentStatus();

        if (orderedResources.Count > 0)
            return typeof(EmployedAgent);

        return typeof(UnemployedAgent);
    }

    public override void OnEnable()
    {
        nest.OnInfoArrived += InfoArrivedHandler;
    }

    public override void OnDisable()
    {
        nest.OnInfoArrived -= InfoArrivedHandler;
    }

    void InfoArrivedHandler(List<Resource> exploredResource)
    {
        if (exploredResource.Count > 0)
        {
            orderedResources = nest.GetOrderedResources();
        }
    }

    void SetAgentStatus()
    {
        _gameObject.name = "UnemployedAgent";
        _renderer.color = Color.blue;
        movement.SetTarget = nest.transform.position;
    }

    
}