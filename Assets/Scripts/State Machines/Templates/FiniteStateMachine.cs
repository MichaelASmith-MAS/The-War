using System.Collections.Generic;

public class FiniteStateMachine<T>
{
    private T currentState;
    private Dictionary<T, IState> states;

    public FiniteStateMachine(T defaultKey, IState defaultState)
    {
        states = new Dictionary<T, IState>();
        states.Add(defaultKey, defaultState);
        currentState = defaultKey;
    }

    public void RegisterState(T key, IState state)
    {
        states.Add(key, state);
    }

    public void UnregisterState(T key)
    {
        states.Remove(key);
    }

    public void ChangeState(T key)
    {
        if(!currentState.Equals(key))
        {
            states[currentState].Exit();
            currentState = key;
            states[currentState].Enter();
        }
    }

    public void RunState()
    {
        states[currentState].Execute();
    }

    public Dictionary<T, IState>.KeyCollection ListStates()
    {
        return states.Keys;
    }

    public void ClearStates()
    {
        states = new Dictionary<T, IState>();
    }

}

public interface IState
{
    void Enter();
    void Execute();
    void Exit();
}