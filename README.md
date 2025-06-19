# Survival Rush: Fire & Escape 🚀

**Survival Rush** is a .NET C# dungeon‑style shooter where you're trapped and pursued by enemies—and you become the hunter. Fight to survive using a clean **MVP architecture** and **Entity Framework**.

---

## 🎮 Demo & Preview

(No demo link provided—feel free to add a screenshot or video here)

---

## 🧩 Features

- **Top-down shooter** gameplay: navigate a dungeon, evade enemies, and shoot back  
- **MVP architecture**: structured separation of concerns for cleaner, testable code  
- **Entity Framework integration**: manages game state, player data, and high scores  
- **Basic AI opponents**: pursue and attack the player

---

## 🛠️ Technologies

- **C#** (.NET 6+)
- **Entity Framework Core**
- **Model‑View‑Presenter (MVP)** design  
- (Optional) Add Unity or WinForms/WPF if applicable

---

## 🚀 Getting Started

### Prerequisites  
- [.NET 6 SDK (or later)](https://dotnet.microsoft.com/download)  
- (Optional) Visual Studio 2022 or another C# IDE

### Installation

Clone the repo:  
\`\`\`bash
git clone https://github.com/Jeysixczs/Survival-Rush.git
cd Survival-Rush
\`\`\`

Restore packages and build:  
\`\`\`bash
dotnet restore
dotnet build
\`\`\`

Run the game:  
\`\`\`bash
dotnet run --project src/SurvivalRushGame/SurvivalRushGame.csproj
\`\`\`

---

## 🧠 Architecture

\`\`\`
+-----------+       +-------------+        +----------------+
|   View    | <---> | Presenter   | <--->  |   Model (EF)   |
+-----------+       +-------------+        +----------------+
\`\`\`

1. **View**: UI layer (console, WinForms, Unity, etc.)  
2. **Presenter**: handles input, updates, and state logic  
3. **Model**: game entities (player, enemies, dungeon) persisted via EF Core

---

## 🪛 Gameplay Loop

1. Game loop ticks on user input or timer  
2. Presenter processes movement and enemy AI  
3. Council actions: shooting, collisions, score updates  
4. Model saves game/session data  
5. View renders updates

---

## 🔧 Contributing

Your ideas and help are welcome!

1. Fork the repository  
2. Create a feature branch  
3. Add tests where applicable  
4. Open a pull request with a detailed description

---

## 📄 License

Licensed under the [MIT License](LICENSE).

---

## ✉️ Contact

Maintained by [Jeysixczs](https://github.com/Jeysixczs).  
Feel free to open issues or PRs!

---

### ✅ To Do

- [ ] Add README screenshots/gifs  
- [ ] Provide release builds  
- [ ] Include unit tests (e.g., for Presenter logic)  
- [ ] Consider cross-platform support (Windows/macOS/Linux)

---

Enjoy the chase—and good luck staying alive! 💥`
