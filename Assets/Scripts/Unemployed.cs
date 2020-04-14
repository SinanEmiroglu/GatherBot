using System;
using UnityEngine;

public class Unemployed : BaseStatus
{
    Resource targetResource;

    #region Overridden Methods
    public Unemployed(GameObject gameObject) : base(gameObject) { }
    public override void OnEnable()
    {
        movement.SetTarget = nestPosition;
        _gameObject.name = "UnemployedAgent";
        _renderer.color = Color.blue;
        _renderer.sortingOrder = 1;
        Debug.Log("<color=blue>Unemployed: </color>Waiting for the new information in the nest.");
    }

    public override Type OnUpdate()
    {
        if (targetResource != null)
            if (!targetResource.IsConsumed)
                return typeof(Employed);

        return typeof(Unemployed);
    }

    public override void TriggerEnter(Collider2D other)
    {
        if (other.gameObject == nest.gameObject)
        {
            nest.OnExplorerReturned += ExplorerReturnHandler;
            _renderer.sortingOrder = 1;
            Recruit();
        }
    }

    public override void TriggerExit(Collider2D other)
    {
        if (other.gameObject == nest.gameObject)
            nest.OnExplorerReturned -= ExplorerReturnHandler;
    }
    #endregion

    void Recruit() => targetResource = nest.GetBestResource();
    void ExplorerReturnHandler() => Recruit();
}