﻿using System;
using FluentAssertions;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Idioms;
using Tranquire.Extensions;
using Xunit;

namespace Tranquire.Tests.Extensions
{
    public class IfActionPredicateAndActionWithAbilityTests : IfActionTestsBase<IfActionWithAbility<Ability, AbilityAction, object>, IAction<Ability,Ability, object>, IAction<AbilityAction, AbilityAction, object>>
    {
        public interface IFunc
        {
            T Func<T1, T>(T1 value);
        }

        [Theory]
        [DomainAutoData]
        public void ExecuteWhen_WhenPredicateIsTrue_ShouldReturnCorrectValue(
            IFixture fixture,
            Mock<IActor> actor,
            object expected,
            Mock<IFunc> funcMock,
            Ability ability)
        {
            //arrange           
            funcMock.Setup(f => f.Func<Ability, bool>(ability)).Returns(true);
            Func<Ability, bool> func = funcMock.Object.Func<Ability, bool>;
            fixture.Inject(func);
            var sut = fixture.Create<IfActionWithAbility<Ability, AbilityAction, object>>();
            actor.Setup(a => a.Execute(sut.Action)).Returns(expected);
            //act
            var actual = sut.ExecuteWhenAs(actor.Object, ability);
            //assert
            actual.Should().Be(expected);
        }

        [Theory]
        [DomainAutoData]
        public void ExecuteWhen_WhenPredicateIsFalse_ShouldReturnCorrectValue(
           IFixture fixture,
           Mock<IActor> actor,
           Mock<IFunc> funcMock,
           Ability ability)
        {
            //arrange
            funcMock.Setup(f => f.Func<Ability, bool>(ability)).Returns(false);
            Func<Ability, bool> func = funcMock.Object.Func<Ability, bool>;
            fixture.Inject(func);
            var expected = fixture.Freeze<object>();
            var sut = fixture.Create<IfActionWithAbility<Ability, AbilityAction, object>>();
            //act
            var actual = sut.ExecuteWhenAs(actor.Object, ability);
            //assert
            actual.Should().Be(expected);
        }

        [Theory]
        [DomainAutoData]
        public void ExecuteGiven_WhenPredicateIsTrue_ShouldReturnCorrectValue(
          IFixture fixture,
          Mock<IActor> actor,
          object expected,
          Mock<IFunc> funcMock,
          Ability ability)
        {
            //arrange           
            funcMock.Setup(f => f.Func<Ability, bool>(ability)).Returns(true);
            Func<Ability, bool> func = funcMock.Object.Func<Ability, bool>;
            fixture.Inject(func);
            var sut = fixture.Create<IfActionWithAbility<Ability, AbilityAction, object>>();
            actor.Setup(a => a.Execute(sut.Action)).Returns(expected);
            //act
            var actual = sut.ExecuteGivenAs(actor.Object, ability);
            //assert
            actual.Should().Be(expected);
        }

        [Theory]
        [DomainAutoData]
        public void ExecuteGiven_WhenPredicateIsFalse_ShouldReturnCorrectValue(
           IFixture fixture,
           Mock<IActor> actor,
           Mock<IFunc> funcMock,
           Ability ability)
        {
            //arrange
            funcMock.Setup(f => f.Func<Ability, bool>(ability)).Returns(false);
            Func<Ability, bool> func = funcMock.Object.Func<Ability, bool>;
            fixture.Inject(func);
            var expected = fixture.Freeze<object>();
            var sut = fixture.Create<IfActionWithAbility<Ability, AbilityAction, object>>();
            //act
            var actual = sut.ExecuteGivenAs(actor.Object, ability);
            //assert
            actual.Should().Be(expected);
        }
    }
}
