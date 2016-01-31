using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class BossBornPosition :BaseBornPosition
{

    public override BornPositionType GetBornPositionType()
    {
        return BornPositionType.boss;
    }
}

