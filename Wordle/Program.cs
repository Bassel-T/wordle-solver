using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Wordle {
	class Program {
		static void Main(string[] args) {
			bool[] greens = new bool[5];

			List<string> allWords = new List<string>();
			// Read all words into list
			Random rand = new Random();
			using (var reader = new StreamReader("5Letters.txt")) {
				while (!reader.EndOfStream) {
					allWords.Add(reader.ReadLine().ToLower());
				}
			}

start:
			List<string> possible = ((string[])allWords.ToArray().Clone()).ToList();


			string input = "";
			string result = ""; // B (Black), Y (Yellow), G (Green) ; Case-Insensitive

			while (possible.Count > 1) {
				Console.WriteLine("===================");
				Dictionary<char, int> vals = new Dictionary<char, int>();
				Dictionary<char, int> guesses = new Dictionary<char, int>();

				Console.WriteLine($"There are still {possible.Count} possible values for this word.");

				if (greens.Where(x => x).Count() != 4) {
					// Print random, possible words
					Console.WriteLine($"Some acceptable guesses by Wordle include: {possible[rand.Next(0, possible.Count)]}, {possible[rand.Next(0, possible.Count)]}, {possible[rand.Next(0, possible.Count)]}, {possible[rand.Next(0, possible.Count)]}, {possible[rand.Next(0, possible.Count)]}.");
				} else {
					// Print word that gives best chance of final letter
					// i.e. right, sight, might possible, print something like "Smart"
					int indexFalse = Array.IndexOf(greens, false);
					Console.WriteLine($"Number index {indexFalse + 1} is wrong. Possible words are:");
					possible.ForEach(x => Console.WriteLine(x));
					Console.WriteLine("A good reduction guess would be:");

					char[] letters = possible.Select(x => x.ElementAt(indexFalse)).ToArray();
					IEnumerable<string> bestGuess =
						from word in allWords
						orderby word.Intersect(letters).Count() descending
						select word;

					Console.WriteLine(bestGuess.First());
				}

				// Get user's guess
				Console.WriteLine("What did you guess?\n" +
					"Alternatively, type [print] to see all possible answers.\n" +
					"Type [start] to start over.\n" +
					"Type [stop] to exit.");
				input = Console.ReadLine();

				while (input.Length != 5 && !input.StartsWith('[')) {
					Console.WriteLine("The input has to be five characters long. Try again.");
					input = Console.ReadLine();
                }

				if (input == "[print]") {
					// Print all possible words
					possible.ForEach(x => Console.WriteLine(x));
					continue;
				}

				if (input == "[start]") {
					// Restart the program
					goto start;
				}

				if (input == "[stop]") {
					// Exit the program
					return;
				}

				if (input.StartsWith('[')) {
					Console.WriteLine("Invalid command.");
					continue;
                }

				greens = new bool[5];

				// Get website's output for guess
				Console.WriteLine("What was the result? Write B for black, Y for yellow, G for green.\n" +
					"(For example: BBYGG, YBBGG, YYBYY, ...)");
				result = Console.ReadLine().ToUpper();

				while (!Regex.IsMatch(result, @"^[BYG]{5}$")) {
					Console.WriteLine("That isn't a valid input. Try again.");
					result = Console.ReadLine().ToUpper();
                }

				// Analyze what letters are correct
				for (int i = 0; i < 5; ++i) {
					if (!vals.ContainsKey(input[i])) {
						vals.Add(input[i], 0);
						guesses.Add(input[i], 0);
					}

					++guesses[input[i]];

					if (result[i] == 'Y') {
						// Correct letter, wrong place
						++vals[input[i]];
						possible = possible.Where(x => (x[i] != input[i]) && (x.Contains(input[i]))).ToList();
					} else if (result[i] == 'G') {
						// Correct letter, correct place
						++vals[input[i]];
						greens[i] = true;
						possible = possible.Where(x => x[i] == input[i]).ToList();
					}
				}

				// Remove words with incorrect amount of certain letter
				foreach (KeyValuePair<char, int> pair in vals) {
					if (pair.Value == 0) {
						// Word does not contain letter
						possible = possible.Where(x => !x.Contains(pair.Key)).ToList();
					} else if (guesses[pair.Key] == pair.Value) {
						// Word contains at least that many instances of letter
						possible = possible.Where(x => x.Count(y => y == pair.Key) >= pair.Value).ToList();
					} else {
						// Word contains between m and n instances of letter
						possible = possible.Where(x => x.Count(y => y == pair.Key) >= pair.Value && x.Count(y => y == pair.Key) < guesses[pair.Key]).ToList();
					}
				}
			}

			// Print the final answer, repeat
			if (possible.Count == 1) {
				Console.WriteLine($"The solution is: {possible[0]}\n");
			} else {
				Console.WriteLine("There was an error somewhere in your input; no words fit the patterns.");
			}

			Console.WriteLine("Would you like to rerun this? (y/n)");
			if (Console.ReadLine().ToLower() == "y") {
				goto start;
			}
		}
	}
}
