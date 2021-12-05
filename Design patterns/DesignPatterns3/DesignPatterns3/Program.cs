using System;
using System.Collections.Generic;
using System.Text;
using DesignPatterns3.Stateless;
using DesignPatterns3.SwitchExpressions;
using Stateless;
//using DesignPatterns3.StatePattern.SwitchBasedStateMachine;
//using DesignPatterns3.StatePattern.HandmadeStateMachine;

namespace DesignPatterns3
{
    #region Handmade State
    /*class Program
    {
        private static Dictionary<State, List<(Trigger, State)>> rules
            = new Dictionary<State, List<(Trigger, State)>>
            {
                [State.OffHook] = new List<(Trigger, State)>
                {
                    (Trigger.CallDialed, State.Connecting)
                },
                [State.Connecting] = new List<(Trigger, State)>
                {
                    (Trigger.HangUp, State.OffHook),
                    (Trigger.CallConnected, State.Connedted)
                },
                [State.Connedted] = new List<(Trigger, State)>
                {
                    (Trigger.LeftMessage,State.OffHook),
                    (Trigger.HangUp, State.OffHook),
                    (Trigger.PlacedOnHold, State.OnHold)
                },
                [State.OnHold] = new List<(Trigger, State)>
                {
                    (Trigger.TakeOffHold, State.Connedted),
                    (Trigger.HangUp, State.OffHook)
                }
            };

        static void Main(string[] args)
        {
            //Initial state
            var state = State.OffHook;
            while (true)
            {
                Console.WriteLine($"The phone is currently {state}");
                Console.WriteLine($"Select a trigger:");
                //
                for (int i = 0; i < rules[state].Count; i++)
                {
                    var (t,_) = rules[state][i];
                    Console.WriteLine($"{i}. {t}");
                }

                int input = int.Parse(Console.ReadLine());
                var (_, s) = rules[state][input];
                state = s;
            }
        }
    */
    #endregion
    #region SwitchBasedStateMachine
    /*class Program
    {
        static void Main(string[] args)
        {
            string code = "1234";
            var state = State.Locked;

            var entry = new StringBuilder();

            while (true)
            {
                switch (state)
                {
                    case State.Locked:
                        entry.Append(Console.ReadKey().KeyChar);
                        if (entry.ToString() == code)
                        {
                            state = State.Unlocked;
                            break;
                        }
                        if (!code.StartsWith(entry.ToString()))
                        {
                            //you can also use goto to go to another case without changing the state
                            state = State.Failed;
                            break;
                        }
                        break;
                    case State.Failed:
                        Console.CursorLeft = 0;
                        Console.WriteLine("FAILED");
                        entry.Clear();
                        state = State.Locked;
                        break;
                    case State.Unlocked:
                        Console.CursorLeft = 0;
                        Console.WriteLine("UNLOCKED");
                        return;
                }
            }
        }
    }*/
    #endregion
    #region SwitchExpressions
    /*class Program
    {
        static Chest Manipulate(Chest chest, ChestAction action, bool haveKey)
                => (chest, action, haveKey) switch
                {
                    (Chest.Locked, ChestAction.Open, true) => Chest.Open,
                    (Chest.Closed, ChestAction.Open, _) => Chest.Open,
                    (Chest.Open, ChestAction.Close, true) => Chest.Locked,
                    (Chest.Open, ChestAction.Close, false) => Chest.Closed,
                    _ => chest
                };

        static void Main(string[] args)
        {
            var chest = Chest.Locked;
            Console.WriteLine($"Chest is {chest}");
            chest = Manipulate(chest, ChestAction.Open, true);
            Console.WriteLine($"Chest is {chest}");
            chest = Manipulate(chest, ChestAction.Close, false);
            Console.WriteLine($"Chest is {chest}");
            chest = Manipulate(chest, ChestAction.Close, false);
            Console.WriteLine($"Chest is {chest}");
        }
    }*/
    #endregion
    #region State machine with stateless
    class Program
    {
        public static bool ParentsNotWatching { get; private set; }

        static void Main(string[] args)
        {
            var machine = new StateMachine<Health, Activity>(Health.NonReproductive);
            machine.Configure(Health.NonReproductive)
                .Permit(Activity.ReachPuberty, Health.Reproductive);
            machine.Configure(Health.Reproductive)
                .Permit(Activity.Historectomy, Health.NonReproductive)
                .PermitIf(Activity.HaveUnprotectedSex, Health.Pregnant,
                () => ParentsNotWatching);
            machine.Configure(Health.Pregnant)
             .Permit(Activity.GiveBirth, Health.Reproductive)
             .Permit(Activity.HaveAbortion, Health.Reproductive);

            
        }
    }
    #endregion
}