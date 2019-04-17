﻿using QuizBotCore.States;

namespace QuizBotCore
{
    public interface IStateMachine<TCommand>
    {
        (State state, TCommand command) GetNextState<TState, TTransition>(TState currentState, TTransition transition)
            where TState : State where TTransition : Transition;
    }
}
