using SimpleBlog.Domain.Utils;
using System;

namespace SimpleBlog.Domain.Entities.Abstract;
public abstract class BaseEntity
{
    public DateTime CreatedAt { get; set; } = LocalClock.GetTime();
    public DateTime? ModifiedAt { get; set; } = LocalClock.GetTime();
}