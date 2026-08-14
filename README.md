# GitReview

CLI tool that converts `git diff` into a structured LLM code review prompt, exports raw diff patches, or directly performs automated AI code reviews via Gemini, DeepSeek, or OpenRouter API. Automatically copies prompts/results to the clipboard and opens your file explorer with the generated file selected.

## Features

- 🤖 **Automated AI Review:** Sends diffs directly to LLM APIs (Google Gemini, DeepSeek, or OpenRouter) and renders code review results without manual copy-pasting.
- ⚡ **Prompt Generator:** Creates `review.md` with git metrics and copies it to clipboard for ChatGPT / Claude / Web interfaces.
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

Set the API key for your preferred provider as an environment variable:

### Google Gemini (Default)
Obtain a free API key from [Google AI Studio](https://aistudio.google.com/app/apikey).

- **Bash / Zsh:** `export GEMINI_API_KEY="your_api_key_here"`
- **PowerShell:** `$env:GEMINI_API_KEY="your_api_key_here"`
- **Windows CMD:** `set GEMINI_API_KEY=your_api_key_here`

### OpenRouter (Free Tier Available)
Obtain a free API key from [OpenRouter Keys](https://openrouter.ai/keys).

- **Bash / Zsh:** `export OPENROUTER_API_KEY="your_api_key_here"`
- **PowerShell:** `$env:OPENROUTER_API_KEY="your_api_key_here"`
- **Windows CMD:** `set OPENROUTER_API_KEY=your_api_key_here`

*(Optional)* Change default model (e.g. `poolside/laguna-s-2.1:free`): `export OPENROUTER_MODEL="poolside/laguna-s-2.1:free"`

### DeepSeek
Obtain an API key from [DeepSeek Platform](https://platform.deepseek.com/).

- **Bash / Zsh:** `export DEEPSEEK_API_KEY="your_api_key_here"`
- **PowerShell:** `$env:DEEPSEEK_API_KEY="your_api_key_here"`
- **Windows CMD:** `set DEEPSEEK_API_KEY=your_api_key_here`

*(Optional)* Set `GIT_REVIEW_PROVIDER="openrouter"` or `"deepseek"` if you want to override the default provider globally.

## Usage

Run directly from any Git repository:

### Automated AI Review
- `git review --ai` — Run AI review using default provider (Gemini).
- `git review --ai -p openrouter` (or `git review --openrouter`) — Run AI review using OpenRouter (Free models).
- `git review --ai -p deepseek` (or `git review --deepseek`) — Run AI review using DeepSeek.
- `git review --ai -p gemini` (or `git review --gemini`) — Run AI review using Gemini explicitly.

### Prompt & Patch Generation
- `git review` — Generate structured LLM prompt (`review.md`).
- `git review raw` (or `git review -r`) — Export raw git diff patch (`git_changes.diff`).

## Output Files

- `ai_review_result.md` — Detailed code review generated directly by the selected AI model.
- `review.md` — Formatted prompt ready for manual pasting into LLM chatbots.
- `git_changes.diff` — Clean git diff patch file.