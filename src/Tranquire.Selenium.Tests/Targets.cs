﻿using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tranquire.Selenium.Tests
{
    public class TargetsTests : IClassFixture<WebDriverFixture>
    {
        private readonly WebDriverFixture _fixture;

        public const string PElementId = "pelement";
        public const string PElementText = "A simple p element text";
        public const string PElementClass = "pelementclass";
        public const string UlElement = "ulelement";
        public static readonly string[] LiElementsContent = new[] { "Red", "Blue", "Yellow", "Green", "Black" };
        public const string YellowClass = "yellow";
        public const string YellowContent = "Yellow";
        public const string DivContainer = "divcontainer";
        public const string RelativeText = "Relative Text";

        public TargetsTests(WebDriverFixture fixture)
        {
            fixture.NavigateTo("Targets.html");
            _fixture = fixture;
        }

        [Fact]
        public void ResolveFor_LocatedById_ShouldReturnCorrectValue()
        {
            var actual = Target.The("target by id")
                               .LocatedBy(By.Id(PElementId))
                               .ResolveFor(_fixture.Actor);
            Assert.Equal(PElementText, actual.Text);
        }

        [Fact]
        public void ResolveFor_LocatedByCssId_ShouldReturnCorrectValue()
        {
            var actual = Target.The("target by css")
                               .LocatedBy(By.CssSelector("#" + PElementId))
                               .ResolveFor(_fixture.Actor);
            Assert.Equal(PElementText, actual.Text);
        }

        [Fact]
        public void ResolveFor_LocatedByCssClass_ShouldReturnCorrectValue()
        {
            var actual = Target.The("target by class")
                               .LocatedBy(By.CssSelector("." + PElementClass))
                               .ResolveFor(_fixture.Actor);
            Assert.Equal(PElementText, actual.Text);
        }

        [Fact]
        public void ResolveFor_LocatedByFormattableId_ShouldReturnCorrectValue()
        {
            var actual = Target.The("target formattable by id")
                               .LocatedBy(PElementId, s => By.Id(s))
                               .Of()
                               .ResolveFor(_fixture.Actor);
            Assert.Equal(PElementText, actual.Text);
        }

        [Fact]
        public void ResolveFor_LocatedByFormattableId_WithParameters_ShouldReturnCorrectValue()
        {
            var actual = Target.The("target formattable by id with parameter")
                               .LocatedBy("{0}", s => By.Id(s))
                               .Of(PElementId)
                               .ResolveFor(_fixture.Actor);
            Assert.Equal(PElementText, actual.Text);
        }

        [Fact]
        public void ResolveAllFor_LocatedByCss_ShouldReturnCorrectValue()
        {
            var actual = Target.The("target by id")
                               .LocatedBy(By.CssSelector($"ul#{UlElement} li"))
                               .ResoveAllFor(_fixture.Actor)
                               .Select(w => w.Text);
            Assert.Equal(LiElementsContent, actual);
        }

        [Fact]
        public void ResolveAllFor_LocatedByFormattableCss_ShouldReturnCorrectValue()
        {
            var actual = Target.The("target by id")
                               .LocatedBy($"ul#{UlElement} li", s => By.CssSelector(s))
                               .Of()
                               .ResoveAllFor(_fixture.Actor)
                               .Select(w => w.Text);
            Assert.Equal(LiElementsContent, actual);
        }

        [Fact]
        public void ResolveAllFor_LocatedByFormattableCss_WithParameters_ShouldReturnCorrectValue()
        {
            var actual = Target.The("target by id")
                               .LocatedBy("ul#{0} li", s => By.CssSelector(s))
                               .Of(UlElement)
                               .ResoveAllFor(_fixture.Actor)
                               .Select(w => w.Text);
            Assert.Equal(LiElementsContent, actual);
        }

        [Fact]
        public void ResolveAllFor_LocatedByFormattableXPath_WithParameters_ShouldReturnCorrectValue()
        {
            var actual = Target.The("target by id")
                               .LocatedBy("//ul[@id='{0}']/li[not(contains(@class, '{1}'))]", s => By.XPath(s))
                               .Of(UlElement, YellowClass)
                               .ResoveAllFor(_fixture.Actor)
                               .Select(w => w.Text);
            Assert.Equal(LiElementsContent.Where(s => s != YellowContent), actual);
        }

        [Fact]
        public void ResolveFor_LocatedByCss_RelativeToOtherTarget_ShouldReturnCorrectValue()
        {
            var containerTarget = Target.The("container")
                                        .LocatedBy(By.Id(DivContainer));

            var actual = Target.The("relative text")                               
                               .LocatedBy(By.CssSelector("div p"))
                               .RelativeTo(containerTarget)
                               .ResolveFor(_fixture.Actor)
                               .Text;
            Assert.Equal(RelativeText, actual);
        }
    }
}
