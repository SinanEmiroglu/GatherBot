using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    public float radiusOfArea;
    public Agent agentPrefab;
    public Nest nestPrefab;
    public Resource resourcePrefab;
    public TextMeshProUGUI AgentSizeUI;
    public TextMeshProUGUI ResourceSizeUI;

    public static Game instance;
    public static Nest Nest => instance.nestPrefab;

    int agentCount;
    int resourceCount;
    List<Resource> resources = new List<Resource>();

    public void StartGame()
    {
        resourceCount = int.Parse(ResourceSizeUI.text);
        agentCount = int.Parse(AgentSizeUI.text);

        GenerateSources();
        SpawnAgent<Explorer>(1, 10f);
        SpawnAgent<Unemployed>(agentCount, 3f);
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void GenerateSources()
    {
        while (resourceCount >= resources.Count + 1)
        {
            bool isIntersect = false;
            var resource = Instantiate(resourcePrefab, GetRandomPosition(), Quaternion.identity);
            var distanceToNest = Vector3.SqrMagnitude(nestPrefab.transform.position - resource.transform.position);

            if (distanceToNest <= (resource.Radius * resource.Radius) + 6f)
            {
                Destroy(resource.gameObject);
                isIntersect = true;
            }

            foreach (var src in resources)
            {
                var distanceBetweenSources = Vector3.SqrMagnitude(src.transform.position - resource.transform.position);
                var radiusSum = (src.Radius + resource.Radius) * (src.Radius + resource.Radius);

                if (distanceBetweenSources <= radiusSum)
                {
                    Destroy(resource.gameObject);
                    isIntersect = true;
                    break;
                }
            }

            if (!isIntersect)
                resources.Add(resource);
        }
    }

    void SpawnAgent<T>(int count, float speed) where T : BaseStatus
    {
        for (int i = 0; i < count; i++)
        {
            var newAgent = Instantiate(agentPrefab, nestPrefab.transform.position, Quaternion.identity);

            var movement = newAgent.GetComponent<Movement>();

            newAgent.SwitchStatus(typeof(T));
            movement.Speed = Random.Range(0.6f, 1.2f) * speed;
        }
    }

    Vector2 GetRandomPosition()
    {
        var position = UnityEngine.Random.insideUnitCircle * radiusOfArea + (Vector2)nestPrefab.transform.position;
        return new Vector2(position.x, position.y);
    }
}