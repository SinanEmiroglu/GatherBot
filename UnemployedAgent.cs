using System;
using System.Collections.Generic;
using UnityEngine;

public class UnemployedAgent : BaseAgent
{

    public override void Initialize()
    {

    }

    public override Type Update()
    {
        if (HasResourceToCollect())
            return typeof(EmployedAgent);

        return typeof(UnemployedAgent);
    }

    private bool HasResourceToCollect()
    {
        throw new NotImplementedException();
    }
}