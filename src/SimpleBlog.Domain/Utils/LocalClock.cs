using System;

namespace SimpleBlog.Domain.Utils;
public static class LocalClock
{
    public static Func<DateTime> GetTime { get; } = () => DateTime.UtcNow;
}