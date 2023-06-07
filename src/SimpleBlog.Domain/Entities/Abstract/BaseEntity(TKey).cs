using System;

namespace SimpleBlog.Domain.Entities.Abstract;
public abstract class BaseEntity<TKey> : BaseEntity where TKey : IEquatable<TKey>
{
    public virtual TKey Id { get; set; }
}