using System;
using Application.Enums;
using Infrastructure.Extentions;
using Shouldly;
using Xunit;

namespace UnitTests.Common.Extensions
{
    public class AgeTests
    {
 
        [Theory]
        [InlineData("01/01/1990")]
        [InlineData("01/01/2020 ")]
        [InlineData("01/06/2020")]
        public void Age_ShouldSupportCalculatingAge(string dateString)
        {
            //Arrange
            var date = DateTime.Parse(dateString);

            //Act
            (var age, var ageUnits) = date.Age();

            //Assert
            age.GetType().ShouldBe(typeof(int));
            ageUnits.GetType().ShouldBe(typeof(AgeUnits));
        }


        [Fact]
        public void Age_WhenDateGreaterThenNow_Return_Exseption()
        {
            //Arrange
            var date = DateTime.Now.AddDays(5);

            //Act
            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => date.Age());
        }

        [Theory]
        [InlineData(AgeUnits.Second)]
        [InlineData(AgeUnits.Minute)]
        [InlineData(AgeUnits.Hour)]
        [InlineData(AgeUnits.Day)]
        [InlineData(AgeUnits.Week)]
        [InlineData(AgeUnits.Month)]
        [InlineData(AgeUnits.Year)]
        public void AgeWithUnits_ShouldSupportCalculatingAgeInSpecificUnits(AgeUnits units)
        {
            //Arrange
            var date = DateTime.Now;

            //Act
            var age = date.Age(units);

            //Assert
            age.GetType().ShouldBe(typeof(int));
        }

        [Fact]
        public void AgeWithUnits_WhenDateGreaterThenNow_Return_Exseption()
        {
            //Arrange
            var date = DateTime.Now.AddDays(5);
            var units = AgeUnits.Day;

            //Act
            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => date.Age(units));
        }


    }
}