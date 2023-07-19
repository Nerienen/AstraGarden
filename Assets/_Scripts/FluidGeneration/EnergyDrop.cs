using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrop : Drop
{
    protected override void OnCollisionEnter(Collision collision)
    {
       base.OnCollisionEnter(collision);

       EnergyDrop energyDrop = collision.gameObject.GetComponent<EnergyDrop>();
       if (energyDrop != null)
       {
           if (transform.localScale.x > collision.transform.localScale.x)
           {
               AddScale(collision.transform.localScale);
               energyDrop.gameObject.SetActive(false);
           }
           else
           {
               energyDrop.AddScale(transform.localScale);
               gameObject.SetActive(false);
           }
           return;
       }
       
       ReduceScale();
    }
    
    protected override void OnCollisionStay(Collision collision)
    {
        base.OnCollisionStay(collision);

        EnergyDrop energyDrop = collision.gameObject.GetComponent<EnergyDrop>();
        if (energyDrop != null)
        {
            if (transform.localScale.x > collision.transform.localScale.x)
            {
                AddScale(collision.transform.localScale);
                energyDrop.gameObject.SetActive(false);
            }
            else
            {
                energyDrop.AddScale(transform.localScale);
                gameObject.SetActive(false);
            }
            return;
        }
        
        ReduceScale();
    }
}
