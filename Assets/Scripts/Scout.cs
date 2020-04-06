using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scout : Bee
{
    [SerializeField] private int unitSize = 1;
    [SerializeField] private int maxMemory = 4;

    public override int Size => unitSize;
    public override HashSet<Resource> FoodSourcesInMemory { get; set; } = new HashSet<Resource>();
    public override void Performed(Bee dancingBee) { }

    private Movement movement;

    private void Awake() => movement = GetComponent<Movement>();
    private void Start() => StartCoroutine(ExploreAfterDelay());

    private void Update()
    {
        if (IsMemoryFull())
            //movement.ReturnNest();

        if (movement.IsTargetReached() && !IsMemoryFull())
            Explore();
    }
    IEnumerator ExploreAfterDelay()
    {
        yield return new WaitForSeconds(1);
        Explore();
    }
    private void InformOtherBees()
    {
        if (IsMemoryFull())
        {
            //Game.instance.OnWaggleDance(this);
            FoodSourcesInMemory.Clear();
        }
    }

    private bool IsMemoryFull()
    {
        return FoodSourcesInMemory.Count >= maxMemory;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Resource>() != null)
        {
            Exploit(other.GetComponent<Resource>());
        }

        if (other.gameObject == Game.Nest.gameObject)
        {
            InformOtherBees();
        }
    }

    private void Exploit(Resource currentSource)
    {
        if (!FoodSourcesInMemory.Contains(currentSource))
        {
            FoodSourcesInMemory.Add(currentSource);
        }
    }

    private void Explore()
    {
        movement.SetTarget = Game.instance.GetRandomPosition();
        movement.Move();
    }
}