using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballData
{
    public class TeamNotFoundException : Exception
    {
        public TeamNotFoundException() : base() { }
    }
}
