using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    Dictionary<Type, BaseAgent> availableAgents;
    BaseAgent currentAgent;

    public event Action<BaseAgent> OnAgentChanged = delegate { };

    void Awake()
    {
        var availableAg = new Dictionary<Type, BaseAgent>()
        {
            {typeof(ExplorerAgent),new ExplorerAgent(gameObject) },
            {typeof(UnemployedAgent),new UnemployedAgent(gameObject) },
            {typeof(EmployedAgent), new EmployedAgent(gameObject) }
        };
        availableAgents = availableAg;
    }

    void Start() => currentAgent.OnStart();

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

    public void SwitchAgent(Type nextAgent)
    {
        if (availableAgents.TryGetValue(nextAgent, out BaseAgent value))
            currentAgent = value;

        OnAgentChanged?.Invoke(currentAgent);
    }
}