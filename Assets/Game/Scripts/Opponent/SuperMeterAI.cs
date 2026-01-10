using System.Collections;
using UnityEngine;

public class SuperMeterAI : MonoBehaviour
{
    [Header("Super Meter Settings")]
    public float maxMeter = 100f;
    public float currentMeter = 0f;

    [Header("Charge Rates")]
    public float meterGainOnHit = 10f; // Gain when AI hits player
    public float meterGainOnDamage = 5f; // Gain when AI takes damage
    public float passiveMeterGain = 1f; // Gain per second

    [Header("Super Move Settings")]
    public int superDamage = 40;
    public float superKnockback = 5f;
    public float superRadius = 3f;
    public string superAnimationName = "SuperAttackAnimation";
    public float superUseChance = 0.7f; // 70% chance to use super when ready and in range

    [Header("References")]
    public SuperMeterUI superMeterUI;
    private OpponentAI opponentAI; // Auto-fetches players from here
    private Animator animator;
    private CharacterController characterController;

    [Header("Super Move Audio")]
    public AudioClip superActivationSound;
    public AudioClip superImpactSound;

    [Header("Super Move State")]
    private bool isSuperReady = false;
    private bool isPerformingSuper = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        opponentAI = GetComponent<OpponentAI>(); // Get reference to share players

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

        // AI decides when to use super
        if (isSuperReady && !isPerformingSuper)
        {
            TryUseSuperMove();
        }
    }

    void TryUseSuperMove()
    {
        // Use players from OpponentAI (no duplicate setup needed!)
        if (opponentAI == null || opponentAI.players == null)
            return;

        // Check if any player is in range
        foreach (Transform player in opponentAI.players)
        {
            if (player == null || !player.gameObject.activeInHierarchy)
                continue;

            if (Vector3.Distance(transform.position, player.position) <= superRadius)
            {
                // Random chance to use super (so AI doesn't instantly use it)
                if (Random.value < superUseChance * Time.deltaTime)
                {
                    StartCoroutine(PerformSuperMove());
                    break;
                }
            }
        }
    }

    // Call this when AI hits a player
    public void OnHitPlayer()
    {
        AddMeter(meterGainOnHit);
    }

    // Call this when AI takes damage
    public void OnTakeDamage()
    {
        AddMeter(meterGainOnDamage);
    }

    void AddMeter(float amount)
    {
        currentMeter = Mathf.Min(currentMeter + amount, maxMeter);
        isSuperReady = currentMeter >= maxMeter;

        if (superMeterUI != null)
        {
            superMeterUI.SetMeterValue(currentMeter);
        }
    }

    IEnumerator PerformSuperMove()
    {
        isPerformingSuper = true;
        Debug.Log($"{gameObject.name} SUPER MOVE ACTIVATED!");

        // Play activation sound
        if (superActivationSound != null)
        {
            AudioSource.PlayClipAtPoint(superActivationSound, transform.position);
        }

        // Hit stop effect
        if (ScreenEffects.Instance != null)
        {
            ScreenEffects.Instance.DoHitStop(0.1f);
        }
        yield return new WaitForSecondsRealtime(0.1f);

        // Slow motion
        if (ScreenEffects.Instance != null)
        {
            ScreenEffects.Instance.DoSlowMotion(0.3f, 0.5f);
        }

        // Play super animation
        animator.Play(superAnimationName);

        // Wait for hit frame
        yield return new WaitForSeconds(0.3f);

        // Deal damage to players in range
        // Use players from OpponentAI (no duplicate setup needed!)
        if (opponentAI == null || opponentAI.players == null || opponentAI.fightingController == null)
        {
            Debug.LogWarning("SuperMeterAI: No OpponentAI or players found!");
            isPerformingSuper = false;
            yield break;
        }

        for (int i = 0; i < opponentAI.players.Length; i++)
        {
            Transform player = opponentAI.players[i];

            if (player == null || !player.gameObject.activeInHierarchy)
                continue;

            if (Vector3.Distance(transform.position, player.position) <= superRadius)
            {
                // Play impact sound
                if (superImpactSound != null)
                {
                    AudioSource.PlayClipAtPoint(superImpactSound, player.position);
                }

                // Screen shake
                if (ScreenEffects.Instance != null)
                {
                    ScreenEffects.Instance.ShakeScreen(0.4f, 0.3f);
                }

                // Deal damage
                if (opponentAI.fightingController[i] != null)
                {
                    opponentAI.fightingController[i].StartCoroutine(opponentAI.fightingController[i].PlayHitDamageAnimation(superDamage));
                    Debug.Log($"AI SUPER HIT! Dealt {superDamage} damage to {player.name}!");

                    // Knockback
                    Vector3 knockbackDirection = (player.position - transform.position).normalized;
                    if (player.TryGetComponent<CharacterController>(out CharacterController playerController))
                    {
                        playerController.Move(knockbackDirection * superKnockback);
                    }
                }
            }
        }

        // Empty meter
        currentMeter = 0f;
        isSuperReady = false;
        if (superMeterUI != null)
        {
            superMeterUI.EmptyMeter();
        }

        yield return new WaitForSeconds(0.5f);

        isPerformingSuper = false;
    }

    public bool IsPerformingSuper()
    {
        return isPerformingSuper;
    }
}
