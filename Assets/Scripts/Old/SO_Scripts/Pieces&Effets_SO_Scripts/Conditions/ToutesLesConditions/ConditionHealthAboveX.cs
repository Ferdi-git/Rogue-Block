using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthAboveXCondition", menuName = "Conditions/HealthAboveX")]

public class ConditionHealthAboveX : SoCondition
{
    public override bool Condition(ConditionOutput conditionOutput)
    {
        if(conditionOutput.port.thisBoardPiece.healthPoint >= conditionOutput.variableList[0])
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
