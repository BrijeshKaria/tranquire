﻿using OpenQA.Selenium;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Selenium.Actions;
using Tranquire.Selenium.Questions;
using Xunit;

namespace Tranquire.Selenium.Tests.Actions
{
    public class EnterTests : WebDriverTest
    {
        public EnterTests(WebDriverFixture fixture) : base(fixture, "Actions.html")
        {
        }       

        [Theory, AutoData]
        public void EnterNewValue_ShouldReturnCorrectValue(string expected)
        {
            //arrange
            var target = Target.The("new value").LocatedBy(By.Id("EnterNewValue"));
            var action = Enter.TheNewValue(expected).Into(target);
            //act
            Fixture.Actor.AttemptsTo(action);
            var actual = Answer(Value.Of(target).Value);
            //assert
            Assert.Equal(expected, actual);
        }
    }
}
