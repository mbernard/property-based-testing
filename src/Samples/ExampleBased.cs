using System;
using Xunit;

namespace Samples
{
    public class ExampleBased
    {
        [Fact]
        public void Given_When_Then()
        {
            // Given
            var input = 3; 

            // When
            var result = Calculator.Calculate(input);

            // Then
            Assert.Equal(3, result);
        }
    }

    public class Calculator
    {
        public static int Calculate(in int input)
        {
            return input;
        }
    }
}
