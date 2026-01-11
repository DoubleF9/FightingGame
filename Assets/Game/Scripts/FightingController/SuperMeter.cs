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

        // 1. FIND TARGET & LOCK
        Transform target = null;
        float closestDist = superRadius;

        // Find closest enemy
        if (fightingController != null && fightingController.opponents != null)
        {
            foreach (Transform opponent in fightingController.opponents)
            {
                if (opponent == null || !opponent.gameObject.activeInHierarchy) continue;
                float d = Vector3.Distance(transform.position, opponent.position);
                if (d < closestDist) { closestDist = d; target = opponent; }
            }
        }

        OpponentAI targetAI = null;
        CharacterController targetCC = null;

        // 2. DISABLE ENEMY & START ANIMATION
        if (target != null)
        {
            target.TryGetComponent<OpponentAI>(out targetAI);
            target.TryGetComponent<CharacterController>(out targetCC);

            if (targetAI != null)
            {
                targetAI.enabled = false;
                targetAI.StopAllCoroutines();
                if (targetAI.animator != null) targetAI.animator.Play("HitDamageAnimation");
            }
        }

        if (superActivationSound != null) AudioSource.PlayClipAtPoint(superActivationSound, transform.position);
        animator.Play(superAnimationName);

        // 3. SMOOTH ROTATION & DASH
        if (target != null)
        {
            float windUpTime = 0.2f;
            float timer = 0f;

            // Rotation Setup
            Quaternion startRot = transform.rotation;
            Vector3 dirToEnemy = (target.position - transform.position).normalized;
            dirToEnemy.y = 0;
            Quaternion endRot = Quaternion.LookRotation(dirToEnemy);

            // Dash Setup
            float desiredDistance = 0.5f;
            Vector3 startPos = transform.position;

            Vector3 endPos = target.position - (dirToEnemy * desiredDistance);
            endPos.y = transform.position.y;

            while (timer < windUpTime)
            {
                // A. Smooth Rotate
                transform.rotation = Quaternion.Slerp(startRot, endRot, timer / windUpTime);

                // B. Smooth Dash (Slide towards enemy)
                Vector3 nextPos = Vector3.Lerp(startPos, endPos, timer / windUpTime);
                Vector3 moveDelta = nextPos - transform.position;

                if (characterController != null)
                {
                    characterController.Move(moveDelta);
                }
                else
                {
                    transform.position = nextPos;
                }

                timer += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSeconds(0.2f);
        }

        // --- HIT 1 ---
        if (target != null)
        {
            ApplyDamage(targetAI, 10);
            if (superImpactSound != null) AudioSource.PlayClipAtPoint(superImpactSound, target.position);
            if (ScreenEffects.Instance != null) { ScreenEffects.Instance.DoHitStop(0.05f); ScreenEffects.Instance.ShakeScreen(0.1f, 0.1f); }
        }
        yield return new WaitForSeconds(0.2f);

        // --- HIT 2 ---
        if (target != null)
        {
            ApplyDamage(targetAI, 10);
            if (superImpactSound != null) AudioSource.PlayClipAtPoint(superImpactSound, target.position);
            if (ScreenEffects.Instance != null) { ScreenEffects.Instance.DoHitStop(0.05f); ScreenEffects.Instance.ShakeScreen(0.1f, 0.1f); }
        }
        yield return new WaitForSeconds(0.2f);

        // --- HIT 3 ---
        if (target != null)
        {
            ApplyDamage(targetAI, 10);
            if (superImpactSound != null) AudioSource.PlayClipAtPoint(superImpactSound, target.position);
            if (ScreenEffects.Instance != null) { ScreenEffects.Instance.DoHitStop(0.05f); ScreenEffects.Instance.ShakeScreen(0.1f, 0.1f); }
        }
        yield return new WaitForSeconds(0.4f);

        // --- FINISHER ---
        if (target != null)
        {
            ApplyDamage(targetAI, superDamage);
            if (superImpactSound != null) AudioSource.PlayClipAtPoint(superImpactSound, target.position);

            if (ScreenEffects.Instance != null) ScreenEffects.Instance.ShakeScreen(0.3f, 0.2f);

            Vector3 knockbackDir = (target.position - transform.position).normalized;
            if (targetCC != null) StartCoroutine(PushEnemy(targetCC, knockbackDir * superKnockback, 0.2f));
        }

        // 4. CLEANUP
        yield return new WaitForSeconds(2f);
        if (targetAI != null) targetAI.enabled = true;

        currentMeter = 0f;
        isSuperReady = false;
        if (superMeterUI != null) superMeterUI.EmptyMeter();
        isPerformingSuper = false;
    }

    // Helper function to handle damage simply
    void ApplyDamage(OpponentAI ai, int dmg)
    {
        if (ai != null)
        {
            // We use StartCoroutine on the AI to trigger its hit logic
            ai.StartCoroutine(ai.PlayHitDamageAnimation(dmg));
        }
    }

    // Helper to push the enemy smoothly
    IEnumerator PushEnemy(CharacterController cc, Vector3 velocity, float time)
    {
        float timer = 0;
        while (timer < time)
        {
            if (cc != null) cc.Move(velocity * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
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
