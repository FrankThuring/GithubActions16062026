# GitHub Copilot — prompting & usage cheat sheet

A one-page reference to hand out. Print double-sided with the Actions cheat sheet.

---

## The one idea that matters

> **Context is the lever.** Copilot suggests from what it can see: open files, names, types, comments, your prompt, and repo instructions. Shape the context and you shape the output.

---

## Keyboard & UI (VS Code)

| Action | How |
|--------|-----|
| Accept suggestion | `Tab` |
| Dismiss | `Esc` |
| Next / previous suggestion | `Alt+]` / `Alt+[` |
| Suggestions panel | `Ctrl+Enter` |
| Inline chat | `Ctrl+I` |
| Chat view | `Ctrl+Alt+I` |
| Open Copilot Edits | Chat menu → Edits |

## Slash commands (Chat)

| Command | Use |
|---------|-----|
| `/explain` | explain selected code |
| `/fix` | propose a fix for a problem/error |
| `/tests` | generate tests for the selection |
| `/doc` | generate doc comments |
| `/optimize` | suggest performance/clarity improvements |
| `/new` | scaffold a new project/file |

## Chat participants & context variables

| Token | Meaning |
|-------|---------|
| `@workspace` | reason across the whole project |
| `@terminal` | help with terminal/commands |
| `@vscode` | help with the editor itself |
| `#file:path` | pin a specific file as context |
| `#selection` | the current selection |
| `#changes` | your current diff |
| `#codebase` | search the codebase for relevant context |

---

## Prompt patterns that work

**1. Specify the contract, not just the task**
> "Angular standalone component, OnPush, inputs `userId: string` + `readonly: boolean`, emits `save`. Include the `.spec.ts`."

**2. Point at the style to copy**
> "`#file:user.service.ts` follow this file's style and error handling."

**3. Ask for the edge cases**
> "What test cases am I missing?" · "List the assumptions you made."

**4. Iterate small**
Accept → adjust → continue. Don't fight one giant suggestion.

**5. Make conventions permanent**
Commit `.github/copilot-instructions.md` so the whole team's Copilot follows the rules.

---

## Test generation prompt skeleton

```
/tests Use <framework>. Cover:
- happy path
- boundary values (0, max, empty)
- null / invalid input (expect throw)
- error path
Name tests as <given>_<when>_<then>.
```

## Refactor safely (order matters)

1. `/tests` → characterisation tests (lock current behaviour)
2. "Refactor into small pure functions, preserve behaviour, explain changes"
3. Apply across files with **Copilot Edits**
4. Re-run tests → green = safe

---

## Review checklist for AI-generated code

- [ ] Does only what I asked?
- [ ] Edge cases: null/empty, boundaries, errors, concurrency?
- [ ] Security: validation, injection, authz, secret handling?
- [ ] Matches our conventions/architecture?
- [ ] Tests assert the **right** behaviour (not just present)?
- [ ] Licensing/duplication OK?

> **Team norm:** *AI-assisted, human-verified.*

## Privacy & security (regulated work)

- Never paste real secrets / PHI / PII into a prompt — treat chat as an external service.
- Use **content exclusion** to keep Copilot away from sensitive files (admin setting).
- Business/Enterprise: your code isn't used to train models.
- Demos use **synthetic data only**.

## When suggestions are bad

| Symptom | Fix |
|---------|-----|
| Empty | check sign-in / `github.copilot.enable` for the language |
| Generic | open related files; add a comment/signature; pin `#file` |
| Wrong style | add `copilot-instructions.md` |
| Repeats a bug | close the bad file; show a good example |
