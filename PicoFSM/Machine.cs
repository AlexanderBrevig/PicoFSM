using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicoFSM
{
    public class Machine
    {
        public Machine()
        {
            Edges = new List<Edge>();
        }

        public void Update()
        {
            foreach (var edge in Edges) {
                if (currentState != null && edge.Condition(currentState)) {
                    TransitionTo(edge.State);
                    if (edge.Callback != null) {
                        edge.Callback(edge.State);
                    }
                }
            }

            if (nextState != null && currentState != null) {
                if (currentState.OnExit == null || currentState.OnExit(currentState)) {
                    currentState = null;
                }
            }
            else if (nextState != null && currentState == null) {
                if (nextState.OnEnter == null || nextState.OnEnter(nextState)) {
                    currentState = nextState;
                    nextState = null;
                }
            }
            else if (currentState != null) {
                if (currentState.OnUpdate(currentState)) {
                    exitNextTick = true;
                }
            }
        }

        public void TransitionTo(PicoFSM.State next)
        {
            if (next != null && next != nextState) {
                nextState = next;
            }
        }

        public Machine TransitionToWhen(PicoFSM.State state, Func<PicoFSM.State, bool> condition)
        {
            Edges.Add(new Edge(state, condition, null));
            return this;
        }

        public Machine TransitionToWhenThen(PicoFSM.State state, Func<PicoFSM.State, bool> condition, Action<PicoFSM.State> callback)
        {
            Edges.Add(new Edge(state, condition, callback));
            return this;
        }

        public List<Edge> Edges { get; set; }

        private PicoFSM.State currentState;
        private PicoFSM.State nextState;
        private bool exitNextTick = false;
    }
}