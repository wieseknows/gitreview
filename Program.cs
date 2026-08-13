using GitReview.Composition;
using GitReview.Git;
using GitReview.Models;
using GitReview.Prompt;
using GitReview.Strategies;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("GitReview started\n");

var options = ReviewOptions.Parse(args);

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
await command.ExecuteAsync(options, cts.Token);

Console.WriteLine("\nDone.");