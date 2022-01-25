using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace Wordle {
	class Program {
		static void Main(string[] args) {
			start:
			List<string> possible = new List<string>();
			
			// Read all words into list
			Random rand = new Random();
			using (var reader = new StreamReader("5Letters.txt")) {
				while (!reader.EndOfStream) {
					possible.Add(reader.ReadLine().ToLower());
				}
			}

			string input = "";
			string result = ""; // B (Black), Y (Yellow), G (Green)

			while (possible.Count > 1) {
				Console.WriteLine("===================");
				Dictionary<char, int> vals = new Dictionary<char, int>();
				Dictionary<char, int> guesses = new Dictionary<char, int>();

				Console.WriteLine($"There are still {possible.Count} possible values for this word.");

				// TODO : Replace with better guess
				Console.WriteLine($"Some possible guesses include: {possible[rand.Next(0, possible.Count - 1)]}, {possible[rand.Next(0, possible.Count - 1)]}, {possible[rand.Next(0, possible.Count - 1)]}, {possible[rand.Next(0, possible.Count - 1)]}, {possible[rand.Next(0, possible.Count - 1)]}.");

				// Get user's guess
				Console.WriteLine("What did you guess?\n" +
					"Alternatively, type [print] to see all possible answers.\n" +
					"Type [start] to start over.\n" +
					"Type [stop] to exit.");
				input = Console.ReadLine();

				if (input == "[print]") {
					// Print all possible words
					possible.ForEach(x => Console.WriteLine(x));
					continue;
				}

				if (input == "[start]") {
					// Restart the code
					goto start;
				}

				if (input == "[stop]") {
					// Exit the program
					return;
				}

				// Get website's output for guess
				Console.WriteLine("What was the result? Write B for black, Y for yellow, G for green.\n" +
					"(For example: BBYGG, YBBGG, YYBYY, ...)");
				result = Console.ReadLine().ToUpper();

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
						// Correct letter, right place
						++vals[input[i]];
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
				Console.WriteLine($"{possible[0]}\n");
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
