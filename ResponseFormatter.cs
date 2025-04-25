using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.AI;

namespace LocalKIAgent
{
    /// <summary>
    /// Formatiert vollständige Konversationen für die Konsole:
    /// - Frage in eigener Farbe
    /// - Denkprozess (zwischen <think>…</think>) in eigener Farbe
    /// - Antwort in eigener Farbe
    /// - Wortumbruch an Wortgrenzen
    /// - optional Prefix pro Zeile
    /// </summary>
    public class ResponseFormatter
    {
        // Allgemeine Einstellungen
        public int WrapWidth { get; set; } = Console.WindowWidth;
        public bool EnableWordWrap { get; set; } = true;
        public string LinePrefix { get; set; } = "";         // Basis-Prefix vor jedem Zeilenblock

        // Farben für die Sektionen
        public ConsoleColor QuestionColor { get; set; } = ConsoleColor.Cyan;
        public ConsoleColor ThinkColor { get; set; } = ConsoleColor.Yellow;
        public ConsoleColor AnswerColor { get; set; } = ConsoleColor.Green;
        public ConsoleColor PrefixColor { get; set; } = ConsoleColor.DarkGray;
        public ConsoleColor TextColor { get; set; } = ConsoleColor.White;

        /// <summary>
        /// Schreibt eine komplett formatierte Konversation:
        /// - Frage
        /// - Denkprozess
        /// - Antwort
        /// </summary>
        public void WriteConversation(string question, IEnumerable<ChatMessage> messages)
        {
            // 1) Frage
            WriteSection("Frage:", question, QuestionColor);

            // Kombiniere alle Assistant-Nachrichten in einen String
            var raw = string.Join("\n", messages.Select(m => m.Text));

            // 2) Denkprozess vs Antwort trennen
            var match = Regex.Match(raw, @"<think>([\s\S]*?)<\/think>([\s\S]*)", RegexOptions.IgnoreCase);
            string thinkPart, answerPart;
            if (match.Success)
            {
                thinkPart = match.Groups[1].Value.Trim();
                answerPart = match.Groups[2].Value.Trim();
            }
            else
            {
                thinkPart = null;
                answerPart = raw.Trim();
            }

            // 3) Denkprozess (falls vorhanden)
            if (!string.IsNullOrEmpty(thinkPart))
            {
                WriteSection("Denkprozess:", thinkPart, ThinkColor);
            }

            // 4) Antwort
            WriteSection("Antwort:", answerPart, AnswerColor);
        }

        /// <summary>
        /// Schreibt eine Sektion mit Überschrift, Abstand und optionalem Zeilenumbruch.
        /// </summary>
        private void WriteSection(string header, string content, ConsoleColor sectionColor)
        {
            Console.WriteLine();  // Leerzeile vor jeder Sektion

            // Header
            Console.ForegroundColor = sectionColor;
            Console.WriteLine(header);
            Console.ResetColor();

            // Content
            WriteWrapped(content);
            Console.WriteLine();  // Leerzeile nach der Sektion
        }

        /// <summary>
        /// Bricht einen langen Text an Wortgrenzen um und schreibt ihn mit Prefix.
        /// </summary>
        private void WriteWrapped(string text)
        {
            var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var line = "";

            foreach (var word in words)
            {
                // prüfen, ob das nächste Wort plus Prefix die Breite sprengt
                if (EnableWordWrap &&
                    (LinePrefix.Length + line.Length + word.Length + 1 > WrapWidth))
                {
                    WriteLineInternal(line);
                    line = word;
                }
                else
                {
                    line = line.Length == 0 ? word : line + " " + word;
                }
            }

            if (!string.IsNullOrEmpty(line))
                WriteLineInternal(line);
        }

        /// <summary>
        /// Schreibt eine einzelne Zeile mit Prefix und Textfarbe.
        /// </summary>
        private void WriteLineInternal(string content)
        {
            Console.ForegroundColor = PrefixColor;
            Console.Write(LinePrefix);

            Console.ForegroundColor = TextColor;
            Console.WriteLine(content);

            Console.ResetColor();
        }
    }
}
