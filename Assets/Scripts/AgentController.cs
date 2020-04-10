using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    BaseAgent currentAgent;
    Dictionary<Type, BaseAgent> availableAgents;

    public void SwitchAgent(Type nextAgent)
    {
        if (availableAgents.TryGetValue(nextAgent, out BaseAgent value))
        {
            currentAgent = value;
            currentAgent.OnEnable();
        }
    }

    void Awake()
    {
        var agents = new Dictionary<Type, BaseAgent>()
        {
            {typeof(ExplorerAgent),new ExplorerAgent(gameObject) },
            {typeof(UnemployedAgent),new UnemployedAgent(gameObject) },
            {typeof(EmployedAgent), new EmployedAgent(gameObject) }
        };
        availableAgents = agents;
    }

    void Update()
    {
        if (currentAgent == null)
            currentAgent = availableAgents.Values.First();

        var nextAgent = currentAgent?.OnUpdate();

        if (nextAgent != null && nextAgent != currentAgent?.GetType())
            SwitchAgent(nextAgent);
    }

    void OnTriggerEnter2D(Collider2D collision) => currentAgent.TriggerEnter(collision);
    void OnTriggerExit2D(Collider2D collision) => currentAgent.TriggerExit(collision);
    void OnDisable() => currentAgent.OnDisable();
}