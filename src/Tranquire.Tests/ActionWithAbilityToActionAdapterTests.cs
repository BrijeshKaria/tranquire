﻿using FluentAssertions;
using Moq;
using Ploeh.AutoFixture.Idioms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Tests;
using Xunit;

namespace Tranquire.Tests
{
    public class ActionWithAbilityToActionAdapterTests
    {
        [Theory, DomainAutoData]
        public void Sut_ShouldBeAction(ActionWithAbilityToActionAdapter<object, object, Unit> sut)
        {
            sut.Should().BeAssignableTo<IAction<Unit>>();
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ActionWithAbilityToActionAdapter<object, object, Unit>));
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyConstructorInitialization(ConstructorInitializedMemberAssertion assertion)
        {
            assertion.Verify(typeof(ActionWithAbilityToActionAdapter<object, object, Unit>).GetConstructors());
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_ShouldCallActorWithAction(
            ActionWithAbilityToActionAdapter<object, object, Unit> sut,
            Mock<IActor> actor)
        {
            //act
            sut.ExecuteGivenAs(actor.Object);
            //arrange
            actor.Verify(a => a.ExecuteWithAbility(sut.Action));
        }
        
        [Theory, DomainAutoData]
        public void ExecuteWhenAs_ShouldCallActorWithAction(
            ActionWithAbilityToActionAdapter<object, object, Unit> sut,
            Mock<IActor> actor)
        {
            //act
            sut.ExecuteWhenAs(actor.Object);
            //arrange
            actor.Verify(a => a.ExecuteWithAbility(sut.Action));
        }
    }
}
