using System;
using System.Collections.Generic;
using UnityEngine;

public class Explorer : BaseStatus
{
    int maxMemory = 3;
    List<Resource> memory = new List<Resource>();
    bool IsMemoryFull => memory.Count >= maxMemory;

    public Explorer(GameObject gameObject) : base(gameObject) { }

    public override void OnEnable()
    {
        _gameObject.name = "ExplorerAgent";
        Explore();
    }

    public override Type OnUpdate()
    {
        if (IsMemoryFull) { 
            movement.SetTarget = nest.transform.position;
            _renderer.sortingOrder = 1;
        }

        if (movement.IsTargetReached())
            Explore();

        return typeof(Explorer);
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

    public override void TriggerExit(Collider2D other)
    {
        if (other.gameObject == nest.gameObject && !IsMemoryFull)
            _renderer.sortingOrder = 5;
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