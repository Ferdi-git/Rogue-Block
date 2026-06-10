using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageToEnnemyAndItself", menuName = "Effet/DamageToEnnemyAndItself")]

public class SoEffetDamageToEnnemyAndItself : SoEffet
{
    public override IEnumerator Effet(Context context, OutputPort port, List<int> amount, int tour)
    {
        port.piecePlayed.PiecePlayedUp();
        port.DoDamageToEnnemi(amount[0]);
        port.ThisPieceTakeDamage(amount[1]);
        port.thisBoardPiece.pieceAnimation.RefreshHealth(port.thisBoardPiece);
        yield return port.thisBoardPiece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.atk, null);

        if (port.thisBoardPiece.healthPoint <= 0)
        {
            context.healthManager.GiveStats(port.thisBoardPiece.healthPoint, port.thisBoardPiece.shield, port.thisBoardPiece);
            context.healthManager.Dead(port.thisBoardPiece.pieceInfo.soPiece.BaseEffectValues[0]);
        }

        context.NbrDeRepetition += 1;
    }
    public override IEnumerator RepeatEffet(Context context, OutputPort port, List<int> amount, int tour, BoardPiece declencheur)
    {
        port.piecePlayed.RepeatedPieceUp();
        port.DoDamageToEnnemi(amount[0]);
        port.ThisPieceTakeDamage(amount[1]);
        port.thisBoardPiece.pieceAnimation.RefreshHealth(port.thisBoardPiece);
        yield return port.thisBoardPiece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPieceRepeated(), PieceAnimations.TypeAnim.atk, declencheur);

        if (port.thisBoardPiece.healthPoint <= 0)
        {
            context.healthManager.GiveStats(port.thisBoardPiece.healthPoint, port.thisBoardPiece.shield, port.thisBoardPiece);
            context.healthManager.Dead(port.thisBoardPiece.pieceInfo.soPiece.BaseEffectValues[0]);
        }

        context.NbrDeRepetition += 1;
    }
}
