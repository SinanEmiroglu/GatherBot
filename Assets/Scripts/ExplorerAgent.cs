using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExplorerAgent : BaseAgent
{
    int maxMemory = 4;
    bool IsMemoryFull => Memory.Count >= maxMemory;

    public override List<Resource> Memory { get; protected set; }
    List<Resource> resourceRecord = new List<Resource>();
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

    public override void TriggerEnter(Collider2D other)
    {
        if (other.GetComponent<Resource>() != null)
        {
            Exploit(other.GetComponent<Resource>());
        }

        if (other.gameObject == Nest.gameObject && IsMemoryFull)
        {
            resourceRecord = resourceRecord.Union(Memory).ToList();
            Nest.OnKnowledgeShared(resourceRecord);

            Memory.Clear();
            movement.SetTarget = Game.instance.GetRandomPosition();
        }
    }

    void Exploit(Resource currentSource)
    {
        if (!Memory.Contains(currentSource) && !IsMemoryFull)
        {
            currentSource.Explored();
            Memory.Add(currentSource);
        }
    }
}