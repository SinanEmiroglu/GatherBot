using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] float radiusOfArea;
    [SerializeField] int resourceCount;
    [SerializeField] AgentController agentPrefab;
    [SerializeField] Nest nestPrefab;
    [SerializeField] Resource resourcePrefab;

    public static Game instance;
    public static Nest Nest => instance.nestPrefab;
    public static List<Resource> Resources => instance.resources;

    List<Resource> resources = new List<Resource>();

    void Awake()
    {
        if (instance == null)
            instance = this;
        
        GenerateSources();
    }

    void GenerateSources()
    {
        while (resourceCount >= resources.Count)
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

    public void StartGame()
    {
        GetAgent<ExplorerAgent>(1, 8);
        GetAgent<UnemployedAgent>(4, 4);
    }

    float RandomizeSpeed(float value)
    {
        return Random.Range(0.6f, 1.0f) * value;
    }

    void GetAgent<T>(int count, float speed) where T : BaseAgent
    {
        for (int i = 0; i < count; i++)
        {
            var newAgent = Instantiate(agentPrefab, nestPrefab.transform.position, Quaternion.identity);

            var movement = newAgent.GetComponent<Movement>();

            newAgent.SwitchAgent(typeof(T));
            movement.Speed = RandomizeSpeed(speed);
        }
    }

    public Vector3 GetRandomPosition()
    {
        var position = UnityEngine.Random.insideUnitSphere * radiusOfArea + nestPrefab.transform.position;

        return new Vector3(position.x, position.y, 0);
    }
}