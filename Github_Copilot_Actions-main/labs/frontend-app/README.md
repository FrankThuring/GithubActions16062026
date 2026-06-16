# frontend-app (TypeScript track)

A small, strictly-typed TypeScript project used in the Copilot & Actions labs. It
represents the kind of frontend/domain code in the frontend track, but stays
lightweight so it builds and tests reliably on a CI runner.

## Run locally

```bash
npm ci          # install deps (use `npm install` the first time, no lockfile yet)
npm run lint    # type-check only (tsc --noEmit)
npm test        # run the vitest unit tests
npm run build   # compile to dist/
```

## Files

- `src/invoices.ts` — invoice domain helpers (overdue, totals, formatting).
- `src/invoices.test.ts` — seed tests (kept green). You add more in **Lab 5**.

## How it's used in the labs

| Lab | What you do here |
|-----|------------------|
| 2 | Ask Copilot to `/explain` `invoices.ts` and answer questions about it |
| 3 | Improve prompts to generate docs + a new helper that matches house style |
| 4 | Refactor `daysBetween`/`overdueInvoices` tests-first |
| 5 | Generate tests for `formatEuros` and missing edge cases; review assertions |
| 8 | The `frontend-ci.yml` workflow runs `lint` + `test` + `build` on push/PR |
| 12 | Capstone: add a feature + tests, push, get CI green |

> Note: this uses `vitest` + `tsc` instead of a full Angular/Karma setup so the
> labs are fast and dependency-light. The Copilot and Actions concepts map 1:1 to
> a real Angular project — only the test runner command changes.

