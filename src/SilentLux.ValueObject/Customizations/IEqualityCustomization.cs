using System.Collections.Generic;

namespace SilentLux.ValueObject.Customizations
{
    public interface IEqualityCustomization
    {
        bool CheckEquality(ValueObject first, ValueObject second);
        IEnumerable<object> GetHashCodeComponents();
    }
}