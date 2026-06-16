namespace MedTrack;

/// <summary>
/// Calculates medication dosing schedules.
/// </summary>
/// <remarks>
/// Intentionally small and well-documented so it is a good GitHub Copilot
/// playground: explaining code, generating xUnit tests, and refactoring.
/// All times are treated as UTC. This is sample/synthetic logic for training —
/// it is NOT clinical software.
/// </remarks>
public sealed class DosingCalculator
{
    /// <summary>
    /// Calculates the time of the next dose.
    /// </summary>
    /// <param name="lastDoseUtc">UTC timestamp of the last dose taken.</param>
    /// <param name="interval">Time between doses. Must be positive.</param>
    /// <returns>UTC timestamp of the next scheduled dose.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="interval"/> is zero or negative.
    /// </exception>
    public DateTime NextDose(DateTime lastDoseUtc, TimeSpan interval)
    {
        if (interval <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(interval), "Dosing interval must be positive.");
        }

        return lastDoseUtc.ToUniversalTime() + interval;
    }

    /// <summary>
    /// Builds a schedule of dose times for a full day starting at
    /// <paramref name="firstDoseUtc"/>.
    /// </summary>
    /// <param name="firstDoseUtc">UTC time of the first dose.</param>
    /// <param name="interval">Time between doses. Must be positive.</param>
    /// <param name="dosesPerDay">How many doses to schedule. Must be 1 or more.</param>
    /// <returns>The scheduled dose times, in chronological order.</returns>
    public IReadOnlyList<DateTime> DailySchedule(
        DateTime firstDoseUtc, TimeSpan interval, int dosesPerDay)
    {
        if (interval <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(interval), "Dosing interval must be positive.");
        }
        if (dosesPerDay < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(dosesPerDay), "There must be at least one dose per day.");
        }

        var schedule = new List<DateTime>(dosesPerDay);
        var current = firstDoseUtc.ToUniversalTime();
        for (var i = 0; i < dosesPerDay; i++)
        {
            schedule.Add(current);
            current += interval;
        }
        return schedule;
    }

    /// <summary>
    /// Returns true if a dose is currently due, i.e. <paramref name="nowUtc"/>
    /// is at or after the next dose time derived from the last dose.
    /// </summary>
    public bool IsDoseDue(DateTime lastDoseUtc, TimeSpan interval, DateTime nowUtc)
        => nowUtc.ToUniversalTime() >= NextDose(lastDoseUtc, interval);
}
