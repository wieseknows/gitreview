# GitReview

CLI tool that converts `git diff` into a structured LLM code review prompt, exports raw diff patches, or directly performs automated AI code reviews via Gemini API. Automatically copies prompts/results to the clipboard and opens your file explorer with the generated file selected.

## Features

- 🤖 **Automated AI Review:** Sends diffs directly to Google Gemini API (`gemini-2.5-flash`) and renders code review results without manual copy-pasting.
- ⚡ **Prompt Generator:** Creates `review.md` with git metrics and copies it to clipboard for ChatGPT / Claude / DeepSeek.
- 📝 **Raw Patch Mode:** Exports clean `.diff` patch files without LLM prompt formatting.
- 📂 **Auto Focus:** Opens Windows Explorer / macOS Finder with the output file pre-selected.
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

## Configuration (For AI Mode)

To use automated AI reviews, obtain a free API key from [Google AI Studio](https://aistudio.google.com/app/apikey) and set the `GEMINI_API_KEY` environment variable:

- **Bash / Zsh:**
  ```bash
  export GEMINI_API_KEY="your_api_key_here"
  ```
- **PowerShell:**
  ```powershell
  $env:GEMINI_API_KEY="your_api_key_here"
  ```
- **Windows CMD:**
  ```cmd
  set GEMINI_API_KEY=your_api_key_here
  ```

## Usage

Run directly from any Git repository:

- **Automated AI Review:** `git review --ai` (or `git review --gemini`)
- **Generate LLM Prompt:** `git review`
- **Export Raw Diff:** `git review raw` (or `git review -r`)

## Output Files

- `ai-review_result.md` — Detailed code review generated directly by Gemini AI.
- `review.md` — Formatted prompt ready for manual pasting into LLM chatbots.
- `git_changes.diff` — Clean git diff patch file.