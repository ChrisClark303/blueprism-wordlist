# Word List

This is a coding challenge to find a route from word A to word B by changing only a single letter at a time. All intermediate words should exist in a supplied dictionary. The path between the two words should be the shortest.

While this is only a sample console app, I've still used standard practices such as DI (note that currently everything is bound as singleton, not necessarily how I'd do this in a larger application), TDD, single responsibility/separation of concerns etc to demonstrate how I'd normally approach a solution. For example, the role of interacting with the file system is separated into a class implementing IWordRepository. A separate object (IDictionaryReader) then uses this interface to load the entire list from the file, but adds extra functionality to restrict the list to only words with the appropriate number of letters. It would therefore be trivial to change the data source to a different file format, a web-based API etc, while still maintaining the functionality provided by IDictionaryReader.

This is also how I've implemented the actual code to plan the route - there's an interface (IWordRoutePlanner), and then different concrete implementations to allow different algorithms to be used. To demonstate this, I've supplied two implementations:

## Implementation 1 - SingleLetterMergeWordRoutePlanner
The initial implementation (SingleLetterMergeWordRoutePlanner) is simple and naive - it is written to handle the example scenario in the spec (there is a unit test that proves this out), and achieves the goal by supplanting each letter of the source word with the letter from the same position in the target word until source and target match. This makes two assumptions:

1. That there is a direct path from source to target (ie, each character must change a maximum of once); 
2. That each intermediate word is in the dictionary. 

If these two assumptions are met, this offers a very quick mechanism; the number of steps will never be any more than (word length - 1). 

## Implementation 2 - BruteForceWordRoutePlanner
For more complex cases where no assumptions can be made, there is a "brute force" approach that walks a path from the source word to the target by finding all words in the dictionary that differ by a single letter. This process is repeated recursively until the path either ends in the target word or else runs out of matches. The path with the least number of steps is then the winner.

While this process should find any path between start and end, no matter how many times each character must change to get there, it is dumb and slow, as the number of paths to explore multiplies quite quickly.  After getting the tests working and ensuring it worked in practice, I made two tweaks to improve performance:

- Paths are explored in order of the number of characters they already share with the end word. This hopefully implies a shorter path to the end word.
- The shortest route to the end word is tracked as the search progresses; any searches that go over this length are terminated as even if successful they will not yield the shortest route.

The solution is currently a recursive algorithm that takes a word, finds all other words that differ by a single character, and then calls itself for each of those words. Because of this recursive nature, this operates on a single-threaded basis; to speed things up, it might be possible to encapsulate each path into individual objects and potentially process them in parallel - an actor framework could be useful here.

The choice of algorithm is determined in the DI setup; to switch between the two, it's just a matter of changing the binding for IWordRoutePlanner in Program.cs.
