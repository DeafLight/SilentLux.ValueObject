using SilentLux.ValueObject.Customizations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SilentLux.ValueObject
{
    /// <summary>
    ///     When overridden, provides a value object with value equality and
    ///     hashcode.
    ///     Based on: https://enterprisecraftsmanship.com/2017/08/28/value-object-a-better-implementation/
    /// </summary>
    public abstract class ValueObject
    {
        private const int HashCodeSeed = 17;
        private const int HashCodeMultiplier = 31;

        private readonly IEqualityCustomization _customization;

        protected ValueObject(IEqualityCustomization customization)
        {
            _customization = customization ??
                throw new ArgumentNullException(nameof(customization));
        }

        protected ValueObject() :
            this(new NoCustomization())
        {
        }

        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object obj)
        {
            var valueObject = obj as ValueObject;

            return EqualsInternal(valueObject);
        }

        protected bool EqualsInternal(ValueObject obj)
        {
            if (obj == null) return false;

            if (ReferenceEquals(this, obj)) return true;

            if (GetType() != obj.GetType()) return false;

            return GetEqualityComponents()
                       .SequenceEqual(
                           obj.GetEqualityComponents()) &&
                   _customization.CheckEquality(this, obj);
        }

        public override int GetHashCode() =>
            GetEqualityComponents()
                .Union(_customization.GetHashCodeComponents())
                .Aggregate(HashCodeSeed, (current, obj) =>
                {
                    unchecked
                    {
                        return current * HashCodeMultiplier +
                               (obj?.GetHashCode() ?? 0);
                    }
                });

        public static bool operator ==(ValueObject a, ValueObject b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(ValueObject a, ValueObject b) =>
            !(a == b);
    }
}