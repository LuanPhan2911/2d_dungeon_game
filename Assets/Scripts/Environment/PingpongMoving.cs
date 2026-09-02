using UnityEngine;

public class PingpongMoving : ObjectMoving
{

    [SerializeField] private float _pauseDuration = 0.5f;

    private float _lerpProgress = 0f;

    private bool _isMovingForward = true;

    private bool _isPaused = false;

    private float _pauseTimer = 0f;

    public override void Move()
    {
        if (_isPaused)
        {
            _pauseTimer += Time.deltaTime;

            if (_pauseTimer > _pauseDuration)
            {
                _isPaused = false;
                _pauseTimer = 0f;
            }
            return;
        }

        if (_isMovingForward)
        {
            _lerpProgress += Time.deltaTime * Speed;
            if (_lerpProgress >= 1f)
            {
                _lerpProgress = 1f;
                _isMovingForward = false;
                _isPaused = true;
            }

        }
        else
        {
            _lerpProgress -= Time.deltaTime * Speed;
            if (_lerpProgress <= 0f)
            {
                _lerpProgress = 0f;
                _isMovingForward = true;
                _isPaused = true;
            }
        }


        float smoothT = Mathf.SmoothStep(0f, 1f, _lerpProgress);


        transform.position=Vector3.Lerp(StartPosition, EndPosition, smoothT);

    }


}
