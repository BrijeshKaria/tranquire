﻿using FluentAssertions;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tranquire.Tests
{
    public class ActorExtensionsTest
    {
        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ActorExtensions));
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyActorDecoratorBehavior(ActorDecoratorExtensionAssertion assertion)
        {
            assertion.Verify(typeof(ActorExtensions));
        }

        [Theory, DomainAutoData]
        public void TakeScreenshots_ShouldDecorateActor(
            ActorDecoratorExtensionAssertion assertion,
            [Modest]Actor actor,
            ReportingActor expected)
        {
            //arrange
            //act
            var actual = ActorExtensions.WithReporting(actor, expected.Observer).InnerActorBuilder(expected.Actor);
            //assert
            actual.Should().BeOfType<ReportingActor>().Which.ShouldBeEquivalentTo(expected);
        }
    }
}
