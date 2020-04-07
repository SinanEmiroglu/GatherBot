using UnityEngine;

public class Movement : MonoBehaviour
{
    public float Speed;

    public Vector3 SetTarget { get; set; }
    public float TargetLength { get; set; }

    private bool isMoving;
    private Transform _transform;
    private void Awake() => _transform = transform;
    public void Move() => isMoving = true;
    public void Stop() => isMoving = false;

    void Update()
    {
        if (!isMoving)
            return;

        _transform.position += Speed * GetDirection() * Time.deltaTime;
    }

    public bool IsTargetReached()
    {
        float distance = Vector3.SqrMagnitude(SetTarget - _transform.position);
        return distance <= .08f;
    }

    Vector3 GetDirection()
    {
        var direction = SetTarget - _transform.position;
        var flatDirection = new Vector3(direction.x, direction.y, 0);
        flatDirection.Normalize();

        return flatDirection;
    }
}