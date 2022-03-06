// See https://aka.ms/new-console-template for more information
using Blueprism.Wordlist.Core;
using Microsoft.Extensions.DependencyInjection;

var dictionaryFileName = args[0];
var startWord = args[1];
var endWord = args[2];
var resultsFileName = args[3];


var serviceProvider = new ServiceCollection()
    .AddSingleton<IWordRepository>(new WordRepository(dictionaryFileName, resultsFileName))
    .AddSingleton<IDictionaryReader, DictionaryReader>()
    //.AddSingleton<IWordRoutePlanner, SingleLetterMergeWordRoutePlanner>()
    .AddSingleton<IWordRoutePlanner, BruteForceWordRoutePlanner>()
    .AddSingleton<IWordlistProcessor, WordlistProcessor>()
    .BuildServiceProvider();

var processor = serviceProvider.GetService<IWordlistProcessor>();
var wordRoute = processor!.CalculatePathFromStartToEndWords(startWord, endWord);

foreach (var word in wordRoute)
{
    Console.WriteLine(word);
}

Console.ReadLine();

