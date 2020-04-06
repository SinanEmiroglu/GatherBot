using System;
using System.Collections.Generic;
using UnityEngine;

public class NullAgent : BaseAgent
{
    public NullAgent(GameObject gameObject) : base(gameObject)
    {
    }

    public override List<Resource> Memory { get; protected set; }

    public override Type OnUpdate()
    {
        return typeof(NullAgent);
    }

    public override void ShareKnowledge(BaseAgent sharingAgent)
    {

    }

    public override void TriggerEnter(Collider other)
    {
    }

    public override void TriggerExit(Collider other)
    {
    }
}