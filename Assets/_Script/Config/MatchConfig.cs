using System.Collections;
using System.Collections.Generic;


public class MatchConfig 
{
    public int matchId;
    public List<PlayerConfig> listPlayer;
    public float timeLimit;

    public static MatchConfig CreatePvPMatch(int idMatch, int p1Id, string p1Name, PlayerType p1Type, int p2Id,  string p2Name, PlayerType p2Type, float time)
    {
        return new MatchConfig
        {
            matchId = idMatch,
            timeLimit = time,
            listPlayer = new List<PlayerConfig>
            {
                new PlayerConfig { Id = p1Id, name = p1Name, type = p1Type },
                new PlayerConfig { Id = p2Id, name = p2Name, type = p2Type }
            }
        };
    }
}
