using System.Collections;
using UnityEngine;

public class ScreenEffects : MonoBehaviour
{
    // Singleton instance for easy access
    public static ScreenEffects Instance { get; private set; }

    [Header("Camera Reference")]
    public Camera mainCamera;
    private Vector3 originalCameraPosition;

    [Header("Screen Shake Settings")]
    public float defaultShakeDuration = 0.3f;
    public float defaultShakeIntensity = 0.2f;

    void Awake()
    {
        // Setup singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Get camera reference if not assigned
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }


    }

    // Call this to shake the screen
    public void ShakeScreen(float duration, float intensity)
    {
        if (mainCamera != null)
        {
            StartCoroutine(DoScreenShake(duration, intensity));
        }
    }

    // Shortcut method with default values
    public void ShakeScreen()
    {
        ShakeScreen(defaultShakeDuration, defaultShakeIntensity);
    }

    IEnumerator DoScreenShake(float duration, float intensity)
    {
        float elapsed = 0f;

        // 1. Capture the position RIGHT NOW (relative to the parent moving with the player)
        Vector3 currentOriginalPos = mainCamera.transform.localPosition;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * intensity;
            float y = Random.Range(-1f, 1f) * intensity;

            // 2. Shake relative to that captured position
            mainCamera.transform.localPosition = currentOriginalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 3. Return to the captured position
        mainCamera.transform.localPosition = currentOriginalPos;
    }

    // Slow motion effect for super moves
    public void DoSlowMotion(float slowFactor, float duration)
    {
        StartCoroutine(SlowMotionCoroutine(slowFactor, duration));
    }

    IEnumerator SlowMotionCoroutine(float slowFactor, float duration)
    {
        Time.timeScale = slowFactor;
        Time.fixedDeltaTime = 0.02f * slowFactor; // Adjust physics timestep

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

    // Brief pause for impact (hit stop effect)
    public void DoHitStop(float duration)
    {
        StartCoroutine(HitStopCoroutine(duration));
    }

    IEnumerator HitStopCoroutine(float duration)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }
}
