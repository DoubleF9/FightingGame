using System.Collections;
using UnityEngine;

public class FightingController : MonoBehaviour
{
    [Header("Player Movement")]
    public float movementSpeed = 1f;
    public float rotationSpeed = 10f;
    private CharacterController characterController;
    private Animator animator; // Add reference to Animator

    [Header("Player Fight")]
    public float attackCooldown = 0.5f;
    public int attackDamage = 5;
    public string[] attackAnimations = { "Attack1Animation", "Attack2Animation", "Attack3Animation", "Attack4Animation" };
    public float dodgeDistance = 2f;
    public float attackRadius = 2.2f;
    public Transform[] opponents;
    private float lastAttackTime;

    [Header("Effects and Sound")]
    public AudioClip[] hitSounds;

    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

    [Header("Super Meter")]
    public SuperMeter superMeter; // Reference to super meter component

    void Awake()
    {

        if (healthBar == null)
        {
            return;
        }

        currentHealth = maxHealth;
        healthBar.GiveFullHealth(currentHealth);
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Block all actions while performing super move
        if (superMeter != null && superMeter.IsPerformingSuper())
        {
            return;
        }

        PerformMovement();
        PerformDodgeFront();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PerformAttack(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PerformAttack(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PerformAttack(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PerformAttack(3);
        }
    }

    void PerformMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput);

        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (horizontalInput > 0)
            {
                animator.SetBool("Walking", true);
            }
            else if (horizontalInput < 0)
            {
                animator.SetBool("Walking", true);
            }
            else if (verticalInput != 0)
            {
                animator.SetBool("Walking", true);
            }
        }
        else
        {
            animator.SetBool("Walking", false);
        }

        characterController.Move(movement * movementSpeed * Time.deltaTime);

    }

    void PerformAttack(int attackIndex)
    {
        if (Time.time - lastAttackTime > attackCooldown)
        {
            animator.Play(attackAnimations[attackIndex]);
            int damage = attackDamage;
            Debug.Log($"Performing attack {attackIndex} with damage {damage}");
            lastAttackTime = Time.time;

            foreach (Transform opponent in opponents)
            {
                // Skip this opponent if they are missing or inactive
                if (opponent == null || !opponent.gameObject.activeInHierarchy)
                    continue;

                if (Vector3.Distance(transform.position, opponent.position) <= attackRadius)
                {
                    if (opponent.TryGetComponent<OpponentAI>(out OpponentAI opponentAI))
                    {
                        opponentAI.StartCoroutine(opponentAI.PlayHitDamageAnimation(damage));
                        Debug.Log($"Dealt {damage} damage to {opponent.name}");

                        // Charge super meter when hitting opponent
                        if (superMeter != null)
                        {
                            superMeter.OnHitOpponent();
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"No OpponentAI found on {opponent.name}");
                    }
                }

            }

        }
        else
        {
            Debug.Log("Attack on cooldown");
        }

    }

    void PerformDodgeFront()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.Play("DodgeFrontAnimation");
            Vector3 dodgeDirection = transform.forward * dodgeDistance;

            characterController.Move(dodgeDirection);

            // Charge super meter on dodge
            if (superMeter != null)
            {
                superMeter.OnDodge();
            }
        }
    }

    public IEnumerator PlayHitDamageAnimation(int takeDamage)
    {
        yield return new WaitForSeconds(0.5f);

        if (hitSounds != null && hitSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, hitSounds.Length);
            AudioSource.PlayClipAtPoint(hitSounds[randomIndex], transform.position); // Play sound at player's position
        }

        //decrease health
        currentHealth -= takeDamage;
        healthBar.SetHealth(currentHealth);
        //play anim
        animator.Play("HitDamageAnimation");

        // Charge super meter when taking damage (rage mechanic)
        if (superMeter != null)
        {
            superMeter.OnTakeDamage();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        //play death animation
        //gameObject.SetActive(false);
    }
}
