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
        renderer = transform.GetComponent<SpriteRenderer>();
        Nest = Game.Nest;
        Memory = new List<Resource>();
    }

    protected GameObject gameObject;
    protected Transform transform;
    protected SpriteRenderer renderer;
    protected Movement movement;
    protected Nest Nest;

    public static Resource BestResource;

    public abstract List<Resource> Memory { get; protected set; }
    public abstract System.Type OnUpdate();
    public virtual void TriggerEnter(Collider2D other) { }
    public virtual void TriggerExit(Collider2D other) { }
    public virtual void OnStart() { }
    public virtual void GetResourceRecord(List<Resource> res) { }
    protected virtual void LoadResource(Resource currentResource) { }
    protected virtual void UnloadResource() { }

    protected Resource GetBestResource(List<Resource> memory)
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