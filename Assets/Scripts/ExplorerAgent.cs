using System;
using System.Collections.Generic;
using UnityEngine;

public class ExplorerAgent : BaseAgent
{
    int maxMemory = 3;
    List<Resource> memory = new List<Resource>();
    bool IsMemoryFull => memory.Count >= maxMemory;

    public ExplorerAgent(GameObject gameObject) : base(gameObject) { }

    public override void OnEnable()
    {
        _gameObject.name = "ExplorerAgent";
        Explore();
    }

    public override Type OnUpdate()
    {
        if (IsMemoryFull)
            movement.SetTarget = nest.transform.position;

        if (movement.IsTargetReached())
            Explore();

        return typeof(ExplorerAgent);
    }

    public override void TriggerEnter(Collider2D other)
    {
        if (other.GetComponent<Resource>() != null)
            Exploit(other.GetComponent<Resource>());

        if (other.gameObject == nest.gameObject && IsMemoryFull)
        {
            memory.RemoveAll(n => n == null);
            nest.SetExploredResources(memory);
            memory.Clear();
            Explore();
        }
    }

    void Explore()
    {
        movement.SetTarget = Game.instance.GetRandomPosition();
        movement.Move();
    }

    void Exploit(Resource resource)
    {
        if (!IsMemoryFull && !resource.IsExplored)
        {
            resource.ExploreResource();
            memory.Add(resource);
        }
    }
}