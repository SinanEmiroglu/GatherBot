using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BaseAgent
{
    public BaseAgent(GameObject gameObject)
    {
        this.gameObject = gameObject;
        transform = gameObject.transform;
        movement = transform.GetComponent<Movement>();
        Nest = Game.Nest;
        Memory = new List<Resource>();
    }

    protected GameObject gameObject;
    protected Transform transform;
    protected Movement movement;

    public Nest Nest { get; protected set; }
    public Resource BestResource { get; protected set; }
    public abstract List<Resource> Memory { get; protected set; }

    public abstract System.Type OnUpdate();
    public virtual void TriggerEnter(Collider other) { }
    public virtual void TriggerExit(Collider other) { }
    public virtual void OnStart() { }
    public abstract void ShareKnowledge(BaseAgent sharingAgent);
    protected virtual void LoadResource(Resource currentResource) { }
    protected virtual void UnloadResource() { }

    protected Vector3 GetBestResourceLocation(List<Resource> memory)
    {
        Dictionary<float, Resource> sortedSources = new Dictionary<float, Resource>();
        float highestQuality = 0;

        foreach (var src in memory)
        {
            highestQuality = src.Amount / src.Distance;

            sortedSources.Add(highestQuality, src);
        }

        var resource = sortedSources.OrderByDescending(s => s.Key).FirstOrDefault().Value;
        BestResource = resource;

        return resource.transform.position;
    }
}