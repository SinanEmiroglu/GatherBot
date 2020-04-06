using System;
using System.Collections.Generic;
using UnityEngine;

public class ExplorerAgent : BaseAgent
{
    int maxMemory = 4;
    bool IsMemoryFull => Memory.Count >= maxMemory;

    public override List<Resource> Memory { get; protected set; }

    public ExplorerAgent(GameObject gameObject) : base(gameObject) { }

    public override void OnStart()
    {
        gameObject.name = "ExplorerAgent";
        movement.SetTarget = Nest.transform.position;
        movement.Move();
    }

    public override Type OnUpdate()
    {
        if (IsMemoryFull)
            movement.SetTarget = Nest.transform.position;

        if (movement.IsTargetReached())
            movement.SetTarget = Game.instance.GetRandomPosition();

        return typeof(ExplorerAgent);
    }

    public override void TriggerEnter(Collider other)
    {
        if (other.GetComponent<Resource>() != null)
        {
            Exploit(other.GetComponent<Resource>());
        }

        if (other.gameObject == Nest.gameObject && IsMemoryFull)
        {
            Nest.OnKnowledgeShared(this);
            movement.SetTarget = Game.instance.GetRandomPosition();
        }
    }
    public override void TriggerExit(Collider other)
    {
        if (other.gameObject == Nest.gameObject && IsMemoryFull)
        {
            Memory.Clear();
        }
    }
    void Exploit(Resource currentSource)
    {
        if (!Memory.Contains(currentSource))
        {
            Memory.Add(currentSource);
        }
    }

    public override void ShareKnowledge(BaseAgent sharingAgent) { }
}