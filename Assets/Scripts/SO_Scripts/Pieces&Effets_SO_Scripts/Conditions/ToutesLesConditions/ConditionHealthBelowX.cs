using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeathBelowXCondition", menuName = "Conditions/HealthBelowX")]

public class ConditionHealthBelowX : SoCondition
{
    public override bool Condition(ConditionOutput conditionOutput)
    {
        if(conditionOutput.port.thisBoardPiece.healthPoint <= conditionOutput.variableList[0])
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
