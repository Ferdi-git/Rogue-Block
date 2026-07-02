using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageScaleWithEmptyEffet", menuName = "Effet/DamageScaleWithEmptyEmpty")]

public class SoEffetDamageScaleWithEmpty : SoEffet
{
    public override IEnumerator Effet(Context context, OutputPort port, List<int> amount, int tour)
    {
        port.piecePlayed.PiecePlayedUp();
        port.thisBoardPiece.soPieces.TempEffectValues[1] -= 3;
        port.thisBoardPiece.soPieces.TempEffectValues[1] += context.NbrCaseLibre * amount[0];
        port.DoDamageToEnnemi(port.thisBoardPiece.soPieces.TempEffectValues[1]);
        yield return port.thisBoardPiece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.atk, null);
        context.NbrDeRepetition += 1;
    }
    public override IEnumerator RepeatEffet(Context context, OutputPort port, List<int> amount, int tour, BoardPiece declencheur)
    {
        port.piecePlayed.RepeatedPieceUp();
        port.thisBoardPiece.soPieces.TempEffectValues[1] -= 3;
        port.thisBoardPiece.soPieces.TempEffectValues[1] += context.NbrCaseLibre * amount[0];
        port.DoDamageToEnnemi(port.thisBoardPiece.soPieces.TempEffectValues[1]);
        yield return port.thisBoardPiece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPieceRepeated(), PieceAnimations.TypeAnim.atk, declencheur);
        context.NbrDeRepetition += 1;
    }
}
