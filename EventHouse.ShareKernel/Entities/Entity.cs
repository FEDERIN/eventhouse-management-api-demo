namespace EventHouse.ShareKernel.Entities
{
    public abstract class Entity : IEquatable<Entity>
    {
        public Guid Id { get; init; }

        public bool Equals(Entity? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;

            return Id == other.Id;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals((Entity)obj);
        }

        public static bool operator ==(Entity? left, Entity? right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;

            return left.Equals(right);
        }

        public static bool operator !=(Entity? left, Entity? right)
            => !(left == right);

        public override int GetHashCode()
            => Id.GetHashCode();
    }

}
