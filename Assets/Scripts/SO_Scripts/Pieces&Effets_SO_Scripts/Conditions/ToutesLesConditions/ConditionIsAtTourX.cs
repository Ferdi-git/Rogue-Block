using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IsAtTourXCondition", menuName = "Conditions/IsAtTourX")]
public class ConditionIsAtTourX : SoCondition
{
    public override bool Condition(ConditionOutput conditionOutput)
    {
        if(conditionOutput.context.Tour >= conditionOutput.variableList[0])
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
