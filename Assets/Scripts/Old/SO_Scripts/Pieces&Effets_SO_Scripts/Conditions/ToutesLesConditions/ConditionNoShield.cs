using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NoShieldCondition", menuName = "Conditions/NoShield")]
public class ConditionNoSHield : SoCondition
{
    public override bool Condition(ConditionOutput conditionOutput) //  ajouter context 
    {
        if(conditionOutput.port.thisBoardPiece.shield == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
