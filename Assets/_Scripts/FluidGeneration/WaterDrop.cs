using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrop : Drop
{
    protected override void OnCollisionEnter(Collision collision)
    {
       base.OnCollisionEnter(collision);

       WaterDrop waterDrop = collision.gameObject.GetComponent<WaterDrop>();
       if (waterDrop != null)
       {
           if (transform.localScale.x > collision.transform.localScale.x)
           {
               AddScale(collision.transform.localScale);
               waterDrop.gameObject.SetActive(false);
           }
           else
           {
               waterDrop.AddScale(transform.localScale);
               gameObject.SetActive(false);
           }
           return;
       }
       
       Plant plant = collision.gameObject.GetComponent<Plant>();
       if (plant != null)
       {
           foreach (var col in Physics.OverlapSphere(transform.position, transform.localScale.x + transform.localScale.x/maxDropScale))
           {
               col.GetComponent<Plant>()?.Water(transform.localScale.x);
           }

           gameObject.SetActive(false);
       }
       
       ReduceScale();
    }
    
    protected override void OnCollisionStay(Collision collision)
    {
        base.OnCollisionStay(collision);

        WaterDrop waterDrop = collision.gameObject.GetComponent<WaterDrop>();
        if (waterDrop != null)
        {
            if (transform.localScale.x > collision.transform.localScale.x)
            {
                AddScale(collision.transform.localScale);
                waterDrop.gameObject.SetActive(false);
            }
            else
            {
                waterDrop.AddScale(transform.localScale);
                gameObject.SetActive(false);
            }
            return;
        }
        
        Plant plant = collision.gameObject.GetComponent<Plant>();
        if (plant != null)
        {
            foreach (var col in Physics.OverlapSphere(transform.position, transform.localScale.x + transform.localScale.x/maxDropScale))
            {
                col.GetComponent<Plant>()?.Water(transform.localScale.x);
            }

            gameObject.SetActive(false);
        }
        
        ReduceScale();
    }
}
