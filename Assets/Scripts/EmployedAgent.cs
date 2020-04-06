using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EmployedAgent : BaseAgent
{
    private int loadAmount = 1;
    private bool isLoaded;

    public override List<Resource> Memory { get; protected set; }

    public EmployedAgent(GameObject gameObject) : base(gameObject) { }

    public override void OnStart()
    {
    }
    public override Type OnUpdate()
    {
        gameObject.name = "EmployedAgent";

        //if (IsResourceAbandoned())
        //    return typeof(UnemployedAgent);

        return typeof(EmployedAgent);
    }

    private bool IsResourceAbandoned()
    {
        if (BestResource == null)
        {
            BestResource = Game.Resources.First();
        }
        return BestResource.Amount <= 0;
    }

    public override void ShareKnowledge(BaseAgent sharingAgent)
    {
        var memory = Memory.Union(sharingAgent.Memory).ToList();
        movement.SetTarget = GetBestResourceLocation(memory);
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
            Nest.ResourceAmount += loadAmount;
            movement.SetTarget = BestResource.transform.position;
            isLoaded = false;
        }
    }

    public override void TriggerEnter(Collider other)
    {
        if (other.GetComponent<Resource>() != null)
        {

            Debug.Log("asda");

            if (BestResource == other.GetComponent<Resource>())
            {

                Debug.Log("NAME");

                LoadResource(BestResource);
            }
        }

        if (other.gameObject == Nest.gameObject)
        {
            Nest.GetIn(this);
            //Nest.OnKnowledgeShared(this);
            UnloadResource();
        }
    }

    public override void TriggerExit(Collider other)
    {
        if (other.gameObject == Nest.gameObject)
        {
            Nest.GetOut(this);
        }
    }
}