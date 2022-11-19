using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieController : MonoBehaviour
{
    private float minRange = 18.0f;
    private float maxRange = 22.0f;
    private float minTorqueAngle = 0;
    private float maxTorqueAngle = 45.0f;
    private bool canSpin = true;
    public Rigidbody rb;
    public AudioSource dieSound;
    public GameObject[] faces;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(NumberOnDie());
        if (canSpin)
        {
            Spin();
        }
    }

    private void Spin()
    {
        float torqueSpeed = Random.Range(minRange, maxRange);
        Vector3 torque = new Vector3(Random.Range(minTorqueAngle, maxTorqueAngle), Random.Range(minTorqueAngle, maxTorqueAngle), Random.Range(minTorqueAngle, maxTorqueAngle));
        rb.AddTorque(torque * torqueSpeed * Time.deltaTime, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ludo Board"))
            dieSound.Play();

        canSpin = false;
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
