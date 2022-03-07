# Word List

This is a coding challenge to find a route from word A to word B by changing only a single letter at a time. All intermediate words should exist in a supplied dictionary. The path between the two words should be the shortest.

While this is only a sample console app, I've still used standard practices such as DI (note that currently everything is bound as singleton, not necessarily how I'd do this in a larger application), TDD, single responsibility/separation of concerns etc to demonstrate how I'd normally approach a solution. For example, the role of interacting with the file system is separated into a class implementing IWordRepository. A separate object (IDictionaryReader) then uses this interface to load the entire list from the file, but adds extra functionality to restrict the list to only words with the appropriate number of letters. It would therefore be trivial to change the data source to a different file format, a web-based API etc, while still maintaining the functionality provided by IDictionaryReader.

This is also how I've implemented the actual code to plan the route - there's an interface (IWordRoutePlanner), and then different concrete implementations to allow different algorithms to be used. To demonstate this, I've supplied two implementations (see below).

There are a few TODOs left in the code to document some of the changes I considered - for example, the character count is currently limited to 4 characters by the GetFourLetterWords() method; it might have been better to have provided a method that takes the number of characters as a parameter; this value could then be driven by the length of start word.

The solution was test driven; however, not every individual item has it's own test class. For example, the extension methods were split out from the code they are used in at later stages in the development, either for readability or to eliminate duplication. In these cases, they are covered as part of the tests on consuming code.

I didn't follow any other documented way of handling this; I was aware that there would be pre-existing strategies, but I was having fun trying to come up with my own, so I didn't look! I did, however, consider looking to see if there were any web-based APIs that were already provided that I could hook into from a WordRoutePlanner implementation....


## Implementation 1 - SingleLetterMergeWordRoutePlanner
The initial implementation (SingleLetterMergeWordRoutePlanner) is simple and naive - it is written to handle the example scenario in the spec (there is a unit test that proves this out), and achieves the goal by supplanting each letter of the source word with the letter from the same position in the target word until source and target match. This makes two assumptions:

1. That there is a direct path from source to target (ie, each character must change a maximum of once); 
2. That each intermediate word is in the dictionary. 

If these two assumptions are met, this offers a very quick mechanism; the number of steps will never be any more than (word length - 1). Essentially, it was a quick way for me to set the project up and get it working with a known set of inputs and get a feel for how to approach the solution. 

## Implementation 2 - BruteForceWordRoutePlanner
For more complex cases where no assumptions can be made, there is a "brute force" approach that walks a path from the source word to the target by finding all words in the dictionary that differ by a single letter. This process is repeated recursively until the path either ends in the target word or else runs out of matches. The path with the least number of steps is then the winner.

Because of this recursive nature, the algorithm operates on a single-threaded basis; to speed things up, it might be possible to encapsulate each path into individual objects and potentially process them in parallel - an actor framework could be useful here. Certainly care would have to be taken about how granular to make the encapsulation; given that each word may have 5 words differing by 1 letter, and those 5 may each have 5 more etc, the number of threads being used in parallel could get large quickly.

(One change I considered was to run ProcessNextSetOfWords in Task.Run(), and return the tasks from the Select on Line 51 of BruteForceWordRoutePlanner.cs. This would result in an array of Tasks that could then be passed Task.WaitAll. However, I thought that this may chew through the thread pool quite quickly so I didn't pursue it). 

While this process should find any path between start and end, no matter how many times each character must change to get there, it is dumb and slow, as the number of paths to explore multiplies quite quickly.  After getting the tests working and ensuring it worked in practice, I made two tweaks to improve performance:

- Paths are explored in order of the number of characters they already share with the end word. This hopefully implies a shorter path to the end word.
- The shortest route to the end word is tracked as the search progresses; any searches that go over this length are terminated as even if successful they will not yield the shortest route.

Another small performance improvement was to move the GetWordsDifferingBySingleCharacter method into an extension method; this was originally a method on a WordMatcher class, and the code spun up a new instance of this class on every recursion. I figured that this was unnecessary generation of new objects given that the class actually held no state. The original plan was to have a set of objects that represented the journey along the word "ladder". This could then possibly be used in a multi-threaded way since each object set could maintain the state they needed, which would allow processing to occur in parallel. However, as the idea progressed I found that the recursive mechanism now in use was simpler and, I think, more elegant, I guess at the expense of scalability.

The choice of algorithm is determined in the DI setup; to switch between the two, it's just a matter of changing the binding for IWordRoutePlanner in Program.cs.


