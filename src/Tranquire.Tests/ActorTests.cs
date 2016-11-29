﻿using FluentAssertions;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Reflection;

namespace Tranquire.Tests
{
    public class ActorTests
    {
        public class AbilityTest
        {
            public AbilityTest AsActor(IActor actor) => this;
        }

        public class AbilityTest2
        {
            public AbilityTest2 AsActor(IActor actor) => this;
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyConstructorParameters(ConstructorInitializedMemberAssertion assertion)
        {
            assertion.Verify(typeof(Actor));
        }

        [Theory, DomainAutoData]
        public void Abilities_WhenUsingModestConstructor_ShouldBeEmpty([Modest]Actor sut)
        {
            sut.Abilities.Should().BeEmpty();
        }

        #region Can
        [Theory, DomainAutoData]
        public void Can_ShouldReturnCorrectValue(
        [Modest]Actor sut,
        AbilityTest ability)
        {
            var expected = new[] { ability };
            //act
            var actual = sut.Can(ability);
            //assert
            actual.Should().BeOfType<Actor>().Which.Abilities.Values.Should().Equal(expected);
        }

        [Theory, DomainAutoData]
        public void Can_WhenActorHasAbilities_ShouldReturnCorrectValue(
           Actor sut,
           AbilityTest ability)
        {
            //
            var expected = sut.Abilities.Values.Concat(new[] { ability }).ToArray();
            //act
            var actual = sut.Can(ability);
            //assert
            actual.Should().BeOfType<Actor>().Which.Abilities.Values.Should().Equal(expected);
        }
        #endregion
        
        #region AttemptsTo
        [Theory, DomainAutoData]
        public void AttemptsTo_ShouldCallExecuteWhen(
          Actor sut,
          Mock<IAction<object>> action,
          object expected)
        {
            action.Setup(a => a.ExecuteWhenAs(It.IsAny<IActor>())).Returns(expected);
            //act
            var actual = sut.AttemptsTo(action.Object);
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void AttemptsToWithAbility_ShouldCallExecuteWhen(
         [Greedy]Actor sut,
         Mock<IAction<Ability1, Ability2, object>> action,
         object expected)
        {
            //arrange
            var expectedAbility = sut.Abilities.Values.OfType<Ability2>().First();
            action.Setup(a => a.ExecuteWhenAs(It.IsAny<IActor>(), expectedAbility)).Returns(expected);
            //act
            var actual = sut.AttemptsTo(action.Object);
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void WasAbleTo_ShouldCallExecuteGiven(
         Actor sut,
         Mock<IAction<object>> action,
         object expected)
        {
            //arrange
            action.Setup(a => a.ExecuteGivenAs(It.IsAny<IActor>())).Returns(expected);
            //act
            var actual = sut.WasAbleTo(action.Object);
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void WasAbleToWithAbility_ShouldCallExecuteGiven(
          [Greedy]Actor sut,
          Mock<IAction<Ability1, Ability2, object>> action,
          object expected)
        {
            //arrange
            var expectedAbility = sut.Abilities.Values.OfType<Ability1>().First();
            action.Setup(a => a.ExecuteGivenAs(It.IsAny<IActor>(), expectedAbility)).Returns(expected);
            //act
            var actual = sut.WasAbleTo(action.Object);
            //assert
            Assert.Equal(expected, actual);
        }
        #endregion

