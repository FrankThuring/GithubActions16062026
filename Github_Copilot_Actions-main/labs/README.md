# Hands-on labs

Working sample code + step-by-step exercises for the Copilot & Actions training.
Full instructions are in **[`lab-guide.md`](lab-guide.md)**.

## Participant setup checklist (do this before Day 1)

- [ ] VS Code installed.
- [ ] **GitHub Copilot** + **GitHub Copilot Chat** extensions installed.
- [ ] Signed in to GitHub in VS Code, Copilot icon active (test it through any corporate proxy/SSO).
- [ ] **Git** installed (`git --version`).
- [ ] **Node.js 20+** installed (`node --version`) — for the frontend track.
- [ ] **.NET 8 SDK** installed (`dotnet --version`) — for the .NET track.
- [ ] A GitHub account that can **create a repo and push** (for the Actions labs).

## What's here

```
labs/
├─ lab-guide.md            ← all 12 labs, step by step
├─ CHANGELOG.md            ← used by the custom action / capstone
├─ frontend-app/           ← TypeScript sample (frontend track)
├─ dotnet-app/             ← .NET 8 sample (.NET track)
└─ .github/
   ├─ workflows/           ← dotnet-ci, frontend-ci, deploy, nightly
   └─ actions/             ← a custom JS action + a composite action
```

## Two tracks, same concepts

Pick the app that matches your stack — the Copilot and Actions skills are identical:

| Track | App | Test command |
|-------|-----|--------------|
| Frontend (TS) | `frontend-app/` | `npm test` |
| .NET | `dotnet-app/` | `dotnet test` |

## Getting the labs onto GitHub (for the Actions day)

Day 2 needs a real repo so workflows run on hosted runners:

```bash
# from the repo root (the folder containing labs/)
git init
git add .
git commit -m "Training labs"
gh repo create copilot-actions-training --private --source=. --push
# or create a repo on github.com and `git remote add origin ... && git push -u origin main`
```

> The workflows live at `labs/.github/workflows/`. If you push **only** the
> `labs/` contents as the repo root, move `.github/` to the repo root so GitHub
> detects the workflows (GitHub only reads `.github/workflows` at the repo root).
> The lab guide explains both options.
