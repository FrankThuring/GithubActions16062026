using FluentAssertions;
using MedTrack;
using Xunit;

namespace MedTrack.Tests;

/// <summary>
/// Seed tests so the repo is green from the start. In Lab 5 participants
/// generate more tests with Copilot (e.g. for DailySchedule and IsDoseDue
/// edge cases) and review the assertions before trusting them.
/// </summary>
public class DosingCalculatorTests
{
    private readonly DosingCalculator _calc = new();

    [Fact]
    public void NextDose_AddsTheInterval()
    {
        var last = new DateTime(2026, 6, 3, 8, 0, 0, DateTimeKind.Utc);

        var next = _calc.NextDose(last, TimeSpan.FromHours(6));

        next.Should().Be(new DateTime(2026, 6, 3, 14, 0, 0, DateTimeKind.Utc));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void NextDose_Throws_WhenIntervalIsNotPositive(int hours)
    {
        var last = new DateTime(2026, 6, 3, 8, 0, 0, DateTimeKind.Utc);

        var act = () => _calc.NextDose(last, TimeSpan.FromHours(hours));

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void DailySchedule_ProducesEvenlySpacedDoses()
    {
        var first = new DateTime(2026, 6, 3, 8, 0, 0, DateTimeKind.Utc);

        var schedule = _calc.DailySchedule(first, TimeSpan.FromHours(8), 3);

        schedule.Should().HaveCount(3);
        schedule.Should().ContainInConsecutiveOrder(
            new DateTime(2026, 6, 3, 8, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 6, 3, 16, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 6, 4, 0, 0, 0, DateTimeKind.Utc));
    }

    [Fact]
    public void IsDoseDue_IsTrue_AtOrAfterNextDose()
    {
        var last = new DateTime(2026, 6, 3, 8, 0, 0, DateTimeKind.Utc);

        _calc.IsDoseDue(last, TimeSpan.FromHours(6),
            new DateTime(2026, 6, 3, 14, 0, 0, DateTimeKind.Utc)).Should().BeTrue();
        _calc.IsDoseDue(last, TimeSpan.FromHours(6),
            new DateTime(2026, 6, 3, 13, 59, 0, DateTimeKind.Utc)).Should().BeFalse();
    }
}
