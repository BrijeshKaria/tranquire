﻿using OpenQA.Selenium;
using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using Tranquire.Selenium.Questions.Converters;

namespace Tranquire.Selenium.Questions
{
    /// <summary>
    /// Represent the UI state for a list of elements in the page
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ManyUIState<T> : UIState<T>
    {
        private readonly Func<IWebElement, T> Resolve;

        /// <summary>
        /// Creates a new instance of <see cref="ManyUIState{T}"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="resolve"></param>
        /// <param name="culture"></param>
        public ManyUIState(ITarget target, Func<IWebElement, T> resolve, CultureInfo culture)
            : base(target, culture)
        {
            Resolve = resolve;
        }

        /// <summary>
        /// Gets a question which returns the state
        /// </summary>
        public IQuestion<ImmutableArray<T>, BrowseTheWeb> Value => CreateQuestion<T>(new GenericConverter<T, T>(t => t));

        /// <summary>
        /// Creates a question
        /// </summary>
        /// <typeparam name="TAnswer">The type of the answer elements</typeparam>
        /// <param name="converter">The converter used to convert the UI state value to the answer type</param>
        /// <returns></returns>
        public IQuestion<ImmutableArray<TAnswer>, BrowseTheWeb> As<TAnswer>(IConverter<T, TAnswer> converter)
        {
            return CreateQuestion(converter);
        }

        private IQuestion<ImmutableArray<TAnswer>, BrowseTheWeb> CreateQuestion<TAnswer>(IConverter<T, TAnswer> converter)
        {
            return new ManyQuestion<T, TAnswer>(Target, ResolveFor, converter, Culture);
        }

        /// <summary>
        /// Resolve the element state
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override T ResolveFor(IWebElement element)
        {
            return Resolve(element);
        }

        private class ManyQuestion<TSource, TConverted> : Question<TSource, TConverted, ImmutableArray<TConverted>>
        {
            public ManyQuestion(ITarget target,
                                Func<IWebElement, TSource> webElementResolver,
                                IConverter<TSource, TConverted> converter,
                                CultureInfo culture)
                : base(target, webElementResolver, converter, culture)
            {
            }

            public override ImmutableArray<TConverted> AnsweredBy(IActor actor, BrowseTheWeb webDriver)
            {
                var webElements = Target.ResoveAllFor(webDriver);
                return webElements.Select(w => Convert(WebElementResolver(w)))
                                  .ToImmutableArray();
            }
        }
    }
}