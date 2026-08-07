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
services.AddTransient<ReviewCommand>();

services.AddTransient<IOutputStrategy, PromptOutputStrategy>();
services.AddTransient<IOutputStrategy, RawDiffOutputStrategy>();

var serviceProvider = services.BuildServiceProvider();

var command = serviceProvider.GetRequiredService<ReviewCommand>();
await command.ExecuteAsync(options);

Console.WriteLine("\nDone.");