namespace Neuro.Domain.Entities;

#region Entity of TPrimaryKey

public interface IEntity
{
}

public interface IEntity<TPrimaryKey> : IEntity
{
    TPrimaryKey Id { get; }
}

public abstract class BaseEntity<TPrimaryKey> : IEntity<TPrimaryKey>
{
    #region Properties

    public virtual TPrimaryKey Id { get; set; }

    #endregion

    #region Methods

    public override string ToString()
    {
        return $"[{GetType().Name} - {Id}]";
    }

    #endregion

    #region Equality

    public override bool Equals(object obj)
    {
        var other = obj as BaseEntity<TPrimaryKey>;
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        var comparer = EqualityComparer<TPrimaryKey>.Default;
        if (comparer.Equals(Id, default) || comparer.Equals(other.Id, default))
            return false;

        return comparer.Equals(Id, other.Id);
    }

    public static bool operator ==(BaseEntity<TPrimaryKey> a, BaseEntity<TPrimaryKey> b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(BaseEntity<TPrimaryKey> a, BaseEntity<TPrimaryKey> b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return (GetType().ToString() + Id).GetHashCode();
    }

    #endregion
}

#endregion

#region Soft Deletable Entity

public interface ISoftDeletableEntity
{
    bool IsDeleted { get; set; }
}

#endregion

#region Audited Entity

public interface IAuditedEntity<TPrimaryKey, TUserPrimaryKey> : IEntity<TPrimaryKey>
{
    TUserPrimaryKey CreatedBy { get; set; }
    DateTimeOffset CreatedAt { get; set; }
    TUserPrimaryKey ModifiedBy { get; set; }
    DateTimeOffset ModifiedAt { get; set; }
    byte[] Timestamp { get; set; }
}

public abstract class AuditedBaseEntity<TPrimaryKey, TUserPrimaryKey> : BaseEntity<TPrimaryKey>, IAuditedEntity<TPrimaryKey,TUserPrimaryKey>
{
    public TUserPrimaryKey CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public TUserPrimaryKey ModifiedBy { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
    public byte[] Timestamp { get; set; }
    
      protected AuditedBaseEntity()
     {
         CreatedAt = DateTimeOffset.UtcNow;
     }
}

#endregion