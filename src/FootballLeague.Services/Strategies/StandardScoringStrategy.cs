using FootballLeague.Domain.Models;

namespace FootballLeague.Services.Strategies;

public class StandardScoringStrategy : IScoringStrategy
{
    public void Calculate(Team home, Team away, int homeScore, int awayScore)
    {
        if (homeScore > awayScore) 
        {
            home.AddWin(); 
            away.AddLoss(); 
        }
        else if (awayScore > homeScore) 
        { 
            away.AddWin();
            home.AddLoss(); 
        }
        else 
        { 
            home.AddDraw();
            away.AddDraw();
        }
    }

    public void RevertPoints(Team home, Team away, int homeScore, int awayScore)
    {
        if (homeScore > awayScore) 
        { 
            home.RemoveWin();
            away.RemoveLoss();
        }
        else if (awayScore > homeScore) 
        { 
            away.RemoveWin();
            home.RemoveLoss();
        }
        else 
        { 
            home.RemoveDraw();
            away.RemoveDraw();
        }
    }
}
