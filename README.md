# GitReview

CLI tool that converts `git diff` into a structured LLM code review prompt or exports raw diff patches. Automatically copies prompts to the clipboard and opens Windows Explorer with the generated file selected.

## Features

- ⚡ **Prompt Generator:** Creates `review.md` with git metrics and copies it to clipboard.
- 📝 **Raw Patch Mode:** Exports clean `.diff` without LLM prompt formatting.
- 📂 **Auto Focus:** Opens Windows Explorer with the output file pre-selected.
- 🏗 **Zero Config:** Works out of the box as a native Git subcommand via .NET Global Tool.

## Installation

Install globally via .NET SDK:

```bash
dotnet tool install -g wieseknows.GitReview
```

To update to the latest version:

```bash
dotnet tool update -g wieseknows.GitReview
```

To uninstall:

```bash
dotnet tool uninstall -g wieseknows.GitReview
```

## Usage

Run directly from any Git repository:

- **Generate LLM Prompt:** `git review`
- **Export Raw Diff:** `git review raw` (or `git-review raw`)

## Output

- `review.md` — Formatted prompt ready for ChatGPT / Claude / Gemini.
- `git_changes.diff` — Clean git diff patch.
