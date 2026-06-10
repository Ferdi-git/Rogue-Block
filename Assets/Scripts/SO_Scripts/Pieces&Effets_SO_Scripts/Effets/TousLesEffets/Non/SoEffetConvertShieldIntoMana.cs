using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConvertShieldIntoManaEffet", menuName = "Effet/ConvertShieldIntoMana")]

public class SoEffetConvertShieldIntoMana : SoEffet
{
    public override IEnumerator Effet(Context context, OutputPort port, List<int> amount, int tour)
    {
        port.piecePlayed.PiecePlayedUp();
        port.PlayerGainMana(port.thisBoardPiece.shield);
        port.thisBoardPiece.shield = 0;
        port.thisBoardPiece.pieceAnimation.RefreshHealth(port.thisBoardPiece);
        yield return port.thisBoardPiece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.atk, null);
        context.NbrDeRepetition += 1;
    }

    public override IEnumerator RepeatEffet(Context context, OutputPort port, List<int> amount, int tour, BoardPiece declencheur)
    {
        port.piecePlayed.RepeatedPieceUp();
        port.PlayerGainMana(port.thisBoardPiece.shield);
        port.thisBoardPiece.shield = 0;
        port.thisBoardPiece.pieceAnimation.RefreshHealth(port.thisBoardPiece);
        yield return port.thisBoardPiece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPieceRepeated(), PieceAnimations.TypeAnim.atk, declencheur);
        context.NbrDeRepetition += 1;
    }
}
