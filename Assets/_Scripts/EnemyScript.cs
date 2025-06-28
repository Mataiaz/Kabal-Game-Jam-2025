using UnityEngine;
using System.Collections.Generic;

public class EnemyScript : MonoBehaviour
{
    public List<Transform> targets = new List<Transform>();
    public float speed = 0.1f;
    public float stopDistance = 0.1f;
    int i = 1;

    private Rigidbody rb;

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
            rb.linearVelocity = Vector3.zero;
        }
    }
}
