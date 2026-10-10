using UnityEngine;

public class GroundBurstTornado : MonoBehaviour
{


    private Animator _animator;

    const string END_TORNADO = "EndTornado";
    private HorizontalMove _horizontalMove;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _horizontalMove = GetComponent<HorizontalMove>();
    }

    private void Start()
    {
        _horizontalMove.OnMovedToDestination += MovedToDestination;
        _horizontalMove.OnMovedToObstacle += MovedToObstacle;
    }

    private void MovedToDestination()
    {
        _animator.SetTrigger(END_TORNADO);
    }
    private void MovedToObstacle()
    {
        _horizontalMove.StopMove();
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }


}
