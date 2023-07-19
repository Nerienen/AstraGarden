using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldPoint : MonoBehaviour
{
    [SerializeField] private Transform holdTransform;
    
    public void HoldObject(Transform objectTransform)
    {
        objectTransform.position = holdTransform.position;
        objectTransform.rotation = holdTransform.rotation;

        objectTransform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
}
