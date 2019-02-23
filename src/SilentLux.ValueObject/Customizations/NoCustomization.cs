using System.Collections.Generic;
using System.Linq;

namespace SilentLux.ValueObject.Customizations
{
    public class NoCustomization : IEqualityCustomization
    {
        public virtual bool CheckEquality(
            ValueObject first,
            ValueObject second
        ) =>
            true;

        public virtual IEnumerable<object> GetHashCodeComponents() =>
            Enumerable.Empty<object>();
    }
}