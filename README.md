# 🚗 Racing Car Game

A feature-rich **2D Racing Car Game** developed using **C#**, **.NET**, and **Windows Forms**. The game demonstrates object-oriented programming principles through a modular architecture featuring collision detection, physics simulation, multiple enemy movement behaviors, audio management, and multiple game levels.

---

# 📖 Overview

The objective of the game is to control a racing car, avoid enemy vehicles and obstacles, collect power-ups, and survive through increasingly challenging levels. The project focuses on clean software architecture and demonstrates various programming concepts including abstraction, interfaces, inheritance, modular design, and event-driven programming.

---

# 🎮 Features

- 🚗 Player-controlled racing car
- 🎯 Multiple game levels
- 🤖 Intelligent enemy movement patterns
- ⚡ Energy booster pickups
- 💥 Collision detection system
- 🧮 Physics system
- 🎵 Background music and sound effects
- 🏆 Score tracking
- ⌨️ Keyboard controls
- 📖 Instructions screen
- 👤 Player information screen
- 🔄 Game session management
- 🎲 Randomized enemy behavior
- 📂 File handling
- ⏱️ Timer-based game loop
- 🧩 Modular object-oriented architecture

---

# 🛠 Technologies Used

- C#
- .NET
- Windows Forms
- Visual Studio

---

# 📂 Project Structure

```text
RacingCarGame/
│
├── Audios/
│   └── AudioManager.cs
│
├── Component/
│   └── Audio.cs
│
├── Core/
│   ├── AudioTrack.cs
│   ├── Game.cs
│   └── GameTime.cs
│
├── Entities/
│   ├── Enemy.cs
│   ├── EnergyBooster.cs
│   ├── GameObject.cs
│   └── Player.cs
│
├── Interfaces/
│   ├── IAudio.cs
│   ├── ICollidable.cs
│   ├── IDrawable.cs
│   ├── IMovable.cs
│   ├── IMovement.cs
│   ├── IPhysicsObject.cs
│   └── IUpdatable.cs
│
├── Movements/
│   ├── ChaseMovement.cs
│   ├── HorizontalMovement.cs
│   ├── JumpingMovement.cs
│   ├── KeyboardMovement.cs
│   ├── PatrolMovement.cs
│   ├── RandomPatrolMovement.cs
│   ├── VerticalMovement.cs
│   └── ZigzagMovement.cs
│
├── Resources/
│   ├── bgmusic.mp3
│   ├── collision.wav
│   ├── crash.wav
│   ├── energyEater.wav
│   ├── jump.wav
│   └── win.wav
│
├── Systems/
│   ├── CollisionSystem.cs
│   └── PhysicsSystem.cs
│
├── MainForm.cs
├── SelectForm.cs
├── InstructionsForm.cs
├── PlayerInfoForm.cs
├── Level2Form.cs
├── Level3Form.cs
├── GameSession.cs
├── FileManager.cs
├── RacingCar.cs
├── Program.cs
│
├── README.md
├── LICENSE
└── RacingCarGame.sln
```

---

# 🚀 Getting Started

## Prerequisites

- Visual Studio 2022 (or later)
- .NET SDK
- Windows Operating System

## Installation

1. Clone the repository.

```bash
git clone https://github.com/RafyaJamil/racing-car-game.git
```

2. Open `RacingCarGame.sln` in Visual Studio.

3. Build the solution.

4. Press **F5** or click **Start** to run the game.

---

# 🎮 Controls

| Key | Action |
|------|--------|
| ← | Move Left |
| → | Move Right |
| ↑ | Move Forward / Jump (if applicable) |
| ↓ | Move Backward (if applicable) |

---


# 🏗 Software Architecture

The project follows a modular object-oriented architecture by separating responsibilities into different components.

### Core
Contains the primary game engine and game timing logic.

### Entities
Represents all game objects such as the player, enemies, and power-ups.

### Interfaces
Defines reusable contracts including movement, collision detection, drawing, updating, and physics.

### Systems
Implements collision handling and physics calculations.

### Movements
Contains different movement algorithms used by enemies and the player.

### Audio
Manages background music and sound effects.

---

# 📚 Concepts Demonstrated

- Object-Oriented Programming
- Interfaces
- Inheritance
- Abstraction
- Polymorphism
- Event-driven Programming
- Collision Detection
- Physics Simulation
- Timer-based Game Loop
- File Handling
- Modular Software Design

---

# 🎯 Learning Outcomes

This project helped me strengthen my understanding of:

- C# Programming
- .NET Development
- Windows Forms
- Game Development Fundamentals
- Object-Oriented Programming
- Software Architecture
- Event Handling
- Collision Detection
- Physics Systems
- Audio Management
- Debugging
- Problem Solving

---


# 👨‍💻 Author

**Rafya Jamil**

Computer Science Student — UET Lahore

GitHub: https://github.com/RafyaJamil

---

## ⭐ Support

If you found this project interesting, consider giving it a ⭐ on GitHub.
