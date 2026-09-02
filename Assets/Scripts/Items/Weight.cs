using UnityEngine;

public class Weight : ObjectMoving
{
  
    public bool IsOn = false;
    private Vector3 _targetPosition;
    private float _threashold = 0.1f;

 
    public void Toggle(bool isOn)
    {
        IsOn = isOn;
        _targetPosition= isOn ? EndPosition : StartPosition;
    }

    public override void Move()
    {
        if (Vector3.Distance(transform.position, _targetPosition) > _threashold)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Speed * Time.deltaTime);
        }
      
    }

   


}
