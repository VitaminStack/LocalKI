# LocalKIAgent

Ein kleines .NET-Konsolenprogramm, das über [Ollama](https://ollama.com/) lokal laufende LLM-Modelle (z. B. DeepSeek-R1) anruft und die Antworten in der Konsole schön formatiert und farbig ausgibt.

---

## 📦 Features

- **Lokal**: Läuft komplett auf deiner Maschine (4070 Ti & CUDA)
- **REPL**: Interaktive Prompt-Schleife mit `exit` zum Beenden
- **System-Prompt**: Erzwingt „immer auf Deutsch“
- **Drei Sektionen**:  
  1. **Frage** (Cyan)  
  2. **Denkprozess** (Gelb, aus `<think>…</think>` gezogen)  
  3. **Antwort** (Grün)  
- **Automatischer Zeilenumbruch** und **Prefix** (z. B. Bullet)  

---

## ⚙️ Voraussetzungen

- Windows, Linux oder macOS  
- **.NET 8.0 SDK** (oder höher)  
- **Visual Studio 2022** (optional) oder `dotnet` CLI  
- **Ollama** (lokal installiert, Version ≥ 0.4.x)  
- Mindestens 12 GB VRAM (RTX 4070 Ti)

---

## 🚀 Installation & Einrichtung

1. Repository klonen:
```bash
git clone https://github.com/dein-nutzername/LocalKIAgent.git
cd LocalKIAgent
```


2. Ollama installieren

Windows: Download & Installer von https://ollama.com/download
Linux/macOS: siehe Dokumentation

3. Ollama-Modelle herunterladen
```bash
ollama pull deepseek-r1:8b
```

4. Ollama-Server starten
```bash
ollama serve
```
5. NuGet-Pakete wiederherstellen & bauen
```bash
dotnet restore
dotnet build
```

![Beispiel-Ausgabe](Screen.png)
