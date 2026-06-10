using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "Effet/Heal")]
public class SoEffetHeal : SoEffet
{
    public override IEnumerator Effet(Context context,OutputPort port, List<int> amount, int tour)
    {
        port.piecePlayed.PiecePlayedUp();
        yield return port.thisBoardPiece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.classic, null);
        port.PlayerHeal(amount[0]);
        context.NbrDeRepetition += 1;
    }

    public override IEnumerator RepeatEffet(Context context, OutputPort port, List<int> amount, int tour, BoardPiece declencheur)
    {
        port.piecePlayed.RepeatedPieceUp();
        yield return port.thisBoardPiece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPieceRepeated(), PieceAnimations.TypeAnim.repeat, declencheur);
        port.PlayerHeal(amount[0]);
        context.NbrDeRepetition += 1;
    }
}
