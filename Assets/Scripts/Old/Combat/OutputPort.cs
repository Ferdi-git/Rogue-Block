
public class OutputPort 
{
    public BoardPiece thisBoardPiece;
    public StatsPlayer statsPlayer;
    public StatsEnnemi statsEnnemi;
    public SoNbrOfPiecePlayed piecePlayed;

    //player
    public void PlayerTakeDamage (int amount)
    {
        if (statsPlayer.GetShield() <= 0 )
        {
            statsPlayer.InvokeLoosePV(amount);
        }
        else
        {
            statsPlayer.InvokeTakeDamage(amount);
        }

        
    }
    public void PlayerLoosePV (int amount)
    {
        statsPlayer.InvokeLoosePV(amount);
    }
    public void PlayerHeal (int amount)
    {
        statsPlayer.InvokeGainPV(amount);
    }

    public void PlayerLooseMana(int amount)
    {
        statsPlayer.InvokeLooseMana(amount);
    }
    public void PlayerGainMana(int amount)
    {
        statsPlayer.InvokeGainMana(amount);
    }

    public void PlayerGainShield (int amount)
    {
        statsPlayer.InvokeGainShield (amount);
    }
    public void PlayerLooseShield (int amount)
    {
        statsPlayer.InvokeTakeDamage (amount);
    }



    //ennemi 

    public void DoDamageToEnnemi (int amount )
    {
        if (statsEnnemi.GetShield() <= 0)
        {
            statsEnnemi.EnnemiLoosePV(amount);
        }
        else
        {
            statsEnnemi.EnnemiTakeDamage(amount);
        }
    }
    public void EnnemiHeal (int amount)
    {
        statsEnnemi.EnnemiGainPV(amount);
    }
    public void EnnemiGainShield (int amount )
    {
        statsEnnemi.EnnemiGainShield(amount);
    }
    public void EnnemiLooseShield(int amount)
    {
        statsEnnemi.EnnemiTakeDamage(amount);
    }

    //to this piece 

    public void ThisPieceTakeDamage (int value)
    {
        thisBoardPiece.healthPoint -= value;
    }

    
}
