using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ExplorerAgent : BaseAgent
{
    int maxMemory = 4;
    bool IsMemoryFull => memory.Count >= maxMemory;

    List<Resource> memory = new List<Resource>();

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

    void Exploit(Resource currentSource)
    {
        if (!memory.Contains(currentSource) && !IsMemoryFull)
        {
            currentSource.Explored();
            memory.Add(currentSource);
        }
    }
}