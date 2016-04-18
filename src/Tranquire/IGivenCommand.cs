﻿namespace Tranquire
{
    /// <summary>
    /// Represent a action executed in order to put the system in a given context
    /// </summary>
    public interface IGivenCommand<T>
    {
        /// <summary>
        /// Execute the action with the given actor
        /// </summary>        
        /// <param name = "actor">The actor used to execute the action</param>
        /// <param name="ability">The ability required for this action by the actor</param>
        /// <returns>The current <see cref = "IActor"/> instance, allowing to chain calls</returns>
        void ExecuteGivenAs(IActor actor, T ability);
    }

    /// <summary>
    /// Represent a action executed in order to put the system in a given context
    /// </summary>
    public interface IGivenCommand
    {
        /// <summary>
        /// Execute the action with the given actor
        /// </summary>        
        /// <param name = "actor">The actor used to execute the action</param>
        /// <returns>The current <see cref = "IActor"/> instance, allowing to chain calls</returns>
        void ExecuteGivenAs(IActor actor);
    }
}