# GitHub Actions — cheat sheet

A one-page reference to hand out. Print double-sided with the Copilot cheat sheet.

---

## The vocabulary

```
Workflow   (.github/workflows/*.yml) — triggered by an event
 └─ Job    runs on one runner (VM); jobs run in PARALLEL by default
     └─ Step   a single task
         ├─ run:   a shell command
         └─ uses:  a reusable Action
```

- Jobs are **isolated** (fresh runner, no shared disk) → use **artifacts** to pass files.
- Steps are **sequential** and **share** the job's filesystem.
- `${{ ... }}` = expressions (contexts like `github.ref`, `secrets.X`, functions).

---

## Minimal workflow

```yaml
name: CI
on: [push, pull_request]
jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - run: echo "hello"
```

## Triggers (`on:`)

```yaml
on:
  push: { branches: [main], paths: ['src/**'] }
  pull_request: { branches: [main] }
  workflow_dispatch: {}          # manual button (+ optional inputs)
  schedule: [{ cron: '0 2 * * *' }]   # UTC; default branch only
  release: { types: [published] }
```

## Jobs, ordering, runners

```yaml
jobs:
  build: { runs-on: ubuntu-latest, steps: [...] }
  test:
    needs: build                 # wait for build
    runs-on: windows-latest      # or macos-latest
    steps: [...]
```

## Steps: `uses` vs `run`

```yaml
steps:
  - uses: actions/setup-node@v4
    with: { node-version: 20, cache: 'npm' }
  - name: Test
    run: npm ci && npm test
    env: { NODE_ENV: test }
    if: success()
```

---

## Common actions

| Action | Purpose |
|--------|---------|
| `actions/checkout@v4` | clone the repo |
| `actions/setup-node@v4` / `setup-dotnet@v4` | install toolchain (+ cache) |
| `actions/cache@v4` | cache deps/build output |
| `actions/upload-artifact@v4` / `download-artifact@v4` | move files between jobs |
| `actions/github-script@v7` | run JS against the GitHub API |

## Matrix (parallel across versions)

```yaml
strategy:
  fail-fast: false
  matrix: { node-version: [18, 20, 22] }
runs-on: ubuntu-latest
steps:
  - uses: actions/setup-node@v4
    with: { node-version: ${{ matrix.node-version }} }
```

## Conditionals & status functions

```yaml
if: github.ref == 'refs/heads/main'
# functions: success() failure() always() cancelled()
- if: failure()
  run: echo "something broke"
```

## Secrets & environments

```yaml
permissions: { contents: read }   # least privilege for GITHUB_TOKEN
jobs:
  deploy:
    if: github.ref == 'refs/heads/main'
    environment: production         # approval gate + its own secrets
    steps:
      - run: ./deploy.sh
        env: { API_KEY: ${{ secrets.API_KEY }} }   # masked in logs
```

- Secrets: **Settings → Secrets and variables → Actions** (repo/env/org).
- Fork PRs don't get your secrets by default. Never `echo` a secret.

---

## Custom actions (3 types)

| Type | `runs.using` | When |
|------|--------------|------|
| Composite | `composite` | bundle existing steps (each `run` needs `shell:`) |
| JavaScript | `node20` | logic, GitHub API, fast start |
| Docker | `docker` | need a specific runtime/toolchain |

`action.yml` defines `inputs`, `outputs`, and `runs`. Reference by path
(`./.github/actions/x`) or by `org/action@v1`.

---

## YAML gotchas

- **Spaces, not tabs.** Indentation is structure.
- `|` keeps newlines, `>` folds to spaces.
- Quote strings containing `:`, `{{ }}`, or special chars.
- 90% of parse errors = a tab or bad indent. Let Copilot draft and explain YAML.

## Debugging a run

- Open the **Actions** tab → the run → expand step logs.
- Re-run failed jobs from the UI.
- Upload artifacts on `if: always()` to inspect results after failure.
- Add `echo "::debug::msg"` / `::warning::` / `::error::` annotations.
