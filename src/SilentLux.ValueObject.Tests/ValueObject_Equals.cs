using FluentAssertions;
using Moq;
using SilentLux.AutoFixture.Moq.Xunit2;
using SilentLux.ValueObject.Customizations;
using SilentLux.ValueObject.Tests.Artifacts;
using SilentLux.ValueObject.Tests.Builders;
using Xunit;

namespace SilentLux.ValueObject.Tests
{
    public class ValueObject_Equals
    {
        [Theory]
        [AutoMoqData]
        public void Returns_false_when_other_is_null(DummyValueObject sut)
        {
            sut.Equals(null).Should().BeFalse();
        }

        [Theory]
        [AutoMoqData]
        public void Returns_true_if_other_is_the_same_object(
            DummyValueObject sut
        )
        {
            var other = sut;

            sut.Equals(other).Should().BeTrue();
        }

        public class WithoutEqualityCustomization
        {
            [Theory]
            [AutoMoqData]
            public void Returns_true_when_other_has_the_same_values(
                DummyValueObjectBuilder sutBuilder
            )
            {
                sutBuilder.SkipEqualityCustomizationParameter();

                var sut = sutBuilder.Build();
                var other = sutBuilder.Build();

                sut.Equals(other).Should().BeTrue();
            }

            [Theory]
            [AutoMoqData]
            public void Returns_false_when_other_has_different_values(
                DummyValueObjectBuilder sutBuilder,
                DummyValueObjectBuilder otherBuilder
            )
            {
                var sut = sutBuilder
                    .SkipEqualityCustomizationParameter()
                    .Build();
                var other = otherBuilder
                    .SkipEqualityCustomizationParameter()
                    .Build();

                sut.Equals(other).Should().BeFalse();
            }
        }

        public class WithEqualityCustomization
        {
            [Theory]
            [AutoMoqData]
            public void Calls_customization_when_values_are_equal(
                Mock<IEqualityCustomization> equalityCustomization,
                DummyValueObjectBuilder sutBuilder
            )
            {
                sutBuilder
                    .WithEqualityCustomization(equalityCustomization.Object);

                var sut = sutBuilder.Build();
                var other = sutBuilder.Build();

                equalityCustomization.Setup(x =>
                        x.CheckEquality(sut, other))
                    .Verifiable();

                var res = sut.Equals(other);

                equalityCustomization.Verify();
            }

            [Theory]
            [InlineAutoMoqData(true)]
            [InlineAutoMoqData(false)]
            public void Returns_the_customization_output_when_values_are_equals(
                bool customizationOutput,
                Mock<IEqualityCustomization> equalityCustomization,
                DummyValueObjectBuilder sutBuilder,
                DummyValueObjectBuilder otherBuilder
            )
            {
                sutBuilder
                    .WithEqualityCustomization(equalityCustomization.Object);

                var sut = sutBuilder.Build();
                var other = otherBuilder
                    .WithInt(sut.Int)
                    .WithString(sut.String)
                    .WithEqualityCustomization(equalityCustomization.Object)
                    .Build();

                equalityCustomization.Setup(x =>
                        x.CheckEquality(sut, other))
                    .Returns(customizationOutput);

                sut.Equals(other).Should().Be(customizationOutput);
            }
        }
    }
}
