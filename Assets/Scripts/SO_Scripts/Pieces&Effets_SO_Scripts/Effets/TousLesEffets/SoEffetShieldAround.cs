using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldAround", menuName = "Effet/ShieldAround")]
public class SoEffetShieldAround : SoEffet
{
    public override IEnumerator Effet(Context context,OutputPort port, List<int> amount, int tour)
    {
        port.piecePlayed.PiecePlayedUp();
        BoardPiece piece = port.thisBoardPiece;
        //yield return piece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.classic,null);

        for (int i = 0; i < context.voisins.Count; i++)
        {
            BoardPiece voisin = context.voisins[i];
            port.thisBoardPiece = voisin;
            voisin.shield += amount[0];
            yield return piece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.classic, null);
            yield return port.thisBoardPiece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.shield, piece);
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
            voisin.shield += amount[0];
            if(i != 0) yield return piece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.classic, null);
            yield return port.thisBoardPiece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.shield , piece);
        }
        port.thisBoardPiece = piece;
        context.NbrDeRepetition += 1;

    }

}
