using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour
{
    public GameObject characterCamera;
    public AudioSource audioSource;
    public AudioClip[] clips;
    public float rayDistance = 100f;
    public Transform firePoint;
    private Rigidbody rb;
    private Vector3 movementInput;
    private float cameraPitch = 0f;
    public float mouseSensitivity = 2f;
    public GameObject[] tools;
    public int lastToolUsed = -1;
    public GameObject placement;
    public GameObject trap;
    public bool canFix = false;
    private int trapCount = 5;

    bool isRestoringTraps = false;
    public ParticleSystem gunFire;

    public List<GameObject> leaks = new List<GameObject>();

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        HandleToolSelection();
        HandleToolAction();
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -80f, 80f);
        characterCamera.transform.localEulerAngles = new Vector3(cameraPitch, 0f, 0f);

        Vector3 camForward = characterCamera.transform.forward;
        Vector3 camRight = characterCamera.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        movementInput = (camForward * moveZ + camRight * moveX).normalized;
        rb.MovePosition(transform.position + movementInput * Time.deltaTime * 5f);

    }


    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        if (rb != null)
            rb.MovePosition(transform.position + movementInput * Time.deltaTime * 5f);
    }

    void HandleToolSelection()
    {
        if (lastToolUsed > -1
        && (Input.GetKeyDown(KeyCode.Alpha1)
        || Input.GetKeyDown(KeyCode.Alpha2)
        || Input.GetKeyDown(KeyCode.Alpha3)
        || Input.GetKeyDown(KeyCode.Alpha4)))
        {
            canFix = true;
            tools[lastToolUsed].gameObject.SetActive(false);
            placement.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            tools[0].gameObject.SetActive(true);
            lastToolUsed = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            tools[1].gameObject.SetActive(true);
            placement.SetActive(true);
            lastToolUsed = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            tools[2].gameObject.SetActive(true);
            lastToolUsed = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            tools[3].gameObject.SetActive(true);
            canFix = false;
            lastToolUsed = 3;
        }
    }

    void HandleToolAction()
    {
        if (placement.activeSelf && Input.GetKeyDown(KeyCode.E) && trapCount > 0)
        {
            Instantiate(trap, placement.transform.position, placement.transform.rotation);
            audioSource.clip = clips[1];
            audioSource.Play();
            trapCount--;
            if (trapCount <= 0 && !isRestoringTraps)
            {
                isRestoringTraps = true;
                StartCoroutine(RestoreTraps());
            }
        }

        if (Input.GetButtonDown("Fire1") && lastToolUsed == 0)
        {
            if (audioSource.clip == clips[0] && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            if (audioSource.clip != clips[0])
            {
                audioSource.clip = clips[0];
            }
            audioSource.time = 0.05f;
            audioSource.Play();
            Shoot();

        }

        if (canFix && leaks.Count > 0 && Input.GetKeyDown(KeyCode.E))
        {
            audioSource.clip = clips[2];
            audioSource.Play();
            foreach (GameObject l in leaks)
            {
                if (Vector3.Distance(l.transform.position, gameObject.transform.position) < 0.7f)
                {
                    Destroy(l);
                }
            }
        }
    }

    void Shoot()
    {
        Ray ray = new Ray(firePoint.position, firePoint.forward);
        RaycastHit hit;

        Debug.DrawRay(firePoint.position, firePoint.forward * rayDistance, Color.red, 1f);
        gunFire.Play();
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.gameObject.GetComponent<EnemyScript>().Die();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "leak" && !leaks.Contains(other.gameObject))
        {
            leaks.Add(other.gameObject);
        }
    }

    IEnumerator RestoreTraps()
    {
        yield return new WaitForSeconds(10);
        trapCount = 5;
        isRestoringTraps = false;
  }

}

/*

build the game so it can be played on the web
implement sounds



*/