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
            {typeof(NullAgent),new NullAgent(gameObject) },
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

    void OnTriggerEnter(Collider other) => currentAgent.TriggerEnter(other);
    void OnTriggerExit(Collider other) => currentAgent.TriggerExit(other);

    public void SwitchAgent(Type nextAgent)
    {
        if (availableAgents.TryGetValue(nextAgent, out BaseAgent value))
            currentAgent = value;

        OnAgentChanged?.Invoke(currentAgent);
    }
}