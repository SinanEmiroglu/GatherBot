using UnityEngine;

public class Movement : MonoBehaviour
{
    public float Speed;
    public Vector2 SetTarget { get; set; }

    bool isMoving;
    Transform _transform;
    public void Move() => isMoving = true;
    public void Stop() => isMoving = false;
    public bool IsTargetReached() => Vector2.SqrMagnitude(SetTarget - (Vector2)_transform.position) <= .09f;

    void Awake() => _transform = transform;

    void Update()
    {
        if (isMoving)
            _transform.position = (Vector2)_transform.position + (Speed * GetDirection() * Time.deltaTime);
    }

    Vector2 GetDirection() => (SetTarget - (Vector2)_transform.position).normalized;
}