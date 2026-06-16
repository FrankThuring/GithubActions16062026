# Lab guide — GitHub Copilot & GitHub Actions

Twelve hands-on labs. **Labs 1–6** are Day 1 (Copilot); **Labs 7–12** are Day 2
(Actions). Pick the track that matches your stack — `frontend-app/` (TypeScript)
or `dotnet-app/` (.NET). The skills are identical; only the commands differ.

> **Safety reminder (regulated work):** use only the synthetic sample data in
> these labs. Never paste real PHI, PII, secrets, or proprietary code into a prompt.

**Conventions in this guide**
- 💬 = type this into **Copilot Chat**
- ⌨️ = a terminal command
- ✅ = the check that tells you it worked

---

## Day 1 — GitHub Copilot

### Lab 0 — Warm-up (10 min)

1. Open the `labs/` folder in VS Code.
2. Confirm the Copilot status-bar icon is active (signed in).
3. Frontend track: ⌨️ `cd labs/frontend-app && npm install && npm test` → ✅ 5 tests pass.
4. .NET track: ⌨️ `cd labs/dotnet-app && dotnet test` → ✅ tests pass.

> If sign-in fails behind a proxy, pair with a neighbour for the morning and sort it at the break.

---

### Lab 1 — Completions: let context do the work (15 min)
**Goal:** feel how context quality changes suggestions.

1. Open `frontend-app/src/invoices.ts` (or `dotnet-app/.../DosingCalculator.cs`).
2. At the bottom, type a descriptive comment + signature and let ghost text complete:
   - TS: `/** Returns the single most overdue unpaid invoice, or null. */`
     then `export function mostOverdue(invoices: Invoice[], asOf: string): Invoice | null {`
   - C#: `/// <summary>Returns the number of doses between two times.</summary>`
     then `public int DoseCount(DateTime fromUtc, DateTime toUtc, TimeSpan interval)`
3. Accept with `Tab`. Cycle alternatives with `Alt+]` / `Alt+[`.
4. **Now remove the types** from your signature and retype. Watch suggestions get worse.

✅ **Takeaway:** types + names + a clear comment = the prompt. *Context is the lever.*

> Don't keep this throwaway function — delete it before the next lab (or keep it and add a test in Lab 5).

---

### Lab 2 — Understand code with Chat (20 min)
**Goal:** use Copilot as a *reading* tool.

1. Select the whole domain file → 💬 `/explain`.
2. Ask focused questions:
   - 💬 `What edge cases does daysBetween fail to handle?`
   - 💬 `@workspace Where is currency formatting done, and is it consistent?`
3. Break something on purpose (e.g. change `> days` to `>= days`), run tests to see red, then:
   - 💬 select the failing code → `/fix` — read its explanation before accepting.
4. Undo your break.

✅ **Takeaway:** `/explain`, `@workspace`, and `/fix` turn unfamiliar code into a conversation. (Great for inherited frontends and the BC→.NET migration.)

---

### Lab 3 — Prompt craft: code, docs & house style (25 min)
**Goal:** better prompts for code, docs, and tests. Everyone benefits.

1. **Add a project instructions file.** Create `labs/.github/copilot-instructions.md`:
   ```markdown
   # Conventions
   - TypeScript strict; no `any`. Money is integer cents.
   - C#: nullable enabled; UTC everywhere; XML docs on public members.
   - Tests: vitest (TS) / xUnit + FluentAssertions (.NET). Cover null + boundaries.
   ```
2. Ask for a new helper **matching the existing style**:
   - 💬 `#file:invoices.ts Add a function "agingBuckets" that groups unpaid invoices into 0-30, 31-60, 60+ day buckets as of a date. Match the style of this file. Include JSDoc.`
3. Compare a **vague** vs **specific** prompt for docs:
   - Vague: 💬 `document this`
   - Specific: 💬 `/doc Write JSDoc for totalOutstandingCents: one-line summary, @param, @returns, and a note that amounts are in cents.`
4. Generate a PR-style summary: 💬 `Summarize the changes I've made in this file as a changelog entry.`

✅ **Takeaway:** specific prompts + an instructions file = consistent, on-style output. Promote good prompts into the repo.

---

### Lab 4 — Refactor, tests-first (25 min)
**Goal:** the safe refactoring sequence.

