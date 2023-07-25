using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackOutLights : MonoBehaviour
{
    private void Start()
    {
        FuseBox.Instance.OnPowerDown += () =>
        {
            foreach (Transform light in transform)
            {
                light.gameObject.SetActive(true);
            }
        };
        
        FuseBox.Instance.OnPowerUp += () =>
        {
            foreach (Transform light in transform)
            {
                light.gameObject.SetActive(false);
            }
        };
    }
}
