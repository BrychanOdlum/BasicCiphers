using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace assignments {

 	class CipherSolver {

		private string inputFile = "caesarShiftEncoded.txt";
		private string outputFile = @"cipherLogger.txt";
		private string cipherText = "";

		static void Main(string[] args) {

			// nice friendly lil' header for console output...
			Console.WriteLine("\n");
			Console.WriteLine("// ########## ########## ########## //");
			Console.WriteLine("     ASSIGNMENT 1 :: SHIFT SOLVER");
			Console.WriteLine("// ########## ########## ########## //");
			Console.WriteLine("");

			// make an object for the ShiftSolver
			CipherSolver solver = new CipherSolver();


			solver.loggerInitiate();

			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("Would you like to run the caesar cipher or the affine cipher?");
			Console.ResetColor();
			Console.Write("[Caesar/affine] ");
			string response = Console.ReadLine().ToLower();
			if (response == "caesar" || response == "c" || response == "shift" || response == "basic")
				solver.runCaesar();
			else if (response == "affine" || response == "a" || response == "enhanced")
				solver.runAffine();
			else
				Console.WriteLine("[ERR] Your input was not recognised");
		}

		private void loggerInitiate() {
			try {
				if (!File.Exists(outputFile))  {
				    // Create a file to write to.
				    StreamWriter outputStream = File.CreateText(outputFile);
					outputStream.Close();
				}
			} catch (Exception e) {
				Console.WriteLine("\nThe following exception was caught: \n{0}", e);
			}
		}

		private void loggerLine(string line) {
			try {
				File.AppendAllText(outputFile, line + "\n");
			} catch (Exception e) {
				Console.WriteLine("\nThe following exception was caught: \n{0}", e);
			}
		}

		private void importFile() {
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("Would you like to import the file:");
			Console.ResetColor();
			Console.WriteLine(Directory.GetCurrentDirectory() + "/" + inputFile);
			Console.Write("[Y/n] ");
			if (!Utils.responseToBool(Console.ReadLine())) {
				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.WriteLine("Please enter the file name for the file you\'d like to import");
				Console.ResetColor();
				Console.Write(Directory.GetCurrentDirectory() + "/");
				inputFile = Console.ReadLine();
			}

			loggerLine("--> IMPORTING FILE");
			//	check the file exists (using System.IO), otherwise cancel the program.
			/*if (!File.Exists(inputFile))  {
			}*/
			try {
				cipherText = File.ReadAllText(inputFile);
			} catch {
				// the input text file could not be found
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("[ERR] The file, {0}, does not exist. Please check this further.", inputFile);
				Console.WriteLine("\n");

				inputFile = "caesarShiftEncoded.txt";
				importFile();
			}
			loggerLine("--> IMPORTED FILE");
		}

		private void runAffine() {
			loggerLine("--> RUNNING: AFFINE");
			// nice friendly lil' header for console output...
			Console.WriteLine("\n");
			Console.WriteLine("// ########## ########## ########## //");
			Console.WriteLine("   AFFINE CIPHER");
			Console.WriteLine("// ########## ########## ########## //");
			Console.WriteLine("");

			// import the text file...
			inputFile = "caesarShiftEnhancedEncoding.txt";
			importFile();

			// display the original text file
			Console.WriteLine("Input Contents:");
			Console.WriteLine(cipherText);

			loggerLine("--> INPUT TEXT");
			loggerLine(cipherText);

			// create a Frequency Analysis object
			FrequencyAnalysis freqAnal = new FrequencyAnalysis();

			// iterate the possible shift caesars
			Console.WriteLine("\n -- [ITERATE KEYS] --");
			int[] coprimes = {1, 3, 5, 7, 9, 11, 15, 17, 19, 21, 23, 25};		// these are the possible values of a, as shown on the wiki
			foreach (int var_a in coprimes) {
				for (int i = 0; i < 26; i++) {
					string resultant = Ciphers.decipherAffine(cipherText, var_a, i);
					freqAnal.addOption(var_a.ToString() + "," + i.ToString(), resultant);
					Console.WriteLine("[OPTION:{0}:{1}]", var_a, i);
					Console.WriteLine(resultant);

					loggerLine("--> [OPTION:" + var_a + ":" + i + "]");
					loggerLine(resultant);
				}
			}

			// nice friendly lil' header for console output...
			Console.WriteLine("\n");
			Console.WriteLine("// ########## ########## ########## //");
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("     LOWEST FREQUENCY DIFFERENCE");
			Console.WriteLine("       (most probable result)");
			Console.ResetColor();
			Console.WriteLine("// ########## ########## ########## //");
			Console.WriteLine("");

			string[] solution = freqAnal.solution().Split(',');
			Console.WriteLine("AFFINE KEY BEST FIT: a={0}, b={1}\n", solution[0], solution[1] );

			Console.WriteLine(Ciphers.decipherAffine(cipherText, Int32.Parse(solution[0]), Int32.Parse(solution[1])));


			loggerLine("--> [LOWEST FREQUENCY DIFFERENCE: a=" + solution[0] + ", b=" + solution[1] + "");
			loggerLine(Ciphers.decipherAffine(cipherText, Int32.Parse(solution[0]), Int32.Parse(solution[1])));

		}


		private void runCaesar() {
			loggerLine("--> RUNNING: CAESAR");
			// nice friendly lil' header for console output...
			Console.WriteLine("\n");
			Console.WriteLine("// ########## ########## ########## //");
			Console.WriteLine("     CAESAR SHIFT CIPHER");
			Console.WriteLine("// ########## ########## ########## //");
			Console.WriteLine("");

			// import the text file...
			importFile();

			// display the original text file
			Console.WriteLine("Input Contents:");
			Console.WriteLine(cipherText);

			// create a Frequency Analysis object
			FrequencyAnalysis freqAnal = new FrequencyAnalysis();

			// iterate the possible shift caesars
			Console.WriteLine("\n -- [ITERATE KEYS] --");
			for (int i = 0; i < 26; i++) {
				string resultant = Ciphers.decipherCaesar(cipherText, i);
				freqAnal.addOption(i.ToString(), resultant);
				Console.WriteLine("[OPTION:{0}]", i);
				Console.WriteLine(resultant);

				loggerLine("--> [OPTION:" + i + "]");
				loggerLine(resultant);
			}

			// nice friendly lil' header for console output...
			Console.WriteLine("\n");
			Console.WriteLine("// ########## ########## ########## //");
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("     LOWEST FREQUENCY DIFFERENCE");
			Console.WriteLine("       (most probable result)");
			Console.ResetColor();
			Console.WriteLine("// ########## ########## ########## //");
			Console.WriteLine("");

			Console.WriteLine(freqAnal.solution());

			Console.WriteLine(Ciphers.decipherCaesar(cipherText, Int32.Parse(freqAnal.solution())));


			loggerLine("--> [LOWEST FREQUENCY DIFFERENCE: " + freqAnal.solution() + "]");
			loggerLine(Ciphers.decipherCaesar(cipherText, Int32.Parse(freqAnal.solution())));
		}




	}

	class Utils {
		public static bool responseToBool(string response) {
			response = response.ToLower();
			if (response == "" || response == "y" || response == "yes" || response == "t" || response == "true" || response == "yarp")
				return true;
			return false;
		}

		/*public static void output(string text) {
			Console.WriteLine(text);
		}*/
	}

	class FrequencyAnalysis {

		// define the standardised frequencies of characters in the English language
		private static Dictionary<char, double> engFrequencies = new Dictionary<char, double> () {
			{ 'a', 8.1670 }, { 'b', 1.492 }, { 'c', 2.782 }, { 'd', 4.253 },
			{ 'e', 12.702 }, { 'f', 2.228 }, { 'g', 2.015 }, { 'h', 6.094 },
			{ 'i', 6.9660 }, { 'j', 0.153 }, { 'k', 0.772 }, { 'l', 4.025 },
			{ 'm', 2.4060 }, { 'n', 6.749 }, { 'o', 7.507 }, { 'p', 1.929 },
			{ 'q', 0.0950 }, { 'r', 5.987 }, { 's', 6.327 }, { 't', 9.056 },
			{ 'u', 2.7580 }, { 'v', 0.978 }, { 'w', 2.361 }, { 'x', 0.150 },
			{ 'y', 1.9740 }, { 'z', 0.074 }
		};

		private Dictionary<string, int> attempts = new Dictionary<string, int> ();

		public void addOption(string key, string text) {

			Dictionary<char, int> charCounter = new Dictionary<char, int> () {
				{ 'a', 0 }, { 'b', 0 }, { 'c', 0 }, { 'd', 0 },
				{ 'e', 0 }, { 'f', 0 }, { 'g', 0 }, { 'h', 0 },
				{ 'i', 0 }, { 'j', 0 }, { 'k', 0 }, { 'l', 0 },
				{ 'm', 0 }, { 'n', 0 }, { 'o', 0 }, { 'p', 0 },
				{ 'q', 0 }, { 'r', 0 }, { 's', 0 }, { 't', 0 },
				{ 'u', 0 }, { 'v', 0 }, { 'w', 0 }, { 'x', 0 },
				{ 'y', 0 }, { 'z', 0 }
			};
			List<int> percentageDiffs = new List<int>();

			int totalCharCount = 0;
			foreach(char c in text) {
				int charIndex = (int)c;
				if ((charIndex >= 65 && charIndex <= 90) || (charIndex >= 97 && charIndex <= 122))
					totalCharCount++;
				char newCharLower = Char.ToLower(c);
				if (charCounter.ContainsKey(newCharLower))
					charCounter[newCharLower]++;
			}
			foreach (var item in charCounter) {
				double charPercentage = (100.00/totalCharCount)*item.Value;
				double percentageDiff = ( (engFrequencies[item.Key]-charPercentage)/engFrequencies[item.Key] ) * 100;
				percentageDiff = Math.Abs(percentageDiff);
				percentageDiffs.Add(Convert.ToInt32(percentageDiff));
			}
			int averageDiff = 0;
			foreach (var item in percentageDiffs)
				averageDiff += item;
			averageDiff = averageDiff/26;

			attempts.Add(key, averageDiff);

			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(averageDiff);
			Console.ResetColor();
		}

		public string solution() {
			string final_holder = "";
			int final_freq = -1;
			foreach (var item in attempts) {
				if (final_holder == "") {
					final_holder = item.Key;
					final_freq = item.Value;
				} else if (item.Value < final_freq) {
					final_holder = item.Key;
					final_freq = item.Value;
				}
			}
			return final_holder;
		}
	}

	class Ciphers {
		public static string decipherCaesar(string text, int offset) {
			// in the event of a negative offset, just reverse it
			if (offset < 0)
				offset = 26-offset;
			string builder = "";
			int charIndex = 0;
			foreach(char c in text) {
				charIndex = (int)c;
				if (charIndex >= 65 && charIndex <= 90) {
					charIndex -= offset;
					if (charIndex < 65)
						charIndex += 26;
				} else if (charIndex >= 97 && charIndex <= 122) {
					charIndex -= offset;
					if (charIndex < 97)
						charIndex += 26;
				}
				char newChar = (char)charIndex;
				builder += newChar;
			}
			return builder;
		}

		public static string decipherAffine(string text, int var_a, int var_b) {
			string builder = "";
			foreach(char c in text) {
				int charIndex = (int)c;
				string tempo = "{";
				tempo += charIndex + ", ";
				if (charIndex >= 65 && charIndex <= 90) {
					charIndex -= 65;
					charIndex = ((var_a^-1) * (charIndex - var_b)) % 27;
					if (charIndex < 0)
						charIndex += 26;
					charIndex += 65;
				} else if (charIndex >= 97 && charIndex <= 122) {
					charIndex -= 97;
					charIndex = ((var_a^-1) * (charIndex - var_b)) % 27;
					if (charIndex < 0)
						charIndex += 26;
					charIndex += 97;
				}
				builder += (char)charIndex;
				tempo += charIndex + "}";
			}
			return builder;
		}

	}


}
