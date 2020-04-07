using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum AgentStatus { Explorer, Unemployed, Employed }

public class Agent : MonoBehaviour
{
    public AgentStatus Status;
    public Resource BestResource;
    public List<Resource> Memory;
    public Movement Movement { get; private set; }

    int maxResourceInMemory = 4;
    int loadAmount = 1;
    bool isLoaded;
    bool isAbandon;
    Nest nest;

    bool IsMemoryFull => Memory.Count >= maxResourceInMemory;

    public void ShareKnowledge(Agent sharingAgent)
    {
        var memory = Memory.Union(sharingAgent.Memory).ToList();
        BestResource = GetBestResource(memory);
        Movement.SetTarget = BestResource.transform.position;
    }

    void Awake()
    {
        Movement = GetComponent<Movement>();
        nest = Game.Nest;
    }

    void Start()
    {
        switch (Status)
        {
            case AgentStatus.Explorer:
                Movement.Move();
                break;
            case AgentStatus.Unemployed:
                Movement.Stop();
                break;
            case AgentStatus.Employed:
                BestResource = Game.Resources.First();
                Movement.Move();
                break;
            default:
                break;
        }
    }

    void Update()
    {
        switch (Status)
        {
            case AgentStatus.Explorer:
                if (IsMemoryFull)
                    Movement.SetTarget = nest.transform.position;

                if (Movement.IsTargetReached())
                    Movement.SetTarget = Game.instance.GetRandomPosition();
                break;
            case AgentStatus.Unemployed:
                Movement.SetTarget = nest.transform.position;
                break;
            case AgentStatus.Employed:
                if (BestResource == null)
                    Status = AgentStatus.Unemployed;
                break;
            default:
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (Status == AgentStatus.Explorer)
        {
            gameObject.name = "Explorer Agent";
            if (other.GetComponent<Resource>() != null)
                Exploit(other.GetComponent<Resource>());

            if (other.gameObject == nest.gameObject && IsMemoryFull)
            {
                nest.OnKnowledgeShared(this);
                Movement.SetTarget = Game.instance.GetRandomPosition();
            }
        }
        else if (Status == AgentStatus.Employed)
        {
            gameObject.name = "Employed Agent";
            if (other.GetComponent<Resource>() != null)
            {
                if (BestResource == other.GetComponent<Resource>())
                {
                    LoadResource(BestResource);
                }
            }
        }
        else
        {
            gameObject.name = "Unemployed Agent";
        }

        if (other.gameObject == nest.gameObject && Status != AgentStatus.Explorer)
        {
            nest.GetIn(this);
            UnloadResource();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == nest.gameObject)
            nest.GetOut(this);

        if (Status == AgentStatus.Explorer)
        {
            if (other.gameObject == nest.gameObject && IsMemoryFull)
                Memory.Clear();
        }
    }

    void Exploit(Resource currentSource)
    {
        if (!Memory.Contains(currentSource))
        {
            Memory.Add(currentSource);
        }
    }
    void LoadResource(Resource currentResource)
    {
        Movement.SetTarget = nest.transform.position;
        currentResource.DecreaseAmount(loadAmount);
        isLoaded = true;
    }

    void UnloadResource()
    {
        if (isLoaded)
        {
            if (BestResource == null)
                isAbandon = true;
            else
            {
                nest.ResourceAmount += loadAmount;
                isLoaded = false;
                Movement.SetTarget = BestResource.transform.position;
                Movement.Move();
            }
        }
    }

    Resource GetBestResource(List<Resource> memory)
    {
        Dictionary<float, Resource> sortedSources = new Dictionary<float, Resource>();
        float highestQuality = 0;

        foreach (var src in memory)
        {
            highestQuality = src.Amount / src.Distance;

            sortedSources.Add(highestQuality, src);
        }

        var resource = sortedSources.OrderByDescending(s => s.Key).FirstOrDefault().Value;

        return resource;
    }
}