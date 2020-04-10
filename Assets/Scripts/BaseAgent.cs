using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAgent
{
    protected GameObject _gameObject;
    protected Transform _transform;
    protected SpriteRenderer _renderer;
    protected Movement movement;
    protected Nest nest;

    protected Stack<Resource> orderedResources { get; set; }

    public abstract System.Type OnUpdate();

    public virtual void OnEnable() { }
    public virtual void OnDisable() { }
    public virtual void TriggerEnter(Collider2D other) { }
    public virtual void TriggerExit(Collider2D other) { }

    public BaseAgent(GameObject gameObject)
    {
        _gameObject = gameObject;
        _transform = gameObject.transform;
        _renderer = gameObject.GetComponent<SpriteRenderer>();
        movement = gameObject.GetComponent<Movement>();
        nest = Game.Nest;
        //orderedResources = new Stack<Resource>();
    }
}