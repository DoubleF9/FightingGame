using UnityEngine;
using System.Collections;

public class OpponentAI : MonoBehaviour
{
    [Header("Opponent Movement")]
    public float movementSpeed = 1f;
    public float rotationSpeed = 10f;
    public CharacterController characterController;

    [Header("Opponent Fight")]
    public float attackCooldown = 0.5f;
    public int attackDamage = 5;
    public string[] attackAnimations = { "Attack1Animation", "Attack2Animation", "Attack3Animation", "Attack4Animation" };
    public float dodgeDistance = 2f;
    public int attackCount = 0;
    public int randomNumber;
    public float attackRadius = 2.2f;
    public FightingController[] fightingController;
    public Transform[] players;
    public bool isTakingDamage;
    private float lastAttackTime;

    [Header("Effects and Sound")]
    public AudioClip[] hitSounds;

    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
        createRandomNumber();
    }

    void Update()
    {
        if(attackCount==randomNumber)
        {
            attackCount = 0;
            createRandomNumber();
        }

        for (int i = 0; i < fightingController.Length; i++)
        {
            if (players[i].gameObject.activeSelf && Vector3.Distance(transform.position, players[i].position) <= attackRadius)
            {
                //stop walking animation
                if (Time.time - lastAttackTime > attackCooldown)
                {
                    int randomAttack = Random.Range(0, attackAnimations.Length);
                    if(!isTakingDamage)
                    {
                        PerformAttack(randomAttack);
                    }
                    //play attack anim
                    fightingController[i].StartCoroutine(fightingController[i].PlayHitDamageAnimation(attackDamage));
                }

            }
            else
            { 
                if (players[i].gameObject.activeSelf)
                {
                    Vector3 direction = (players[i].position - transform.position).normalized;
                    characterController.Move(direction * movementSpeed * Time.deltaTime);
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                    //play walk anim

                }
            }
        }
    }

    void PerformAttack(int attackIndex)
    {

        // Trigger attack animation
        int damage = attackDamage;
        Debug.Log($"Opponent performed attack {attackIndex} with damage {damage}");
        lastAttackTime = Time.time;

    }

    void PerformDodgeFront()
    {
        //play animation
        Vector3 dodgeDirection = -transform.forward * dodgeDistance;

        characterController.SimpleMove(dodgeDirection);
    }

    void createRandomNumber()
    {
        randomNumber = Random.Range(1, 5);
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
        //play anim

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        //play death animation
        gameObject.SetActive(false);
    }
}
