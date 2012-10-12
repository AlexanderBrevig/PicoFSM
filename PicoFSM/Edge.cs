using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicoFSM
{
    public class Edge
    {
        public Edge(State state, Func<State, bool> condition)
        {
            this.State = state;
            this.Condition = condition;
        }

        public State State { get; set; }
        public Func<State, bool> Condition { get; set; }
    }
}
