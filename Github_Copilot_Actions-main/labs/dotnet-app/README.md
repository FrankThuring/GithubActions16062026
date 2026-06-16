# dotnet-app (.NET track)

A small .NET 8 solution used in the Copilot & Actions labs. The domain
(medication dosing) mirrors a real medication-software domain, but the logic is **synthetic
sample code for training — not clinical software**.

## Solution layout

```
MedTrack.sln
├─ src/MedTrack/                 class library (DosingCalculator)
└─ tests/MedTrack.Tests/         xUnit + FluentAssertions tests
```

## Run locally

```bash
dotnet restore
dotnet build -c Release
dotnet test  -c Release
```

## How it's used in the labs

| Lab | What you do here |
|-----|------------------|
| 2 | `/explain` `DosingCalculator.cs`; ask about edge cases |
| 3 | Generate XML doc comments + improve prompts |
| 4 | Refactor `DailySchedule` tests-first; (bonus) translate a Business Central snippet to C# |
| 5 | Generate xUnit tests for `IsDoseDue`/`DailySchedule` edge cases; review assertions |
| 8 | `dotnet-ci.yml` runs restore + build + test on push/PR |
| 12 | Capstone: add a feature + tests with Copilot, push, get CI green |

> Uses xUnit + FluentAssertions, the common .NET test stack. Same commands run
> locally and in GitHub Actions.
