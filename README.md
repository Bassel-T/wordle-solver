# Wordle Solver

## Introduction

Wordle is a game that took the world by storm, inspiring many spin-offs. Its success led to its acquisition by the New York Times. In the game, the player must input five letters words to receive hints towards a final answer. If a letter lights up green, it's the right letter in the right position. If it lights up yellow, it's the right letter in the wrong position. If the letter is grayed out, the letter isn't in the word.

Where many players around the world may not have a sufficient-enough English vocabulary to guess five-letter words, a solution is to show the user relavent guesses to help cut down on letters. The algorithm is not intended to find the most efficient ways to solve Wordle. Rather, it is a tool to help show people what they can do. In the event someone finds four greens, the program will show the user a list of all possible words the solution can be, and offer a word that can eliminate most of those options, though only useful in easy mode.

## Requirements

No external libraries are needed to run the program.

## Installation

Download this code repository by using git:

`git clone https://github.com/Bassel-T/wordle-solver.git`

## Execute Code

### Windows

#### Method 1: Executable

To execute this program on Windows, simply download the code and run `Wordle.exe` in the path: `Wordle/bin/Debug/netcoreapp3.1`.

#### Method 2: Debugger

Open the project in Visual Studio or some other compiler capable of running C# code.

### Mac/Linux

To execute this program on Linux, make sure you have mono-complete installed. If not, run:

`sudo apt-get install mono-complete`

When you have it, enter the `Wordle` folder and run:

`mcs Program.cs`

Then run `mono Program.exe`

## Support

For help from the developer, please leave an issue in the issue tracker.
