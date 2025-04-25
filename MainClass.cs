using System;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using LocalKIAgent;  // Hier liegt dein ResponseFormatter

class MainClass
{
    static async Task Main(string[] args)
    {
        // 1) Ollama-Client
        var client = new OllamaChatClient(
            new Uri("http://localhost:11434/"),
            modelId: "deepseek-r1:8b"
        );

        // 2) Formatter konfigurieren
        var formatter = new ResponseFormatter
        {             // Beispiel-Prefix
            PrefixColor = ConsoleColor.DarkGreen,
            EnableWordWrap = true,
            // WrapWidth = 80,                      // alternativ feste Breite
        };

        Console.WriteLine("Lokale KI bereit — gib deinen Prompt ein (oder 'exit' zum Beenden):");
        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input) || input.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase))
                break;

            try
            {
                // 3) System- und User-Nachricht
                var systemMessage = new ChatMessage(ChatRole.System, " Antworte immer auf Deutsch!");
                var userMessage = new ChatMessage(ChatRole.User, input);

                // Wichtig: System zuerst, dann User
                var response = await client.GetResponseAsync(new[] {  userMessage, systemMessage });

                // 4) Ausgabe über den Formatter
                formatter.WriteConversation(input,response.Messages);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Fehler: {ex.Message}");
                Console.ResetColor();
            }
        }

        Console.WriteLine("Programm beendet.");
    }
}
