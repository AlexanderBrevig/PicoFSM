using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicoFSM
{
    public class State
    {
        public State(string name, Func<State, bool> update)
        {
            this.Name = name;
            this.OnEnter = (s) => { Active = true; return true; };
            this.OnUpdate = update;
            this.OnExit = (s) => { Active = false; return true; };
            Initialize();
        }

        public State(string name, Func<State, bool> enter, Func<State, bool> update, Func<State, bool> exit)
        {
            this.Name = name;
            this.OnEnter = (s) => { Active = true; return enter(s); };
            this.OnUpdate = update;
            this.OnExit = (s) => { Active = false; return exit(s); };
            Initialize();
        }

        public override string ToString()
        {
            return Name;
        }

        public Func<State, bool> OnEnter { get; set; }
        public Func<State, bool> OnUpdate { get; set; }
        public Func<State, bool> OnExit { get; set; }
        public Dictionary<object, object> Payload { get; set; }
        public static Dictionary<object, object> GlobalPayload { get; set; }

        public string Name { get; set; }

        private void Initialize()
        {
            Payload = new Dictionary<object, object>();
            if (GlobalPayload == null) {
                GlobalPayload = new Dictionary<object, object>();
            }
        }

        public bool Active { get; set; }
    }
}
