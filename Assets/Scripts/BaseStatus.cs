using UnityEngine;

public abstract class BaseStatus
{
    protected GameObject _gameObject;
    protected Transform _transform;
    protected SpriteRenderer _renderer;
    protected Movement movement;
    protected Nest nest;
    protected Vector2 nestPosition;

    public abstract System.Type OnUpdate();
    public virtual void OnEnable() { }
    public virtual void OnDisable() { }
    public virtual void TriggerEnter(Collider2D other) { }
    public virtual void TriggerExit(Collider2D other) { }

    public BaseStatus(GameObject gameObject)
    {
        _gameObject = gameObject;
        _transform = gameObject.transform;
        _renderer = gameObject.GetComponent<SpriteRenderer>();
        movement = gameObject.GetComponent<Movement>();
        nest = Game.Nest;
        nestPosition = nest.transform.position;
    }
}