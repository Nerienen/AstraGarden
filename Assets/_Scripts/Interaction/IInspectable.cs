using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInspectable
{
   public void Inspect(Vector3 inspectorPivotForward);
   public void StopInspecting();
}
