﻿using OpenQA.Selenium.Firefox;
using Tranquire;
using Tranquire.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using ToDoList.Automation.Actions;

namespace ToDoList.Specifications
{
    public class ToDoListSteps
    {
        public ScenarioContext Context { get; }

        public ToDoListSteps(ScenarioContext context)
        {
            Context = context;
        }

        [BeforeScenario]
        public void Before()
        {
            var driver = new FirefoxDriver();
            Context.Set(new Actor("John").Can(BrowseTheWeb.With(driver)));
            Context.Actor().WasAbleTo(Open.TheApplication());
        }

        [AfterScenario]
        public void After()
        {
            Context.Actor().AbilityTo<BrowseTheWeb>().Driver.Dispose();
        }

        [Given(@"I have an empty to-do list")]
        public void GivenIHaveAnEmptyTo_DoList()
        {
        }

        [Given(@"I add the item ""(.*)""")]
        [When(@"I add the item ""(.*)""")]
        public void WhenIAddTheItem(string item)
        {
            Context.Actor().AttemptsTo(ToDoItem.AddAToDoItem(item));
        }

        [Given(@"I have a list with the items ""(.*)""")]
        public void GivenIHaveAListWithTheItems(ImmutableArray<string> items)
        {
            Context.Actor().AttemptsTo(ToDoItem.AddToDoItems(items));
        }

        [StepArgumentTransformation]
        public ImmutableArray<string> TransformToListOfString(string commaSeparatedList)
        {
            return commaSeparatedList.Split(',').ToImmutableArray();
        }

        [When(@"I remove the item ""(.*)""")]
        public void WhenIRemoveTheItem(string item)
        {
            Context.Actor().AttemptsTo(ToDoItem.RemoveAToDoItem(item));
        }
    }
}
