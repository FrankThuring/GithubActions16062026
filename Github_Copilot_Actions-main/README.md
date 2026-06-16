# GitHub Copilot & GitHub Actions — hands-on materials

Working sample code, ready-to-run CI/CD workflows, hands-on labs, and quick-reference
cheat sheets for the 2-day **GitHub Copilot** (Day 1) and **GitHub Actions** (Day 2) training.

> Slides and trainer notes are kept out of this repo by design (see `.gitignore`).
> This repo is everything you need to **do the labs**.

---

## Setup (before Day 1)

- [ ] **VS Code** with the **GitHub Copilot** + **GitHub Copilot Chat** extensions.
- [ ] A GitHub account with an **active Copilot license** — confirm sign-in works *before* the session (corporate proxy/SSO sometimes blocks it).
- [ ] **Git** installed (`git --version`).
- [ ] **Node.js 20+** (`node --version`) — for the frontend track.
- [ ] **.NET 8 SDK** (`dotnet --version`) — for the .NET track.
- [ ] Rights to **create a repo and push** — the Actions labs run on real hosted runners.

A copy-paste version of this checklist is at the top of [`labs/README.md`](labs/README.md).

---

## Repository structure

```
.
├── README.md
├── labs/
│   ├── README.md                ← how the labs work + setup checklist
│   ├── lab-guide.md             ← all 12 hands-on exercises, step by step
│   ├── frontend-app/            ← TypeScript sample app (frontend track)
│   ├── dotnet-app/              ← .NET 8 sample app (.NET track)
│   └── .github/                 ← ready-to-run CI/CD workflows + custom actions
└── cheatsheets/
    ├── copilot-prompting-cheatsheet.md
    └── actions-cheatsheet.md
```

---

## How to use this repo

1. Read [`labs/README.md`](labs/README.md) and complete the setup checklist.
2. Pick the track that matches your stack — **frontend (TypeScript)** or **.NET**. The
   Copilot and Actions skills are identical; only the test command differs.
3. Work through [`labs/lab-guide.md`](labs/lab-guide.md): Labs 1–6 on Day 1 (Copilot),
   Labs 7–12 on Day 2 (Actions).
4. Keep the [`cheatsheets/`](cheatsheets/) open as you go.

| Day | Theme | Labs |
|-----|-------|------|
| 1 | GitHub Copilot — suggestions, prompting, modes, agentic workflows, testing | Labs 1–6 |
| 2 | GitHub Actions — workflows, marketplace, custom actions, CI/CD, capstone | Labs 7–12 |

---

## Run the sample apps locally

```bash
# Frontend track
cd labs/frontend-app && npm install && npm test

# .NET track
cd labs/dotnet-app && dotnet test
```

Both also build and test automatically via the workflows in `labs/.github/workflows/`
once you push this repo to GitHub. See [`labs/README.md`](labs/README.md) for getting
the labs onto GitHub for the Actions day.
