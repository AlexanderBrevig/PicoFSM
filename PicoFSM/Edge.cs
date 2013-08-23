using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Edge
{
    public Edge(PicoFSM.State state, Func<PicoFSM.State, bool> condition, Action<PicoFSM.State> callback)
    {
        this.State = state;
        this.Condition = condition;
        this.Callback = callback;
    }

    public PicoFSM.State State { get; set; }
    public Func<PicoFSM.State, bool> Condition { get; set; }
    public Action<PicoFSM.State> Callback { get; set; }
}