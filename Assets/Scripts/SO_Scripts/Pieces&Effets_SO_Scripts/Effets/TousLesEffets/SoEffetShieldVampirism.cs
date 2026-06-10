using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldVampirismEffet", menuName = "Effet/ShieldVampirism")]
public class SoEffetShieldVampirism : SoEffet
{
    public override IEnumerator Effet(Context context, OutputPort port, List<int> amount, int tour)
    {
        port.piecePlayed.PiecePlayedUp();
        BoardPiece piece = port.thisBoardPiece;
        //yield return piece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.classic,null);

        for (int i = 0; i < context.voisins.Count; i++)
        {
            BoardPiece voisin = context.voisins[i];
            port.thisBoardPiece = voisin;
            voisin.healthPoint -= piece.pieceInfo.soPiece.BaseEffectValues[0];
            
            piece.pieceInfo.soPiece.TempEffectValues[2] += piece.pieceInfo.soPiece.BaseEffectValues[1];
            voisin.shield += piece.pieceInfo.soPiece.TempEffectValues[2];
            voisin.pieceAnimation.RefreshHealth(voisin);
            yield return piece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.classic, null);
            yield return port.thisBoardPiece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.shield, piece);
            if (voisin.healthPoint <= 0)
            {
                context.healthManager.GiveStats(voisin.healthPoint, voisin.shield, voisin);
                context.healthManager.Dead(piece.pieceInfo.soPiece.BaseEffectValues[0]);
            }
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
            voisin.healthPoint -= piece.pieceInfo.soPiece.BaseEffectValues[0];
            
            piece.pieceInfo.soPiece.TempEffectValues[2] += piece.pieceInfo.soPiece.BaseEffectValues[1];
            voisin.shield += piece.pieceInfo.soPiece.TempEffectValues[2];
            voisin.pieceAnimation.RefreshHealth(voisin);
            if (i != 0) yield return piece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.classic, null);
            yield return port.thisBoardPiece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.shield, piece);
            if (voisin.healthPoint <= 0)
            {
                context.healthManager.GiveStats(voisin.healthPoint, voisin.shield, voisin);
                context.healthManager.Dead(piece.pieceInfo.soPiece.BaseEffectValues[0]);
            }
        }
        port.thisBoardPiece = piece;
        context.NbrDeRepetition += 1;

    }

}
