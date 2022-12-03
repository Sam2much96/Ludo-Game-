using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieController : MonoBehaviour
{
    // Torque values
    private float upwardMagForce = 5.0f;
    private float minSpeed = 8.0f;
    private float minAngle = 4.0f;
    private float maxAngle = 5.0f;

    // Some useful components of the die gameobject
    public Rigidbody rb;
    private AudioSource[] sounds;
    private AudioSource dieSound;
    private AudioSource dieSound1;
    public GameObject[] faces;

    // Volume variables
    private float lastHeight;
    private float maxHeight = 1.0f;

    // Roll values
    public bool hasRolled;
    public int rollCount = 0;
    public int minRollCount = 1;
    public int maxRollCount = 3;
    public int defaultRollCount = 0;

    private void Awake()
    {
        sounds = GameObject.Find("Main Camera").GetComponents<AudioSource>();
        dieSound = sounds[0];
        dieSound1 = sounds[1];
    }

    // Start is called before the first frame update
    void Start()
    {
        lastHeight = maxHeight;
    }

    // Update is called once per frame
    void Update()
    {
        if (lastHeight < transform.position.y)
        {
            lastHeight = transform.position.y;
        }

    }

    private void Spin()
    {
        Vector3 upwardForce = Vector3.up * upwardMagForce;
        rb.AddForce(upwardForce, ForceMode.Impulse);
        StartCoroutine(TimeToSpin());
    }

    IEnumerator TimeToSpin()
    {
        // For performing spin on die after some time it leaves the ground
        yield return new WaitForSeconds(0.1f);
        float torqueSpeed = minSpeed + rollCount;
        Vector3 torque = new Vector3(RandomValues(minAngle, maxAngle), RandomValues(minAngle, maxAngle), RandomValues(minAngle, maxAngle));
        rb.AddTorque(torque * torqueSpeed * Time.deltaTime, ForceMode.Impulse);
    }

    private float RandomValues(float min, float max)
    {
        // For returning random values
        return Random.Range(min, max);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ludo Board"))
        {
            // Sound the die makes
            dieSound.volume = lastHeight / maxHeight;
            dieSound.Play();
            lastHeight = 0;
            hasRolled = true;
        }

        if (collision.gameObject.CompareTag("Frame"))
        {
            dieSound1.Play();
            hasRolled = true;
        }
    }

    private void OnMouseDown()
    {
        if (hasRolled || rollCount >= maxRollCount) // test
            return;

        // Activate die spin mechanism
        Spin();
        rollCount++; // test
    }

    public int NumberOnDie()
    {
        // To detect the number on the die
        int max = 0;
        int selected = 1;
        for (int i = 1; i < 6; i++)
        {
            if (faces[max].transform.position.y > faces[i].transform.position.y)
            {
                selected = max + 1;
            }
            else
            {
                max = i;
                selected = max + 1;
            }
        }

        return selected;
    }
}
