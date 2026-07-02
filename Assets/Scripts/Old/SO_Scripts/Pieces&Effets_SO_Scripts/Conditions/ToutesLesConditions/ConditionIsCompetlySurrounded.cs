using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IsCompletlySurroundedCondition", menuName = "Conditions/IsCompletlySurrounded")]

public class ConditionIsCompetlySurrounded : SoCondition
{
    public override bool Condition(ConditionOutput conditionOutput)
    {
        if(conditionOutput.context.NbrCaseLibre <= conditionOutput.variableList[0])
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
