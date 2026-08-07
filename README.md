# GitReview

CLI tool that converts `git diff` into a structured LLM code review prompt or exports raw diff patches. Automatically copies prompts to the clipboard and opens Windows Explorer with the generated file selected.

## Features

- **Prompt Generator:** Creates `review.md` with git metrics and copies it to clipboard.
- **Raw Patch Mode:** Exports clean `.diff` without LLM prompt formatting.
- **Auto Focus:** Opens Windows Explorer with the output file pre-selected.

## Quick Start

### 1. Build

```bash
git clone [https://github.com/wieseknows/GitReview.git](https://github.com/wieseknows/GitReview.git)
cd GitReview
dotnet build -c Release
```

### 2. Configure Git Alias

Set the environment variable in your shell profile (`~/.bashrc` / `~/.zshrc`):

```bash
export GIT_REVIEW="/path/to/GitReview"
```

Add aliases to `~/.gitconfig`:

```ini
[alias]
    review = "!f(){ \"$GIT_REVIEW/bin/Release/net10.0/GitReview.exe\" \"$@\"; }; f"
    review-raw = review raw
```

## Usage

Run from any Git repository:

- **Generate LLM Prompt:** `git review`
- **Export Raw Diff:** `git review-raw` (or `git review raw`)

## Output

- `review.md` — Formatted prompt ready for ChatGPT / Claude / Gemini.
- `git_changes.diff` — Clean git diff patch.
