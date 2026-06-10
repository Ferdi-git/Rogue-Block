using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HasEnoughManaCondition", menuName = "Conditions/HasEnoughMana")]

public class ConditionHasEnoughMana : SoCondition
{
    public override bool Condition(ConditionOutput conditionOutput) //  ajouter context 
    {
        if (conditionOutput.port.statsPlayer.mana >= conditionOutput.variableList[0])
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
