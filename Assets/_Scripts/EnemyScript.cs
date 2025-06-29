using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemyScript : MonoBehaviour
{
    public List<Transform> targets = new List<Transform>();
    public float speed = 0.1f;
    public float stopDistance = 0.1f;
    public AudioSource audioSource;
    public AudioClip[] clips;
    int i = 1;
    public DamWallScript wallTarget;
    public ParticleSystem deathParticle;
    private Rigidbody rb;
    bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (targets == null) return;
        Vector3 direction = targets[i].position - transform.position;
        float distance = direction.magnitude;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = lookRotation * Quaternion.Euler(0, 90f, 0);

        if (distance > stopDistance)
        {
            Vector3 move = direction.normalized * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + move);
        }
        else if (i + 1 < targets.Count)
        {
            i++;
        }
        else
        {
            if (!isAttacking)
            {
                isAttacking = true;
                rb.linearVelocity = Vector3.zero;
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(2);
        audioSource.clip = clips[0];
        audioSource.Play();
        wallTarget.LooseHealth(12);
        isAttacking = false;
    }

    public void Die()
    {
        deathParticle.Play();
        deathParticle.gameObject.transform.parent = null;
        Destroy(gameObject, 0.2f);
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Trap")
        {
            Destroy(other.gameObject, 2);
        }
    }
}
