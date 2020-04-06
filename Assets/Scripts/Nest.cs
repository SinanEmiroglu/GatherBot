using System.Collections.Generic;
using UnityEngine;

public class Nest : MonoBehaviour
{
    public int ResourceAmount;

    private List<BaseAgent> agentsInNest = new List<BaseAgent>();

    private void Awake()
    {
        ResourceAmount = 150;
    }

    public void GetIn(BaseAgent agent) => agentsInNest.Add(agent);
    public void GetOut(BaseAgent agent) => agentsInNest.Remove(agent);

    public void OnKnowledgeShared(BaseAgent sharingAgent)
    {
        //if (sharingAgent.Memory.Count > 0)
       // {
            for (int i = 0; i < agentsInNest.Count; i++)
            {
                agentsInNest[i].ShareKnowledge(sharingAgent);
            }
      //  }
    }
}