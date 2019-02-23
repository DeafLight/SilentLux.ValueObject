using SilentLux.ValueObject.Customizations;
using System;
using System.Collections.Generic;

namespace SilentLux.ValueObject.Tests.Artifacts
{
    public class DummyValueObject :
        ValueObject
    {
        public DummyValueObject(int i, string s) =>
            Init(i, s);

        public DummyValueObject(
            IEqualityCustomization customization,
            int i,
            string s
        ) :
            base(customization) =>
                Init(i, s);

        public int Int { get; private set; }
        public string String { get; private set; }

        private void Init(int i, string s)
        {
            Int = i;
            String = s ?? throw new ArgumentNullException(nameof(s));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Int;
            yield return String;
        }
    }
}