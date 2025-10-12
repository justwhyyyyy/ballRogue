using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    private bool isSlowing = false;
    private float defaultFixedDeltaTime;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // 保持在场景切换中不被销毁

        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void SlowTime(float slowScale, float duration)
    {
        if (!isSlowing)
        {
            StartCoroutine(SlowTimeRoutine(slowScale, duration));
        }
    }

    private IEnumerator SlowTimeRoutine(float slowScale, float duration)
    {
        isSlowing = true;

        Time.timeScale = slowScale;
        Time.fixedDeltaTime = defaultFixedDeltaTime * slowScale;

        yield return new WaitForSecondsRealtime(duration); // 不受 timeScale 影响

        Time.timeScale = 1f;
        Time.fixedDeltaTime = defaultFixedDeltaTime;
        isSlowing = false;
    }
}
