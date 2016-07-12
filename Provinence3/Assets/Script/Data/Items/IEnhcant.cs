using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public interface IEnhcant
{
    BaseItem BaseItem { get; }

    void Enchant(int sum);

}

