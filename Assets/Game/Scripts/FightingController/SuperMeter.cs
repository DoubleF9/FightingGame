using System.Collections;
using UnityEngine;

public class SuperMeter : MonoBehaviour
{
    [Header("Super Meter Settings")]
    public float maxMeter = 100f;
    public float currentMeter = 0f;

    [Header("Charge Rates")]
    public float meterGainOnHit = 10f; // Gain when you hit opponent
    public float meterGainOnDamage = 5f; // Gain when you take damage
    public float meterGainOnDodge = 3f; // Gain on successful dodge
    public float passiveMeterGain = 1f; // Gain per second (prevents camping)

    [Header("Super Move Settings")]
    public int superDamage = 40;
    public float superKnockback = 5f;
    public float superRadius = 3f; // Larger than normal attack radius
    public string superAnimationName = "SuperAttackAnimation";
    public KeyCode superActivationKey = KeyCode.Q;

    [Header("References")]
    public SuperMeterUI superMeterUI;
    private FightingController fightingController; // Auto-fetches opponents from here
    private Animator animator;
    private CharacterController characterController;

    [Header("Super Move Audio")]
    public AudioClip superActivationSound; // fire_punch_02.wav
    public AudioClip superImpactSound; // body_hit_finisher_42.wav

    [Header("Super Move State")]
    private bool isSuperReady = false;
    private bool isPerformingSuper = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        fightingController = GetComponent<FightingController>(); // Get reference to share opponents

        if (superMeterUI != null)
        {
            superMeterUI.GiveEmptyMeter(maxMeter);
        }
    }

    void Update()
    {
        // Passive meter gain over time
        if (currentMeter < maxMeter)
        {
            AddMeter(passiveMeterGain * Time.deltaTime);
        }

        // Check for super activation
        if (Input.GetKeyDown(superActivationKey) && isSuperReady && !isPerformingSuper)
        {
            StartCoroutine(PerformSuperMove());
        }
    }

    // Call this when player hits an opponent
    public void OnHitOpponent()
    {
        AddMeter(meterGainOnHit);
    }

    // Call this when player takes damage
    public void OnTakeDamage()
    {
        AddMeter(meterGainOnDamage);
    }

    // Call this when player dodges
    public void OnDodge()
    {
        AddMeter(meterGainOnDodge);
    }

    void AddMeter(float amount)
    {
        currentMeter = Mathf.Min(currentMeter + amount, maxMeter);
        isSuperReady = currentMeter >= maxMeter;

        if (superMeterUI != null)
        {
            superMeterUI.SetMeterValue(currentMeter);
        }

        if (isSuperReady)
        {
            Debug.Log("SUPER READY! Press " + superActivationKey + " to activate!");
        }
    }

    IEnumerator PerformSuperMove()
    {
        isPerformingSuper = true;
        Debug.Log("SUPER MOVE ACTIVATED!");

        // Play activation sound
        if (superActivationSound != null)
        {
            AudioSource.PlayClipAtPoint(superActivationSound, transform.position);
        }

        // Hit stop effect (brief freeze)
        if (ScreenEffects.Instance != null)
        {
            ScreenEffects.Instance.DoHitStop(0.1f);
        }
        yield return new WaitForSecondsRealtime(0.1f);

        // Slow motion during super
        if (ScreenEffects.Instance != null)
        {
            ScreenEffects.Instance.DoSlowMotion(0.3f, 0.5f);
        }

        // Play super animation
        animator.Play(superAnimationName);

        // Wait a bit for the animation to reach the hit frame
        yield return new WaitForSeconds(0.3f);

        // Check for opponents in range and deal damage
        // Use opponents from FightingController (no duplicate setup needed!)
        if (fightingController == null || fightingController.opponents == null)
        {
            Debug.LogWarning("SuperMeter: No FightingController or opponents found!");
            isPerformingSuper = false;
            yield break;
        }

        foreach (Transform opponent in fightingController.opponents)
        {
            // Skip if opponent is missing or inactive
            if (opponent == null || !opponent.gameObject.activeInHierarchy)
                continue;

            if (Vector3.Distance(transform.position, opponent.position) <= superRadius)
            {
                // Play impact sound
                if (superImpactSound != null)
                {
                    AudioSource.PlayClipAtPoint(superImpactSound, opponent.position);
                }

                // Screen shake on impact
                if (ScreenEffects.Instance != null)
                {
                    ScreenEffects.Instance.ShakeScreen(0.4f, 0.3f);
                }

                // Deal damage to opponent
                if (opponent.TryGetComponent<OpponentAI>(out OpponentAI opponentAI))
                {
                    // Deal super damage
                    opponentAI.StartCoroutine(opponentAI.PlayHitDamageAnimation(superDamage));
                    Debug.Log($"SUPER HIT! Dealt {superDamage} damage to {opponent.name}!");

                    // Apply knockback
                    Vector3 knockbackDirection = (opponent.position - transform.position).normalized;
                    if (opponent.TryGetComponent<CharacterController>(out CharacterController opponentController))
                    {
                        opponentController.Move(knockbackDirection * superKnockback);
                    }
                }
            }
        }

        // Empty the super meter
        currentMeter = 0f;
        isSuperReady = false;
        if (superMeterUI != null)
        {
            superMeterUI.EmptyMeter();
        }

        // Wait for animation to finish
        yield return new WaitForSeconds(0.5f);

        isPerformingSuper = false;
        Debug.Log("Super move complete!");
    }

    // Check if super is available (for UI or AI)
    public bool IsSuperReady()
    {
        return isSuperReady;
    }

    // Check if currently performing super (to block other actions)
    public bool IsPerformingSuper()
    {
        return isPerformingSuper;
    }
}
