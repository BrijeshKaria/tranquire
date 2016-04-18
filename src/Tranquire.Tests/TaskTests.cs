﻿using FluentAssertions;
using Moq;
using Ploeh.AutoFixture;
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
    public class TaskTests
    {
        [Theory, DomainAutoData]
        public void Sut_ShouldBeAction(Task sut)
        {
            sut.Should().BeAssignableTo<IAction>();
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(Task));
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyConstructorParameters(ConstructorInitializedMemberAssertion assertion)
        {
            assertion.Verify(typeof(Task));
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyConstructorWithParams(IAction[] expected)
        {
            //act            
            var sut = new Task(expected);
            //assert
            sut.Actions.Should().Equal(expected);
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_ShouldCallActorExecute(            
            Task sut,
            Mock<IActor> actor
            )
        {
            //arrange
            //act
            sut.ExecuteGivenAs(actor.Object);
            //assert
            foreach (var action in sut.Actions)
            {
                actor.Verify(a => a.Execute(action), Times.Once());
            }
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_ShouldCallActorExecute(
            Task sut,
            Mock<IActor> actor
            )
        {
            //arrange
            //act
            sut.ExecuteWhenAs(actor.Object);
            //assert
            foreach (var action in sut.Actions)
            {
                actor.Verify(a => a.Execute(action), Times.Once());
            }
        }
    }
}
