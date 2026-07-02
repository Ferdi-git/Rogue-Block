using UnityEngine;

[CreateAssetMenu(fileName = "OnAttackedZoneCondition", menuName = "Conditions/OnAttackedZone")]
public class ConditionOnAttackedZone : SoCondition
{
    public override bool Condition(ConditionOutput conditionOutput) //  ajouter context 
    {
        if(conditionOutput.port.thisBoardPiece.context.NbrCaseAtk > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
