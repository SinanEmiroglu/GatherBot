using UnityEngine;

public class Resource : MonoBehaviour
{
    public int Amount { get; private set; }
    public float Radius { get; private set; }
    public float Distance => GetDistanceToNest();
    SpriteRenderer renderer;
    public void DecreaseAmount(int amount)
    {
        Amount -= amount;

        SetScale();

        if (Amount <= 1)
            Destroy(gameObject);
    }

    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        renderer.color = Color.black;
        Amount = Random.Range(5, 20);
        SetScale();
        Radius = GetComponent<CircleCollider2D>().radius;
    }

    void SetScale() => transform.localScale = new Vector3(1, 1, 0) * Amount * 0.1f;

    float GetDistanceToNest()
    {
        var distance = Vector3.SqrMagnitude(transform.position - Game.Nest.transform.position);
        return distance * distance;
    }
    public void Explored() 
    {
        renderer.color = Color.yellow;
    }
    void OnDisable() => Game.Resources.Remove(this);
}