1. Pick a function with room to improve (`overdueInvoices` / `DailySchedule`).
2. **Tests first:** select it → 💬 `/tests cover the happy path, boundaries, and the empty/zero cases`. Save them and run — they should pass (characterisation).
3. **Refactor:** 💬 `Refactor this into smaller, well-named pure functions. Preserve behaviour. Explain each change.`
4. Re-run tests → ✅ still green = safe refactor.
5. **Multi-file:** open Copilot **Edits**, ask to `rename amountCents-related helpers consistently across the module` and review the per-file diff.

**Bonus (.NET track):** paste a short Business Central / AL snippet → 💬 `This is Business Central AL. Produce an idiomatic C# equivalent using DI and async. List any assumptions you made.` → discuss the assumptions.

✅ **Takeaway:** tests *before* refactor. Copilot makes the safety net cheap.

---

### Lab 5 — Generate unit tests (25 min)
**Goal:** test generation you can trust.

1. Find an under-tested function (`formatEuros` in TS; `IsDoseDue`/`DailySchedule` in C#).
2. 💬 `/tests` with a precise spec:
   - TS: `vitest tests for formatEuros: zero, negative, large amounts, and that it uses euro formatting.`
   - C#: `xUnit + FluentAssertions for DailySchedule: throws on interval<=0 and dosesPerDay<1; spacing is correct for 4 doses.`
3. Ask the killer question: 💬 `What test cases am I still missing?`
4. **Review every assertion.** Deliberately find one that's weak or wrong and fix it.
5. Run the suite → ✅ green.

✅ **Takeaway:** generated tests are a *reviewed draft*. A test asserting the wrong thing is worse than none. These fast tests feed Day 2's CI.

---

### Lab 6 — Agent mode: build a slice (25 min)
**Goal:** supervised autonomy.

1. ⌨️ Create a branch: `git checkout -b feature/agent-slice` (init git first if needed — see labs/README).
2. In Copilot **agent mode**, give a goal:
   - TS: `Add a "paidRatio(invoices)" function returning the share of paid invoices (0–1), with JSDoc and vitest tests. Run the tests.`
   - C#: `Add a "DosesRemaining(schedule, nowUtc)" method with XML docs and xUnit tests. Run dotnet test.`
3. **Approve actions deliberately.** Watch it create files, edit, and run tests; let it self-correct on failures.
4. **Read every diff** before accepting. Commit when green.

✅ **Takeaway:** agent mode = a fast junior pair. Branch first, give it tests to check itself, review the diffs. (For medical software: this discipline is non-negotiable.)

> **End of Day 1:** export `slides/day1-github-copilot.md` to PDF and email to `trainer@eduvision.nl` (see root README).

---

## Day 2 — GitHub Actions

> **Prereq:** push the labs to a GitHub repo so workflows run. See "Getting the
> labs onto GitHub" in [`labs/README.md`](README.md). If you push `labs/` as the
> repo root, move `.github/` to the repo root so GitHub detects the workflows.

### Lab 7 — Your first workflow (15 min)
**Goal:** the green-check dopamine hit.

1. Create `.github/workflows/hello.yml`:
   ```yaml
   name: Hello
   on: [push, workflow_dispatch]
   jobs:
     greet:
       runs-on: ubuntu-latest
       steps:
         - uses: actions/checkout@v4
         - run: echo "Hello from $GITHUB_ACTOR on ${{ github.ref }}"
   ```
2. Commit, push, open the repo's **Actions** tab, watch it run.
3. Use the **Run workflow** button (from `workflow_dispatch`).

✅ **Takeaway:** name + `on:` + `jobs:` = a workflow. The Actions tab is your feedback loop.

---

### Lab 8 — Real CI: build + test your app (25 min)
**Goal:** use case A — tests run automatically.

1. The repo already ships `dotnet-ci.yml` and `frontend-ci.yml` under `.github/workflows/`.
2. Make a trivial change in your app and push to a branch; open a **pull request**.
3. Watch CI run on the PR. Open the logs; expand each step.
4. 💬 (Copilot) `Explain this workflow file line by line` on your CI yml.

✅ **Takeaway:** every push/PR now builds and tests. Tests that don't run automatically rot.

---

### Lab 9 — Marketplace actions & artifacts (20 min)
**Goal:** reuse prebuilt actions; pass files between jobs.

1. Browse the **Marketplace** (Actions tab → search). Inspect `actions/setup-node` inputs.
2. Confirm your CI uploads an artifact (`upload-artifact`); download it from the run summary.
3. Add a second job that `needs:` the first and downloads the artifact:
   ```yaml
   inspect:
     needs: build-test
     runs-on: ubuntu-latest
     steps:
       - uses: actions/download-artifact@v4
         with: { name: frontend-dist }
       - run: ls -la
   ```
4. **Security:** change a `@v4` to a pinned commit SHA for one action; note why (supply-chain).

✅ **Takeaway:** jobs are isolated → artifacts move files between them. Pin third-party actions.

---

### Lab 10 — Custom actions (25 min)
**Goal:** stay DRY; understand the action contract.

1. **Composite:** replace the install/lint/test steps in `frontend-ci.yml` with
   `- uses: ./labs/.github/actions/setup-and-test-frontend`. Push; confirm it still works.
2. **JavaScript action:** add a job that uses `./labs/.github/actions/changelog-check`
   (remember `fetch-depth: 0` on checkout). Open a PR *without* touching `CHANGELOG.md`
   → it fails. Add a changelog entry → it passes.
3. 💬 `Rewrite changelog-check/index.js using @actions/core and @actions/exec, same inputs/output.`

✅ **Takeaway:** composite = extract-method for YAML; JS actions = logic with an `inputs/outputs` contract.

---

### Lab 11 — Advanced: matrix, conditionals, secrets (25 min)
**Goal:** the building blocks of real pipelines.

1. **Matrix:** `frontend-ci.yml` already tests Node 18/20/22 with `fail-fast: false`.
   Break a test on one version only (via a version check) and watch which jobs fail.
2. **Conditional + environment:** look at `deploy.yml`. Add a `production` **Environment**
   in repo Settings with yourself as a **required reviewer**.
3. **Secret:** add a dummy `DEPLOY_TOKEN` secret (Settings → Secrets and variables → Actions).
4. Push to `main` (or run via dispatch) → the deploy job **waits for your approval**; the
   secret shows as `***` in logs.

✅ **Takeaway:** matrix = parallel coverage; `if:` = controlled execution; environments + secrets = the audit-friendly deploy story.

---

### Lab 12 — Capstone: Copilot → CI → green (60–75 min)
**Goal:** the whole training in one loop.

1. ⌨️ `git checkout -b feature/discount`.
2. **Copilot writes the feature + tests:**
   - TS: 💬 (agent) `Add applyDiscount(invoice, percent) returning a new invoice with amountCents reduced by percent (0–100), rounding to the nearest cent. Reject out-of-range percent. Add vitest tests. Run them.`
   - C#: 💬 (agent) `Add AdjustDose(dose, factor) to DosingCalculator scaling an interval by a positive factor, throwing on factor<=0. Add xUnit tests. Run dotnet test.`
3. **Introduce a realistic bug on purpose** (e.g. forget the upper-bound check), commit, push, open a **PR**.
4. Watch **CI go red**. Open the failing test log.
5. 💬 `/fix` (or paste the failure) → let Copilot diagnose; **review** the fix; push again.
6. CI goes **green** ✅.
7. **Branch protection (tie-off):** Settings → Branches → require the CI status check
   before merging. Show that a red build now *blocks* merge.
8. Merge → (bonus) the gated `deploy.yml` runs and waits for approval.

✅ **Takeaway — the thesis of both days:** *Copilot raises the rate of change; Actions keeps that change safe.* Faster **and** steadier.

> **End of Day 2:** export `slides/day2-github-actions.md` to PDF and email to `trainer@eduvision.nl`.

---

## If you finish early — stretch goals

- Add **ESLint** to the frontend and gate the PR on it.
- Add **code coverage** reporting to CI and fail under a threshold.
- Convert `dotnet-ci.yml` to a **reusable workflow** (`on: workflow_call`).
- Add a **Slack/Teams notification** action on failure.
- Wire an **MCP server** into Copilot agent mode and try a task that uses it.
- Write a `copilot-instructions.md` for one of *your own* repos and feel the difference.
