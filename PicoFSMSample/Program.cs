using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicoFSM
{
    class Program
    {
        static void Main(string[] args)
        {
            Machine machine = new Machine();

            /// Definition of the wake state
            /// This state will just print Alarm sounds! and add alarmSounded=true to the global payload
            var wake = new State(
                "wake",
                (s) => {
                    Console.WriteLine("Alarm sounds!");
                    State.GlobalPayload["alarmSounded"] = true;
                    return true;
                }
            );

            /// Definition of the breakfeast state
            /// It will print Cut bread to the console, remove the alarmSounded from global payload 
            /// and initialize the local state step = 1
            /// Then, during the update phase, it prints Make sandwitch step x of 3
            /// until the third step, then  we continue to the exit action
            /// Finally we print Eat bread to the console
            var breakfeast = new State(
                "breakfast",
                (s) => {
                    Console.WriteLine("Cut bread");
                    State.GlobalPayload.Remove("alarmSounded");
                    s.Payload["step"] = 1;
                    return true;
                },
                (s) => {
                    Console.WriteLine("Make sandwitch step " + s.Payload["step"] + " of 3");
                    int next = Int32.Parse("" + s.Payload["step"]) + 1;
                    s.Payload["step"] = next;
                    return next > 3;
                },
                (s) => {
                    Console.WriteLine("Eat bread");
                    return true;
                }
            );

            int iteration = 0;

            /// This is a rule that whenever iteration is 3, we transition to wake state
            machine.TransitionToWhen(wake, (s) => {
                return iteration == 3;
            });

            /// This rule says that we transition to breakfast state when the global payload contains a key for alarmSounded
            machine.TransitionToWhen(breakfeast, (s) => {
                return State.GlobalPayload.ContainsKey("alarmSounded");
            });

            /// process the state machine
            for (iteration = 0; iteration < 12; iteration++) {
                machine.Update();
            }

            Console.Read();
        }
    }
}
