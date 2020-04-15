using TMPro;
using UnityEngine;
using System.Collections.Generic;

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
    public static Vector2 GetRandomPoint(float minX, float maxX, float minY, float maxY) => new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

    int agentSizeUI;
    int resourceSizeUI;
    List<Resource> resources = new List<Resource>();

    public void StartGame()
    {
        resourceSizeUI = int.Parse(ResourceSizeUI.text);
        agentSizeUI = int.Parse(AgentSizeUI.text);

        GenerateSources();
        SpawnAgent<Explorer>(1, 10f);
        SpawnAgent<Unemployed>(agentSizeUI, 5f);
    }

    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(0);
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void GenerateSources()
    {
        while (resourceSizeUI >= resources.Count + 1)
        {
            bool isIntersect = false;
            var resource = Instantiate(resourcePrefab, GetRandomPoint(-10, 10, -8, 8), Quaternion.identity);
            var distanceToNest = Vector3.SqrMagnitude(nestPrefab.transform.position - resource.transform.position);

            if (Mathf.Sqrt(distanceToNest) <= resource.Radius + 3)
            {
                Destroy(resource.gameObject);
                isIntersect = true;
            }

            foreach (var src in resources)
            {
                var distanceBetweenSources = Vector3.SqrMagnitude(src.transform.position - resource.transform.position);
                var radiusSum = src.Radius + resource.Radius;

                if (Mathf.Sqrt(distanceBetweenSources) <= radiusSum + 1)
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
            movement.Speed = Random.Range(.5f, 2f) * speed;
        }
    }
}