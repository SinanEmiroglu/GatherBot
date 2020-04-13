using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Agent : MonoBehaviour
{
    BaseStatus currentStatus;
    Dictionary<Type, BaseStatus> availableStatuses;

    public void SwitchStatus(Type nextStatus)
    {
        if (availableStatuses.TryGetValue(nextStatus, out BaseStatus value))
        {
            currentStatus = value;
            currentStatus.OnEnable();
        }
    }

    void Awake()
    {
        var statuses = new Dictionary<Type, BaseStatus>()
        {
            {typeof(Explorer),new Explorer(gameObject) },
            {typeof(Unemployed),new Unemployed(gameObject) },
            {typeof(Employed), new Employed(gameObject) }
        };
        availableStatuses = statuses;
    }

    void Update()
    {
        if (currentStatus == null)
            return;

        var nextStatus = currentStatus?.OnUpdate();

        if (nextStatus != null && nextStatus != currentStatus?.GetType())
            SwitchStatus(nextStatus);
    }

    void OnTriggerEnter2D(Collider2D collision) => currentStatus.TriggerEnter(collision);
    void OnTriggerExit2D(Collider2D collision) => currentStatus.TriggerExit(collision);
    void OnDisable() => currentStatus.OnDisable();
}