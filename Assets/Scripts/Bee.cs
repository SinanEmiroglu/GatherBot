using System.Collections.Generic;
using UnityEngine;

public abstract class Bee : MonoBehaviour
{
    public abstract int Size { get; }

    public abstract HashSet<Resource> FoodSourcesInMemory { get; set; }

    public abstract void Performed(Bee dancingBee);
}