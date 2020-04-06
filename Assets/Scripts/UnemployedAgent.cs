using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnemployedAgent : BaseAgent
{
    public UnemployedAgent(GameObject gameObject) : base(gameObject) { }

    bool hasResourceToTarget;

    public override List<Resource> Memory { get; protected set; }

    public override void OnStart()
    {
    }

    public override Type OnUpdate()
    {
        gameObject.name = "UnemployedAgent";

        if (hasResourceToTarget)
        {
            movement.Move();
            return typeof(EmployedAgent);
        }

        movement.SetTarget = Nest.transform.position;

        return typeof(UnemployedAgent);
    }

    public override void ShareKnowledge(BaseAgent sharingAgent)
    {
        var memory = Memory.Union(sharingAgent.Memory).ToList();
        movement.SetTarget = GetBestResourceLocation(memory);
        hasResourceToTarget = true;
    }

    public override void TriggerEnter(Collider other)
    {
        if (other.gameObject == Nest.gameObject)
        {
            Nest.GetIn(this);
            UnloadResource();
        }
    }

    public override void TriggerExit(Collider other)
    {
        if (other.gameObject == Nest.gameObject)
        {
            Nest.GetOut(this);
        }
    }
}