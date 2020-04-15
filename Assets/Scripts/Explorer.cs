using System;
using System.Collections.Generic;
using UnityEngine;

public class Explorer : BaseStatus
{
    int maxMemory = 3;
    List<Resource> memory = new List<Resource>();
    float timer;
    bool IsMemoryFull => memory.Count >= maxMemory;
    bool IsTimeOut => timer >= 30;

    #region Overridden Methods
    public Explorer(GameObject gameObject) : base(gameObject) { }
    public override void OnEnable()
    {
        _gameObject.name = "ExplorerAgent";
        _renderer.color = Color.green;
        Explore();

        Debug.Log("<color=green><b>Explorer</b>: </color>Getting out to explore new resources.");
    }

    public override Type OnUpdate()
    {
        timer += Time.deltaTime;
        if (IsMemoryFull || IsTimeOut)
        {
            movement.SetTarget = nestPosition;
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

        if (other.gameObject == nest.gameObject && (IsMemoryFull || IsTimeOut))
        {
            memory.RemoveAll(r => r.IsConsumed);
            nest.SetExploredResources(memory);
            Debug.Log("<color=green><b>Explorer</b>: </color>" + memory.Count + " resources are successfully recorded on a list of the explored resources.");
            memory.Clear();
            Explore();
            timer = 0;
            Debug.Log("<color=green><b>Explorer</b>: </color>Getting out to explore new resources.");
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
        movement.SetTarget = Game.GetRandomPoint(-12, 12, -9, 9);
        movement.Move();
    }

    void Exploit(Resource resource)
    {
        if (!IsMemoryFull && !resource.IsExplored)
        {
            resource.ExploreResource();
            memory.Add(resource);

            Debug.Log("<color=green><b>Explorer</b>: </color>" + resource.name + " is just exploited.");
        }
    }
}