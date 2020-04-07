using System.Collections.Generic;
using UnityEngine;

public class Nest : MonoBehaviour
{
    public int ResourceAmount;

    private List<Agent> agentsInNest = new List<Agent>();

    private void Awake()
    {
        ResourceAmount = 150;
    }

    public void GetIn(Agent agent) => agentsInNest.Add(agent);
    public void GetOut(Agent agent) => agentsInNest.Remove(agent);

    public void OnKnowledgeShared(Agent sharingAgent)
    {
        for (int i = 0; i < agentsInNest.Count; i++)
        {
            agentsInNest[i].ShareKnowledge(sharingAgent);
        }
    }
}