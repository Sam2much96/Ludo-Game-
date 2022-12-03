using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSpawner : MonoBehaviour
{
    public DieController die;
    private Vector3 defaultPosition;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BoxCollider>().isTrigger = true;
        defaultPosition = die.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = defaultPosition;
    }
}
