using UnityEngine;

public class Resource : MonoBehaviour
{
    public int Amount { get; private set; }
    public float Radius { get; private set; }
    public float Distance => GetDistanceToNest();

    void Awake()
    {
        Amount = Random.Range(5, 20);
        SetScale();
        Radius = transform.localScale.x * 0.5f;
    }
    public void DecreaseAmount(int amount)
    {
        Amount -= amount;

        SetScale();

        if (Amount <= 1)
            Destroy(gameObject);
    }
    void SetScale()
    {
        transform.localScale = new Vector3(1, 1, 0) * Amount * 0.2f;
    }

    float GetDistanceToNest()
    {
        var distance = Vector3.SqrMagnitude(transform.position - Game.Nest.transform.position);
        return distance * distance;
    }

    void OnDisable() => Game.Resources.Remove(this);
}