using GitReview.Cli.Composition;
using GitReview.Cli.Parsing;
using GitReview.Core.Git;
using GitReview.Core.Models;
using GitReview.Core.Prompt;
using GitReview.Core.Strategies;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

Console.WriteLine("GitReview started\n");

var options = ReviewOptionsParser.Parse(args);

var services = new ServiceCollection();

services.AddSingleton<IGitService, GitService>();
services.AddSingleton<IPromptBuilder, PromptBuilder>();

services.AddLlmReviewService(options.Provider);

services.AddTransient<IOutputStrategy, PromptOutputStrategy>();
services.AddTransient<IOutputStrategy, RawDiffOutputStrategy>();
services.AddTransient<IOutputStrategy, AiReviewOutputStrategy>();

services.AddTransient<ReviewCommand>();

var serviceProvider = services.BuildServiceProvider();

using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

var command = serviceProvider.GetRequiredService<ReviewCommand>();
await command.ExecuteAsync(options.Mode, cts.Token);

Console.WriteLine("\nDone.");