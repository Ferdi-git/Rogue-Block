using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

[Serializable]
public class Context 
{
    public int Tour;

    //public int NbrCaseDeLaPiece;

    public int NbrCaseLibre;
    public int NbrCaseOccupe;

    public int NbrDeRepetition;

    // script de stats joueur 
    // script de stats ennemi 

    public List<BoardPiece> voisins;
    public int NbrCaseAtk =0 ;
    public int NbrCaseGenerateMana =0;

    public ConditionOutput conditionOutput;
    public PieceHealthManager healthManager;
}
