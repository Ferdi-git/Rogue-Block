using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NoneCondition", menuName = "Conditions/None")]
public class ConditionNone : SoCondition
{
    public override bool Condition(ConditionOutput conditionOutput) //  ajouter context 
    {
        return true;
    }
}
