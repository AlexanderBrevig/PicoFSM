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
                if (edge.Condition(currentState)) {
                    TransitionTo(edge.State);
                }
            }

            if (exitNextTick) {
                if (currentState.OnExit != null) {
                    currentState.OnExit(currentState);
                }
                currentState = nextState;
                nextState = null;
                exitNextTick = false;
            } else if (nextState != null && currentState != null) {
                if (currentState.OnExit == null || currentState.OnExit(currentState)) {
                    currentState = null;
                }
            } else if (nextState != null && currentState == null) {
                if (nextState.OnEnter == null || nextState.OnEnter(nextState)) {
                    currentState = nextState;
                    nextState = null;
                }
            } else if (currentState != null) {
                if (currentState.OnUpdate(currentState)) {
                    exitNextTick = true;
                }
            }
        }

        public void TransitionTo(State next)
        {
            if (next != null && next != nextState) {
                if (currentState == null) {
                    if (next.OnEnter == null || (next.OnEnter != null && next.OnEnter(next))) {
                        currentState = next;
                    }
                } else {
                    nextState = next;
                }
            }
        }

        public Machine TransitionToWhen(State state, Func<State, bool> condition)
        {
            Edges.Add(new Edge(state, condition));
            return this;
        }

        public List<Edge> Edges { get; set; }

        private State currentState;
        private State nextState;
        private bool exitNextTick = false;
    }
}
