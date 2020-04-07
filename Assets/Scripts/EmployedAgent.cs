using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EmployedAgent : BaseAgent
{
    private int loadAmount = 1;
    private bool isLoaded;
    private bool isAbandon;

    public override Resource BestResource { get; protected set; }
    public override List<Resource> Memory { get; protected set; }
    public EmployedAgent(GameObject gameObject) : base(gameObject) { }

    public override void OnStart()
    {
        BestResource = Game.Resources.FirstOrDefault();
        movement.SetTarget = BestResource.transform.position;
        movement.Move();
    }

    public override Type OnUpdate()
    {
        gameObject.name = "EmployedAgent";
        movement.Move();

        if (isAbandon)
        {
            isAbandon = false;
            return typeof(UnemployedAgent);
        }
        return typeof(EmployedAgent);
    }

    public override void ShareKnowledge(BaseAgent sharingAgent)
    {
        var memory = Memory.Union(sharingAgent.Memory).ToList();
        BestResource = GetBestResource(memory);
        movement.SetTarget = BestResource.transform.position;
    }

    protected override void LoadResource(Resource currentResource)
    {
        movement.SetTarget = Nest.transform.position;
        currentResource.DecreaseAmount(loadAmount);
        isLoaded = true;
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
                //BestResource = GetBestResource(Memory);
                movement.SetTarget = BestResource.transform.position;
            }
        }
    }

    public override void TriggerEnter(Collider other)
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

    public override void TriggerExit(Collider other)
    {
        if (other.gameObject == Nest.gameObject)
            Nest.GetOut(this);
    }
}