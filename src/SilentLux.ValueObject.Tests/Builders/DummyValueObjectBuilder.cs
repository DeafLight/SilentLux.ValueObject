using AutoFixture;
using SilentLux.AutoFixture.Builders;
using SilentLux.ValueObject.Customizations;
using SilentLux.ValueObject.Tests.Artifacts;

namespace SilentLux.ValueObject.Tests.Builders
{
    public class DummyValueObjectBuilder :
        Builder<DummyValueObjectBuilder, DummyValueObject>
    {
        private IEqualityCustomization _equalityCustomization;
        private int _int;
        private bool _skipEqualityCustomizationParameter;
        private string _string;

        public DummyValueObjectBuilder(IFixture fixture) :
            base(fixture)
        {
            _int = fixture.Create<int>();
            _string = fixture.Create<string>();
            _equalityCustomization = fixture.Create<IEqualityCustomization>();
        }

        public DummyValueObjectBuilder WithEqualityCustomization(
            IEqualityCustomization equalityCustomization
        )
        {
            _skipEqualityCustomizationParameter = false;
            return With(() => _equalityCustomization = equalityCustomization);
        }

        public DummyValueObjectBuilder SkipEqualityCustomizationParameter()
        {
            _skipEqualityCustomizationParameter = true;
            return this;
        }

        public DummyValueObjectBuilder WithInt(int @int)
        {
            return With(() => _int = @int);
        }

        public DummyValueObjectBuilder WithString(string @string) =>
            With(() => _string = @string);

        protected override DummyValueObject NewInstance() =>
            _skipEqualityCustomizationParameter
                ? new DummyValueObject(_int, _string)
                : new DummyValueObject(_equalityCustomization, _int, _string);
    }
}