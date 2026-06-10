using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageScaleIfNoShieldEffet", menuName = "Effet/DamageScaleIfNoShield")]
public class SoEffetDamageScaleIfNoShield : SoEffet
{
    public override IEnumerator Effet(Context context, OutputPort port, List<int> amount, int tour)
    {
        if (port.thisBoardPiece.shield < 1)
        {
            port.thisBoardPiece.pieceInfo.soPiece.TempEffectValues[1] += amount[0];
        }

        port.piecePlayed.PiecePlayedUp();
        port.DoDamageToEnnemi(port.thisBoardPiece.pieceInfo.soPiece.TempEffectValues[1]);
        yield return port.thisBoardPiece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.atk, null);
        context.NbrDeRepetition += 1;
    }
    public override IEnumerator RepeatEffet(Context context, OutputPort port, List<int> amount, int tour, BoardPiece declencheur)
    {
        if (port.thisBoardPiece.shield < 1)
        {
            port.thisBoardPiece.pieceInfo.soPiece.TempEffectValues[1] += amount[0];
        }

        port.piecePlayed.RepeatedPieceUp();
        port.DoDamageToEnnemi(port.thisBoardPiece.pieceInfo.soPiece.TempEffectValues[1]);
        yield return port.thisBoardPiece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPieceRepeated(), PieceAnimations.TypeAnim.atk, declencheur);
        context.NbrDeRepetition += 1;
    }
}
