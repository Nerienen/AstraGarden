using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMachine : MonoBehaviour
{
    private HoldPoint _holdPoint;
    [SerializeField] private GameObject waterButton;
    [SerializeField] private GameObject oxygenButton;
    [SerializeField] private GameObject energyButton;
    
    private void Start()
    {
        _holdPoint = GetComponent<HoldPoint>();
    }

    public void UseButton(GameObject button)
    {
        Animator animator = button.GetComponent<Animator>();
        animator.ResetTrigger("Press");
        animator.SetTrigger("Press");

        if(!_holdPoint.IsHoldingObject) return;
        Plant plant = _holdPoint.CurrentHoldenObject.GetComponent<Plant>();

        if (button == waterButton)
        {
        }
        else if (button == energyButton)
        {
            
        }
        else if (button == oxygenButton)
        {
            
        }
    }
}
