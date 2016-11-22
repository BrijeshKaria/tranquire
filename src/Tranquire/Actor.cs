﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    /// <summary>
    /// Represent an actor which use the system under test. The actor can be given capabilities, such as browsing the web, with the method <see cref="Can{T}(T)"/>
    /// </summary>
    public class Actor : IActorFacade
    {
        private readonly IReadOnlyDictionary<Type, object> _abilities;

        /// <summary>
        /// Returns the abilities for this actor
        /// </summary>
        public IReadOnlyDictionary<Type, object> Abilities => _abilities;

        /// <summary>
        /// Gets the actor name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Create a new instance of <see cref="Actor"/>
        /// </summary>
        /// <param name="name">The actor's name</param>
        /// <param name="abilities">A dictionary containing the abilities for the actor</param>
        public Actor(string name, IReadOnlyDictionary<Type, object> abilities)
        {
            Guard.ForNull(name, nameof(name));
            Guard.ForNull(abilities, nameof(abilities));
            Name = name;
            _abilities = abilities;
        }

        /// <summary>
        /// Create a new instance of <see cref="Actor"/> without abilities
        /// </summary>
        /// <param name="name"></param>
        public Actor(string name):this(name, new Dictionary<Type, object>()) { }
                
        /// <summary>
        /// Retrieve an actor's ability
        /// </summary>
        /// <typeparam name="T">The type of ability to retrieve</typeparam>
        /// <returns>The ability</returns>
        /// <exception cref="InvalidOperationException">The actor does not have the requested ability</exception>
        private T AbilityTo<T>()
        {
            object ability;
            if(!_abilities.TryGetValue(typeof(T), out ability))
            {
                throw new InvalidOperationException($"The ability {typeof(T).Name} was requested but the actor {Name} does not have it.");
            }
            return (T)_abilities[typeof(T)];
        }

        /// <summary>
        /// Execute the action in the When context with an ability
        /// </summary>
        /// <typeparam name="T">Type of the required ability</typeparam>
        /// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>
        /// <param name="command">The command to execute</param>
        public TResult AttemptsTo<T, TResult>(IWhenCommand<T, TResult> command)
        {
            Guard.ForNull(command, nameof(command));
            return command.ExecuteWhenAs(this, AbilityTo<T>());
        }

        /// <summary>
        /// Execute the action in the Given context with an ability
        /// </summary>
        /// <typeparam name="T">Type of the required ability</typeparam>
        /// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>
        /// <param name="command">The command to execute</param>
        public TResult WasAbleTo<T, TResult>(IGivenCommand<T, TResult> command)
        {
            Guard.ForNull(command, nameof(command));
            var actor = new GivenActor(this);
            return command.ExecuteGivenAs(actor, AbilityTo<T>());
        }

        /// <summary>
        /// Execute the action in the When context
        /// </summary>
        /// <param name="command">The command to execute</param>
        /// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>
        public TResult AttemptsTo<TResult>(IWhenCommand<TResult> command)
        {
            Guard.ForNull(command, nameof(command));
            return command.ExecuteWhenAs(this);
        }

        /// <summary>
        /// Execute the action in the Given context
        /// </summary>
        /// <param name="command"></param>
        /// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>
        public TResult WasAbleTo<TResult>(IGivenCommand<TResult> command)
        {
            Guard.ForNull(command, nameof(command));
            var actor = new GivenActor(this);
            return command.ExecuteGivenAs(actor);
        }

        /// <summary>
        /// Give an ability to the actor
        /// </summary>
        /// <typeparam name="T">The type of the ability</typeparam>
        /// <param name="doSomething">Ability</param>
        /// <returns>A new actor with the given ability</returns>
        public IActorFacade Can<T>(T doSomething) where T : class
        {            
            Guard.ForNull(doSomething, nameof(doSomething));
            var abilities = _abilities.Concat(new[] { new KeyValuePair<Type, object>(typeof(T), doSomething) })
                                      .ToDictionary(k => k.Key, k => k.Value);
            return new Actor(Name, abilities);
        }

        /// <summary>
        /// Ask a question about the current state of the system
        /// </summary>
        /// <typeparam name="TAnswer">Type answer's type</typeparam>
        /// <param name="question">A <see cref="IQuestion{TAnswer}"/> instance representing the question to ask</param>
        /// <returns>The answer to the question.</returns>
        public TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question)
        {
            Guard.ForNull(question, nameof(question));
            return question.AnsweredBy(this);
        }

        /// <summary>
        /// Ask a question about the current state of the system with an ability
        /// </summary>
        /// <typeparam name="TAnswer">Type answer's type</typeparam>
        /// <typeparam name="TAbility">Type of the required ability</typeparam>
        /// <param name="question">A <see cref="IQuestion{TAnswer}"/> instance representing the question to ask</param>
        /// <returns>The answer to the question.</returns>
        public TAnswer AsksFor<TAnswer, TAbility>(IQuestion<TAnswer, TAbility> question)
        {
            Guard.ForNull(question, nameof(question));
            return question.AnsweredBy(this, AbilityTo<TAbility>());
        }

        /// <summary>
        /// Execute an action with an ability
        /// </summary>
        /// <typeparam name="TGiven">The ability required for the Given context</typeparam>
        /// <typeparam name="TWhen">The ability required for the When context</typeparam>
        /// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>
        /// <param name="action">The action to execute</param>
        public TResult Execute<TGiven, TWhen, TResult>(IAction<TGiven, TWhen, TResult> action)
        {
            Guard.ForNull(action, nameof(action));
            return action.ExecuteWhenAs(this, AbilityTo<TWhen>());           
        }

        /// <summary>
        /// Execute an action
        /// </summary>
        /// <param name="action">The action to execute</param>
        /// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>
        public TResult Execute<TResult>(IAction<TResult> action)
        {
            Guard.ForNull(action, nameof(action));
            return action.ExecuteWhenAs(this);
        }

        private class GivenActor : IActor
        {
            private readonly Actor _innerActor;

            public GivenActor(Actor innerActor)
            {
                _innerActor = innerActor;
            }

            public TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question)
            {
                return _innerActor.AsksFor(question);
            }

            public TAnswer AsksFor<TAnswer, TAbility>(IQuestion<TAnswer, TAbility> question)
            {
                return _innerActor.AsksFor(question);                
            }
        
            public TResult Execute<TResult>(IAction<TResult> action)
            {
                return action.ExecuteGivenAs(this);
            }

            public TResult Execute<TGiven, TWhen, TResult>(IAction<TGiven, TWhen, TResult> action)
            {
                return action.ExecuteGivenAs(this, _innerActor.AbilityTo<TGiven>());
            }
        }
    }
}
