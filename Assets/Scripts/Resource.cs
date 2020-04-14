using TMPro;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public TextMeshPro qualityText;
    public int Quality { get; private set; }
    public float Radius { get; private set; }
    public float GetDistanceToNest => Mathf.Sqrt(Vector3.SqrMagnitude(transform.position - Game.Nest.transform.position));
    public bool IsConsumed { get; private set; }
    public bool IsExplored { get; private set; }

    int amount;
    SpriteRenderer _renderer;

    public void DecreaseAmount(int amount)
    {
        this.amount -= amount;
        SetScale();
        if (this.amount <= 3)
            gameObject.SetActive(false);
    }

    public void ExploreResource()
    {
        qualityText.gameObject.SetActive(true);
        _renderer.color = new Color(1f, .8f, .3f);
        IsExplored = true;
    }

    void Awake()
    {
        IsExplored = false;
        amount = Random.Range(5, 15);
        Quality = Random.Range(1, 5);
        Radius = transform.localScale.x * .5f;
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.color = Color.gray;
        gameObject.name = "Resource[" + Quality + "]";
        SetScale();

        for (int i = 0; i < Quality; i++)
            qualityText.text += "+";
    }

    void SetScale() => transform.localScale = new Vector3(1, 1, 0) * amount * 0.15f;
    void OnDisable() => IsConsumed = true;
}