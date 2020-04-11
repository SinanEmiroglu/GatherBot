using TMPro;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public TextMeshPro amountText;
    public int Amount { get; private set; }
    public float Radius { get; private set; }
    public float Distance => GetDistanceToNest();
    public bool IsDestroyed { get; private set; }
    public bool IsExplored { get; private set; }

    SpriteRenderer _renderer;

    public void DecreaseAmount(int amount)
    {
        Amount -= amount;

        SetScale();

        if (Amount <= 1)
            Destroy(gameObject);
    }

    public void ExploreResource()
    {
        amountText.gameObject.SetActive(true);
        amountText.text = Amount.ToString();
        _renderer.color = Color.yellow;
        IsExplored = true;
    }

    void Awake()
    {
        IsExplored = false;
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.color = Color.gray;
        Amount = Random.Range(5, 20);
        SetScale();
        Radius = GetComponent<CircleCollider2D>().radius;
        gameObject.name = "Resource[" + Amount + "]";
    }

    void OnTriggerEnter2D(Collider2D collision) => amountText.text = Amount.ToString();

    void SetScale() => transform.localScale = new Vector3(1, 1, 0) * Amount * 0.1f;

    float GetDistanceToNest()
    {
        var distance = Vector3.SqrMagnitude(transform.position - Game.Nest.transform.position);
        return distance * distance;
    }

    void OnDisable() => IsDestroyed = true;
}