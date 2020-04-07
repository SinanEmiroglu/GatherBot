using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnemployedAgent : BaseAgent
{
    public UnemployedAgent(GameObject gameObject) : base(gameObject) { }
    public override List<Resource> Memory { get; protected set; }
    Queue<Resource> gatheringOrder;
    public override Type OnUpdate()
    {
        SetAgentStatus();

        if (BestResource != null)
            return typeof(EmployedAgent);

        movement.SetTarget = Nest.transform.position;

        return typeof(UnemployedAgent);
    }

    void SetAgentStatus()
    {
        gameObject.name = "UnemployedAgent";
        renderer.color = Color.blue;
    }

    public override void GetResourceRecord(List<Resource> resources)
    {
        var memory = Memory.Union(resources).ToList();
        gatheringOrder = new Queue<Resource>(memory.OrderByDescending(x => x.Amount));
        // BestResource = GetBestResource(memory);

    }

    public override void TriggerEnter(Collider2D other)
    {
        if (other.gameObject == Nest.gameObject)
            Nest.GetIn(this);
    }

    public override void TriggerExit(Collider2D other)
    {
        if (other.gameObject == Nest.gameObject)
            Nest.GetOut(this);
    }
    IEnumerator GatheringResources()
    {
        if (gatheringOrder.Count > 0)
        {
            BestResource = gatheringOrder.Dequeue();
            movement.SetTarget = BestResource.transform.position;
        }
        yield return new WaitUntil(() => BestResource == null);

    }
}