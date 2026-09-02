using UnityEngine;

public class ObjectMoving : MonoBehaviour
{

    public bool CanStanding = true;
    public float Speed = 0.5f;
  


    public Vector3 StartPosition, EndPosition;
    private void OnDrawGizmos()
    {


        Gizmos.color = Color.red;
        Gizmos.DrawSphere(StartPosition, 0.3f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(EndPosition, 0.3f);


    }


    [ContextMenu(nameof(SetStartPosition))]
    public void SetStartPosition()
    {
        StartPosition = transform.position;
    }
    [ContextMenu(nameof(SetEndPosition))]
    public void SetEndPosition()
    {
        EndPosition = transform.position;
    }

    private void Start()
    {
        transform.position = StartPosition;
    }
    private void Update()
    {
        Move();
    }

    public virtual void Move()
    {
        float t = Mathf.PingPong(Time.time * Speed, 1f);
        transform.position = Vector3.Lerp(StartPosition, EndPosition, t);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!CanStanding) return;
        if (collision.collider.TryGetComponent(out Player player))
        {
            player.transform.SetParent(transform);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(!CanStanding) return;
        if (collision.collider.TryGetComponent(out Player player))
        {
            player.transform.SetParent(null);
        }
    }
}
