using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Nest : MonoBehaviour
{
    public int ResourceAmount;

    List<BaseAgent> agentsInNest = new List<BaseAgent>();

    public void GetIn(BaseAgent agent) => agentsInNest.Add(agent);
    public void GetOut(BaseAgent agent) => agentsInNest.Remove(agent);

    public void OnKnowledgeShared(List<Resource> resources)
    {
        for (int i = 0; i < agentsInNest.Count; i++)
        {
            agentsInNest[i].GetResourceRecord(resources);
        }
    }
}