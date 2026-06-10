using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Audio.ProcessorInstance;

[CreateAssetMenu(fileName = "ShieldAtTheCostOfHpEffet", menuName = "Effet/ShieldAtTheCostOfHp")]
public class SoEffetShieldAtTheCostOfHp : SoEffet
{
    public override IEnumerator Effet(Context context, OutputPort port, List<int> amount, int tour)
    {
        int remainingHealth = 0;

        bool wasAtLimit = false;

        bool wasAtOneHp = false;

        port.piecePlayed.PiecePlayedUp();
        BoardPiece piece = port.thisBoardPiece;
        //yield return piece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.classic,null);

        if (context.voisins.Count > 0)
        {
            if(piece.healthPoint > amount[0])
            {
                piece.healthPoint -= amount[0];
                wasAtLimit = false;
            }
            else
            {
                remainingHealth = piece.healthPoint - 1;
                wasAtLimit = true;
                if (piece.healthPoint == 1) { wasAtOneHp = true; }
                else { piece.healthPoint = 1; }
            }
            piece.pieceAnimation.RefreshHealth(piece);
        }

        if (!wasAtOneHp)
        {
            for (int i = 0; i < context.voisins.Count; i++)
            {
                if (!wasAtLimit)
                {
                    BoardPiece voisin = context.voisins[i];
                    port.thisBoardPiece = voisin;
                    voisin.shield += amount[0] * 2;
                    port.thisBoardPiece.pieceAnimation.RefreshHealth(port.thisBoardPiece);
                    yield return piece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.classic, null);
                    yield return port.thisBoardPiece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.shield, piece);
                }
                else
                {
                    BoardPiece voisin = context.voisins[i];
                    port.thisBoardPiece = voisin;
                    voisin.shield += remainingHealth * 2;
                    piece.pieceAnimation.RefreshHealth(piece);
                    port.thisBoardPiece.pieceAnimation.RefreshHealth(port.thisBoardPiece);
                    yield return piece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.classic, null);
                    yield return port.thisBoardPiece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.shield, piece);
                }
            }
        }
        port.thisBoardPiece = piece;
        context.NbrDeRepetition += 1;
    }
    public override IEnumerator RepeatEffet(Context context, OutputPort port, List<int> amount, int tour, BoardPiece declencheur)
    {
        int remainingHealth = 0;

        bool wasAtLimit = false;

        bool wasAtOneHp = false;

        port.piecePlayed.RepeatedPieceUp();
        BoardPiece piece = port.thisBoardPiece;

        yield return piece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.classic, declencheur);

        if (context.voisins.Count > 0)
        {
            if (piece.healthPoint > amount[0])
            {
                piece.healthPoint -= amount[0];
                wasAtLimit = false;
            }
            else
            {
                remainingHealth = piece.healthPoint - 1;
                wasAtLimit = true;
                if (piece.healthPoint == 1) { wasAtOneHp = true; }
                else { piece.healthPoint = 1; }
            }
            piece.pieceAnimation.RefreshHealth(piece);
        }

        if (!wasAtOneHp)
        {
            for (int i = 0; i < context.voisins.Count; i++)
            {
                if (!wasAtLimit)
                {
                    BoardPiece voisin = context.voisins[i];
                    port.thisBoardPiece = voisin;
                    voisin.shield += amount[0] * 2;
                    port.thisBoardPiece.pieceAnimation.RefreshHealth(port.thisBoardPiece);
                    if (i != 0) yield return piece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.classic, null);
                    yield return port.thisBoardPiece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.shield, piece);
                }
                else
                {
                    BoardPiece voisin = context.voisins[i];
                    port.thisBoardPiece = voisin;
                    voisin.shield += amount[0] * 2;
                    voisin.shield += remainingHealth * 2;
                    port.thisBoardPiece.pieceAnimation.RefreshHealth(port.thisBoardPiece);
                    port.thisBoardPiece.pieceAnimation.RefreshHealth(port.thisBoardPiece);
                    if (i != 0) yield return piece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.classic, null);
                    yield return port.thisBoardPiece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.shield, piece);
                }
            }
        }
        port.thisBoardPiece = piece;
        context.NbrDeRepetition += 1;
    }
}

