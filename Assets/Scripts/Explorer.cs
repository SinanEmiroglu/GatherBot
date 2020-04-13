using System;
using System.Collections.Generic;
using UnityEngine;

public class Explorer : BaseStatus
{
    int maxMemory = 3;
    List<Resource> memory = new List<Resource>();
    bool IsMemoryFull => memory.Count >= maxMemory;

    #region Overridden Methods
    public Explorer(GameObject gameObject) : base(gameObject) { }
    public override void OnEnable()
    {
        _gameObject.name = "ExplorerAgent";
        Explore();

        Debug.Log("<color=white>Explorer: </color>Getting out to explore new resources.");
    }

    public override Type OnUpdate()
    {
        if (IsMemoryFull)
        {
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
            memory.RemoveAll(r => r.IsConsumed);
            nest.SetExploredResources(memory);
            Debug.Log("<color=white>Explorer: </color>" + memory.Count + " resources are successfully recorded on a list of the explored resources.");
            memory.Clear();
            Explore();
            Debug.Log("<color=white>Explorer: </color>Getting out to explore new resources.");
        }
    }

    public override void TriggerExit(Collider2D other)
    {
        if (other.gameObject == nest.gameObject && !IsMemoryFull)
            _renderer.sortingOrder = 5;
    }
    #endregion

    void Explore()
    {
        movement.SetTarget = GetRandomDirection();
        movement.Move();
    }

    void Exploit(Resource resource)
    {
        if (!IsMemoryFull && !resource.IsExplored)
        {
            resource.ExploreResource();
            memory.Add(resource);

            Debug.Log("<color=white>Explorer: </color>" + resource.name + " is just exploited.");
        }
    }
    //REPEAT
    Vector2 GetRandomDirection()
    {
        Vector2 randomPoint;
        Vector2 direction;

        do
        {
            randomPoint = UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(2f, 10f) + nest.transform.position;
            direction = randomPoint - (Vector2)_transform.position;
        }
        while (Vector2.Dot(direction, randomPoint) < 0.5f);

        return randomPoint;
    }
}