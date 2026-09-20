using System.Collections;
using UnityEngine;

public class HitStopManager : MonoBehaviour
{
    public static HitStopManager Instance { get; private set; }

    private bool isWaiting;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Hàm gọi Hit Stop từ bên ngoài
    public void TriggerHitStop(float duration, float timeScaleValue = 0.05f)
    {
        if (isWaiting) return; // Tránh việc đè chồng các hiệu ứng Hit Stop liên tục
        StartCoroutine(HitStopCoroutine(duration, timeScaleValue));
    }

    private IEnumerator HitStopCoroutine(float duration, float timeScaleValue)
    {
        isWaiting = true;

        // Lưu lại Fixed Delta Time gốc để không làm hỏng tính toán vật lý của Unity
        float originalFixedDeltaTime = Time.fixedDeltaTime;

        // Giảm tốc độ thời gian hệ thống
        Time.timeScale = timeScaleValue;
        // Cập nhật lại fixedDeltaTime tương ứng với timeScale mới để vật lý mượt mà
        Time.fixedDeltaTime = originalFixedDeltaTime * Time.timeScale;

        // Sử dụng WaitForSecondsRealtime vì WaitForSeconds thông thường sẽ bị ảnh hưởng bởi timeScale
        yield return new WaitForSecondsRealtime(duration);

        // Trả mọi thứ về trạng thái bình thường
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = originalFixedDeltaTime;

        isWaiting = false;
    }
}
