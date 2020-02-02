using System;
using System.Collections.Generic;

using UnityEngine;

namespace LWG.Core.Fsm {

// In this class, the type argument would 
// be the identifier for the list of states. 
// This would be useful for enumeration. 

// USAGE:
//  - Define an enum representing a set of states (T)
//  - Define an enum representing a set of triggers for transitions (U)
//  - Declare a state system with the state enum and transition enum (FiniteStateMachine<T, U>)
//  - Define a set of states with state objects cooresponding to the state enum 
//  - Add the states to the list for the given enum types (AddState)
//  - Add transitions between states with (AddTransition(T, T, U))
//  - Fire off triggers in your update loop and the state will transition automatically
//
//  NOTE(clark): Transitions will CONSUME the triggers they search for. 
//  NOTE(clark): Be real cool B)

public class FiniteStateMachine<T, U> 
    where T : IComparable // Enum or string
    where U : struct, IComparable, IFormattable // Enum or uint
{

    public class Transition {
        public T Target {get; private set; }
        public U[] Conditions {get; private set; }

        public Transition(T target, U[] conditions) {
            Target = target;
            Conditions = conditions;
        }
    }

    // Keyed list of states. Should be enumerated by a type. 
    Dictionary<T, IState> states = new Dictionary<T, IState>();

    // Keyed list of transitions for a given state. 
    Dictionary<T, List<Transition>> transitions = new Dictionary<T, List<Transition>>();
    HashSet<U> triggers = new HashSet<U>();
    List<U> _toRemoveList = new List<U>();

    T entryState;

    // NOTE(clark): THIS IS ONLY ACCESSABLE IN THE EXIT FUNCTION OF A STATE
    public T TargetState { get; private set; }
    public T CurrentState { get; private set; }
    public T PreviousState { get; private set; }

    public FiniteStateMachine(T startingState) {
        CurrentState = startingState;
        entryState = startingState;
    }

    public bool HasState(T key) => states.ContainsKey(key);

    public void AddState(T key, IState state) {
        if(state == null) throw new ArgumentException("State can't be null");
        
        if(!states.ContainsKey(key)) {
            states[key] = state;
            transitions[key] = new List<Transition>();
        }
        else {
            throw new ArgumentException("State already exists!");
        }
    }

    // Will only use distinct states from the list. 
    public void AddStates(IEnumerable<KeyValuePair<T, IState>> keyValues) {
        if(keyValues == null) throw new ArgumentException("states can't be null");

        foreach(var kvp in keyValues) {
            if(!states.ContainsKey(kvp.Key)) {
                states[kvp.Key] = kvp.Value;
                transitions[kvp.Key] = new List<Transition>();
            }
            else {
                throw new ArgumentException("State already exists!");
            }
        }
    }

    public IState RemoveState(T key) {
        // Can't delete the entry state
        if(states.ContainsKey(key) && !key.Equals(entryState)) { 
            IState state = states[key];
            states.Remove(key);
            transitions.Remove(key);
            return state;
        }
        throw new ArgumentException("State doesn't exist or state is the entry state!!");
    }

    public void AddTransition(T source, T target, params U[] conditions) {
        if(!(states.ContainsKey(source) && transitions.ContainsKey(source))) {
            throw new ArgumentException("Source State doesn't exist!");
        }

        if(!(states.ContainsKey(target) && transitions.ContainsKey(target))) {
            throw new ArgumentException("Target state doesn't exist!");
        }

        // Adds a new transition to the state system. 
        transitions[source].Add(new Transition(target, conditions));
    }

    // Adds the transition to ALL existing states. 
    // NOTE(clark): USE THIS ONLY AFTER adding all the states you want to the system.
    public void AddTransitionAll(T target, U condition, bool inclusive = true) =>
        AddTransitionAll(target, new [] { condition }, inclusive);

    // Adds the transition to ALL existing states. 
    // NOTE(clark): USE THIS ONLY AFTER adding all the states you want to the system.
    public void AddTransitionAll(T target, U[] conditions, bool inclusive = true) {
        // Loop through all states and add the transitions to them
        foreach(var kvp in transitions) {
            // If it's inclusive, add the transition to the target state transition list
            // If it's not, only add it if the target is not the key
            if(inclusive || !kvp.Key.Equals(target)) {
                AddTransition(kvp.Key, target, conditions);
            }
        }
    }


    // Targets the enumerated state
    void TransitionAway(T target) {
        // Exit
        TargetState = target;
        states[CurrentState].Exit();

        // swap
        PreviousState = CurrentState;
        CurrentState = target;

        // Debug.Log(PreviousState.ToString() + CurrentState.ToString());

        // Enter
        TargetState = default(T);
        states[CurrentState].Enter();
    }

    public void Reset() {
        if(!CurrentState.Equals(entryState)) {
            TransitionAway(entryState);
        }
    }


    public void SetTrigger(U trigger) => triggers.Add(trigger);
    public void SetTrigger(params U[] triggers) => Array.ForEach(triggers, x => SetTrigger(x));

    // Set for all triggers. 
    public void SetTrigger(float time, params U[] triggers) => Array.ForEach(triggers, x => SetTrigger(time, x));

    // Sometimes we jsut want to clear a trigger we know we don't want to care about!
    public void ReleaseTrigger(U trigger) => triggers.Remove(trigger);
    // If the trigger is active, return true.
    public bool CheckTrigger(U condition) => triggers.Contains(condition);

    public void Start() => states[entryState].Enter();

    public void Update(float deltaTime) {
   
        // Run this until we lang in a state that doesn't transition away with 
        //      the current triggers being thrown. 
        while(ProcessTriggers()) {}

        // Reset triggers. 
        triggers.Clear();

        // ProcessTriggers();
        states[CurrentState].Update();
    }

    bool ProcessTriggers() {
        foreach (var transition in transitions[CurrentState]) {

            // Check if the triggers have been fired bit bitwise ANDING
            //      all the triggers with the trigger mask and checking 
            //      if the trigger mask remains. 
            var conditions = transition.Conditions; // storing condition

            bool containsAll = true;
            foreach(var condition in conditions) {
                containsAll = containsAll && CheckTrigger(condition);
            }

            if(containsAll) {
                // Check if the constraints are not ALSO met.
                TransitionAway(transition.Target);

                // Eat the triggers that are matched
                foreach(var c in conditions){
                    triggers.Remove(c);
                }

                // We goochi. Get outta here. 
                return true;
            }
        }
        return false;
    }
}

} // LWG