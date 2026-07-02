using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HasXPiecesNextToItCondition", menuName = "Conditions/HasXPiecesNextToIt")]
public class ConditionHasXPiecesNextToIt : SoCondition
{
    public override bool Condition(ConditionOutput conditionOutput) //  ajouter context 
    {
        if(conditionOutput.context.voisins.Count >= conditionOutput.variableList[0])
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

