using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealBetterWhenRetriggeredEffet", menuName = "Effet/HealBetterWhenRetriggered")]

public class SoEffetHealBetterWhenRetriggered : SoEffet
{
    public override IEnumerator Effet(Context context, OutputPort port, List<int> amount, int tour)
    {
        port.piecePlayed.PiecePlayedUp();
        BoardPiece piece = port.thisBoardPiece;
        //yield return piece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.classic);

        for (int i = 0; i < context.voisins.Count; i++)
        {
            BoardPiece voisin = context.voisins[i];
            port.thisBoardPiece = voisin;
            
            yield return piece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.classic, null);
            yield return port.thisBoardPiece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.heal, piece);
            voisin.healthPoint += amount[0];
        }
        port.thisBoardPiece = piece;
        context.NbrDeRepetition += 1;
    }
    public override IEnumerator RepeatEffet(Context context, OutputPort port, List<int> amount, int tour, BoardPiece declencheur)
    {
        port.piecePlayed.RepeatedPieceUp();
        BoardPiece piece = port.thisBoardPiece;
        yield return piece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.classic, declencheur);
        for (int i = 0; i < context.voisins.Count; i++)
        {
            BoardPiece voisin = context.voisins[i];
            port.thisBoardPiece = voisin;
            
            if (i != 0) yield return piece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.classic, null);
            yield return port.thisBoardPiece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.heal, piece);
            voisin.healthPoint += amount[0] + context.NbrDeRepetition * amount[1];
        }
        port.thisBoardPiece = piece;
        context.NbrDeRepetition += 1;
    }
}
