﻿using OpenQA.Selenium;
using System;
using System.Globalization;
using Tranquire.Selenium.Questions.Converters;

namespace Tranquire.Selenium.Questions
{
    /// <summary>
    /// Represent a question
    /// </summary>
    /// <typeparam name="TSource">The type of the value in the web page</typeparam>
    /// <typeparam name="TConverted">The converterd type of the value</typeparam>
    /// <typeparam name="TAnswer">The final type of the answer</typeparam>
    internal abstract class Question<TSource, TConverted, TAnswer> : IQuestion<TAnswer, BrowseTheWeb>
    {
        public Func<IWebElement, TSource> WebElementResolver { get; }
        public ITarget Target { get; }
        public IConverter<TSource, TConverted> Converter { get; }
        public CultureInfo Culture { get; }

        public Question(ITarget target, Func<IWebElement, TSource> webElementResolver, IConverter<TSource, TConverted> converter, CultureInfo culture)
        {
            Guard.ForNull(target, nameof(target));
            Guard.ForNull(webElementResolver, nameof(webElementResolver));
            Guard.ForNull(converter, nameof(converter));
            Guard.ForNull(culture, nameof(culture));
            Target = target;
            WebElementResolver = webElementResolver;
            Converter = converter;
            Culture = culture;
        }

        public abstract TAnswer AnsweredBy(IActor actor, BrowseTheWeb webDriver);

        protected TConverted Convert(TSource value)
        {
            return Converter.Convert(value, Culture);
        }
    }
}