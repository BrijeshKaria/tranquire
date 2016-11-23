﻿using Tranquire;
using Tranquire.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Selenium.Actions;

namespace ToDoList.Automation.Actions
{
    public class ToDoItem : Tranquire.Task
    {
        public string Title { get; }

        public ToDoItem(string title) :
            base(t => t.And(Enter.TheValue(title).Into(ToDoPage.NewToDoItemInput))
                       .And(Hit.Enter().Into(ToDoPage.NewToDoItemInput)))
        {
            Title = title;
        }

        public static IAction<Unit> RemoveAToDoItem(string item)
        {
            return new RemoveToDoItem(item);
        }

        public static IAction<Unit> AddAToDoItem(string title)
        {
            return new ToDoItem(title);
        }

        public static IAction<Unit> AddToDoItems(ImmutableArray<string> items)
        {
            return new Tranquire.Task(items.Select(i => new ToDoItem(i)).ToArray());
        }

        public override string ToString()
        {
            return "Add item '" + Title + "'";
        }
    }
}
