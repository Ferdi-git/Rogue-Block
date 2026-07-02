using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HasLessThanXPiecesNextToItCondition", menuName = "Conditions/LessThanHasXPiecesNextToIt")]
public class ConditionHasLessThanXPiecesNext : SoCondition
{
    public override bool Condition(ConditionOutput conditionOutput) //  ajouter context 
    {
        if (conditionOutput.context.voisins.Count <= conditionOutput.variableList[0])
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
