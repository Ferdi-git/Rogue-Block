using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEffetTest", menuName = "Effet/effetRepeatAround")]
public class SoEffetRepeatAround : SoEffet
{
    public override IEnumerator Effet(Context context,OutputPort port, List<int> amount, int tour)
    {
        port.piecePlayed.PiecePlayedUp();
        //yield return port.thisBoardPiece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.classic);
        BoardPiece piece = port.thisBoardPiece;
        context.NbrDeRepetition += 1;

        for (int i = 0; i < context.voisins.Count; i++)
            {
                BoardPiece voisin = context.voisins[i];
                port.thisBoardPiece = voisin;
                ConditionOutput conditionOutput =  CreateNewConditionOutput(port);
                yield return piece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPiecePlayed(), PieceAnimations.TypeAnim.classic,null);

                if (voisin.soPieces.pieceEffet.condition.Condition(conditionOutput))
                {
                    yield return voisin.soPieces.pieceEffet.effet.RepeatEffet(voisin.context, port, voisin.soPieces.TempEffectValues, tour, piece);
                }
                

            }
            port.thisBoardPiece = piece;

    }

    public override IEnumerator RepeatEffet(Context context, OutputPort port, List<int> amount, int tour, BoardPiece declencheur)
    {
        port.piecePlayed.RepeatedPieceUp();

        BoardPiece piece = port.thisBoardPiece;
        context.NbrDeRepetition += 1;

        for (int i = 0; i < context.voisins.Count; i++)
            {
                BoardPiece voisin = context.voisins[i];
                port.thisBoardPiece = voisin;
                ConditionOutput conditionOutput =  CreateNewConditionOutput(port);
                if (!voisin.soPieces.isRepetition)
                {
                    yield return port.thisBoardPiece.pieceAnimation.PlayAnimations(port.piecePlayed.GetPieceRepeated(), PieceAnimations.TypeAnim.repeat, declencheur);

                    if (voisin.soPieces.pieceEffet.condition.Condition(conditionOutput))
                    {
                        yield return voisin.soPieces.pieceEffet.effet.RepeatEffet(voisin.context, port, voisin.soPieces.TempEffectValues, tour, piece);
                    }
                    
                }

            }
            port.thisBoardPiece = piece;
    }

    private ConditionOutput CreateNewConditionOutput (OutputPort port)
    {
        ConditionOutput conditionOutput = new ConditionOutput();
        conditionOutput.port = port;
        conditionOutput.context = port.thisBoardPiece.context;
        conditionOutput.variableList = port.thisBoardPiece.soPieces.ConditionValues;
        return conditionOutput;
    }
}