        #region AsksFor
        [Theory, DomainAutoData]
        public void AsksFor_ShouldReturnCorrectValue(
           Actor sut,
           Mock<IQuestion<object>> question,
           object expected)
        {
            question.Setup(q => q.AnsweredBy(It.IsAny<IActor>())).Returns(expected);
            //act
            var actual = sut.AsksFor(question.Object);
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void AsksFor_AfterCallingWasAbleTo_ShouldReturnCorrectValue(
          [Greedy]Actor actor,
          Mock<IQuestion<object>> question,
          Mock<IGivenCommand<object>> givenCommand,
          object expected)
        {            
            question.Setup(q => q.AnsweredBy(It.IsAny<IActor>())).Returns(expected);
            givenCommand.Setup(g => g.ExecuteGivenAs(It.IsAny<IActor>()))
                        .Returns((IActor a) => a.AsksFor(question.Object));            
            //act
            var actual = actor.WasAbleTo(givenCommand.Object); 
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void AsksFor_WithAbility_ShouldReturnCorrectValue(
          Actor sut,
          Mock<IQuestion<object, Ability1>> question,
          object expected)
        {
            //arrange
            var ability = sut.Abilities.Values.OfType<Ability1>().First();
            question.Setup(q => q.AnsweredBy(It.IsAny<IActor>(), ability)).Returns(expected);
            //act
            var actual = sut.AsksFor(question.Object);
            //assert
            Assert.Equal(expected, actual);
        }
        #endregion

        [Theory, DomainAutoData]
        public void Name_WhenCallingWasAbleTo_ShouldReturnTheCorrectValue(
            [Greedy]Actor sut,
            Mock<IGivenCommand<string>> command)
        {
            //arrange
            var expected = sut.Name;
            command.Setup(a => a.ExecuteGivenAs(It.IsAny<IActor>())).Returns((IActor a) => a.Name);
            //act
            var actual = sut.WasAbleTo(command.Object);
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void Name_WhenCallingAttemptsTo_ShouldReturnTheCorrectValue(
            [Greedy]Actor sut,
            Mock<IWhenCommand<string>> command)
        {
            //arrange
            var expected = sut.Name;
            command.Setup(a => a.ExecuteWhenAs(It.IsAny<IActor>())).Returns((IActor a) => a.Name);
            //act
            var actual = sut.AttemptsTo(command.Object);
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void Name_WhenCallingWasAbleToWithAbility_ShouldReturnTheCorrectValue(
            [Greedy]Actor sut,
            Mock<IGivenCommand<Ability1, string>> command)
        {
            //arrange
            var expected = sut.Name;
            command.Setup(a => a.ExecuteGivenAs(It.IsAny<IActor>(), It.IsAny<Ability1>())).Returns((IActor a, Ability1 _) => a.Name);
            //act
            var actual = sut.WasAbleTo(command.Object);
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void Name_WhenCallingAttemptsToWithAbility_ShouldReturnTheCorrectValue(
            [Greedy]Actor sut,
            Mock<IWhenCommand<Ability1, string>> command)
        {
            //arrange
            var expected = sut.Name;
            command.Setup(a => a.ExecuteWhenAs(It.IsAny<IActor>(), It.IsAny<Ability1>())).Returns((IActor a, Ability1 _) => a.Name);
            //act
            var actual = sut.AttemptsTo(command.Object);
            //assert
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> ActionsWithAbility
        {
            get
            {
                return new System.Action<Actor, IFixture>[]
                {
                    (sut, fixture) => sut.AsksFor(fixture.Create<IQuestion<string, AbilityTest>>()),
                    (sut, fixture) => sut.AttemptsTo(fixture.Create<IWhenCommand<AbilityTest, object>>()),
                    (sut, fixture) => sut.WasAbleTo(fixture.Create<IGivenCommand<AbilityTest, object>>()),
                    //(sut, fixture) => sut.Execute(fixture.Create<IAction<AbilityTest, AbilityTest, object>>()),
                }
                .Select(a => new object[] { a });
            }
        }

        [Theory, MemberData("ActionsWithAbility")]
        public void ActionWithAbility_WhenAbilityIsNotRegistered_ShouldThrow(System.Action<Actor, IFixture> action)
        {
            //arrange
            var fixture = new Fixture().Customize(new ActorCustomization()).Customize(new AutoConfiguredMoqCustomization());
            var sut = fixture.Create<Actor>();
            System.Action testedAction = () => action(sut, fixture);
            //act and assert
            testedAction.ShouldThrow<InvalidOperationException>().Where(ex => ex.Message.Contains(typeof(AbilityTest).Name));
        }
    }
}
