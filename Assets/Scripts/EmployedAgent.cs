using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EmployedAgent : BaseAgent
{
    private int loadAmount = 1;
    private bool isLoaded;
    private bool isAbandon;

    public override List<Resource> Memory { get; protected set; }
    public EmployedAgent(GameObject gameObject) : base(gameObject) { }
    Queue<Resource> gatherings;
    public override Type OnUpdate()
    {
        SetAgentStatus();

        if (Memory.Count == 0)
            return typeof(UnemployedAgent);

        if (isAbandon)
        {
            isAbandon = false;
            GetNextResource();
            return typeof(UnemployedAgent);
        }
        return typeof(EmployedAgent);
    }

    void SetAgentStatus()
    {
        gameObject.name = "EmployedAgent";
        renderer.color = Color.green;
        movement.Move();
    }
    private void GetNextResource()
    {

    }

    public override void TriggerEnter(Collider2D other)
    {
        if (other.GetComponent<Resource>() != null)
        {
            if (BestResource == other.GetComponent<Resource>())
            {
                LoadResource(BestResource);
            }
        }

        if (other.gameObject == Nest.gameObject)
        {
            Nest.GetIn(this);
            UnloadResource();
        }
    }

    public override void TriggerExit(Collider2D other)
    {
        if (other.gameObject == Nest.gameObject)
            Nest.GetOut(this);
    }

    public override void GetResourceRecord(List<Resource> res)
    {
        var memory = Memory.Union(res).ToList();
        BestResource = GetBestResource(memory);
        movement.SetTarget = BestResource.transform.position;

    }

    protected override void LoadResource(Resource currentResource)
    {
        //CHECK QUALITY OF THE RESOURCE
        movement.SetTarget = Nest.transform.position;
        currentResource.DecreaseAmount(loadAmount);
        BestResource = GetBestResource();
        isLoaded = true;
    }

    private Resource GetBestResource()
    {
        Resource src = new Resource();
        if (Memory.Count > 0)
        {
            gatherings = new Queue<Resource>(Memory.OrderByDescending(x => x.Amount));
            src = gatherings.Dequeue();
        }

        return src;
    }

    protected override void UnloadResource()
    {
        if (isLoaded)
        {
            if (BestResource == null)
                isAbandon = true;
            else
            {
                Nest.ResourceAmount += loadAmount;
                isLoaded = false;
                movement.SetTarget = BestResource.transform.position;
            }
        }
    }
}