using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialForce : MonoBehaviour
{
    
    void Start()
    {
        if (TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.AddForce(new Vector3(Random.value, Random.value, Random.value).normalized*Random.value, ForceMode.Impulse);
        }
    }

    
}
