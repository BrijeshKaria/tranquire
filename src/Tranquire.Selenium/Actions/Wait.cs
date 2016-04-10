﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire.Selenium.Actions
{
    /// <summary>
    /// An action used to wait for a condition
    /// </summary>
    public class Wait : Action
    {
        private readonly TimeSpan _timeout;
        private readonly ITarget _target;

        /// <summary>
        /// Creates a new instance of <see cref="Wait"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="timeout"></param>
        public Wait(ITarget target, TimeSpan timeout)
        {
            Guard.ForNull(target, nameof(target));
            _target = target;
            _timeout = timeout;
        }

        /// <summary>
        /// Creates a new instance of <see cref="Wait"/>
        /// </summary>
        /// <param name="target"></param>
        public Wait(ITarget target) : this(target, TimeSpan.FromSeconds(5)) { }

        /// <summary>
        /// Change the maximum duration to wait
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public Wait Timeout(TimeSpan timeout)
        {
            return new Wait(_target, timeout);
        }

        /// <summary>
        /// Wait
        /// </summary>
        /// <param name="actor"></param>
        protected override void ExecuteWhen(IActor actor)
        {
            var wait = new WebDriverWait(actor.BrowseTheWeb(), _timeout);
            try
            {
                wait.Until(_ => _target.ResolveFor(actor));
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new TimeoutException($"The target '{_target.Name}' was not present after waiting {_timeout.ToString()}", ex);
            }
        }

        /// <summary>
        /// Returns a new wait action for the given target
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Wait UntilTargetIsPresent(ITarget target)
        {
            return new Wait(target);
        }
    }
}
