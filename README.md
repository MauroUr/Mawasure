# Mawasure

## 📌 Description

Mawasure is a Unity RPG where players take on the role of a mage, designing combat strategies through a modular spell system. This project demonstrates the use of multiple design patterns to optimize performance, scalability, and code maintainability.

## 🎮 Main Features

**Modular spell system:** Implemented with ScriptableObjects and a custom editor to create different spell-casting strategies (single cast, multi-cast, AOE cast, chain cast, etc.).

**Strategic combat mechanics:** Based on casting times, ranges, effects, and spell synergies.

**UI system for spell customization and stats:** Allows players to dynamically assign and modify their spells.

**State-based AI:** Enemies use a state machine to patrol, attack, and cast spells based on the situation.

**Optimized design using design patterns:** Implementation of advanced patterns to enhance code organization and efficiency.

## 🛠️ Design Patterns Implemented

**Object Pool:** Optimizes instance creation and reuse, reducing garbage collector load.

**Service Locator:** Facilitates dependency management and access to core services such as input handling and visual effects.

**Flyweight:** Reduces memory usage by sharing data for similar entities, such as animations and spell attributes.

**Command:** Separates player input logic from action execution, allowing greater flexibility and decoupling.

**Abstract Factory:** Provides an interface for creating families of related objects, such as different spell types.

**State and State Machine:** Controls enemy behavior and combat flow through clear and flexible state management.

**Strategy:** Defines different attack and defense strategies for enemies, making combat more varied.

**Wrapper:** Encapsulates services and functionalities to simplify usage and improve modularity.

## 🚀 Installation & Execution

1. Clone the repository:

2. git clone https://github.com/MauroUr/Mawasure.git

3. Open the project in Unity 2022 or later.

4. Run the main scene from the editor.

## 🎯 Project Goal

This project was developed to improve my skills in software design and game development, applying design patterns and optimization techniques in Unity.

## 📩 Contact

If you're interested in my work, you can contact me at:

LinkedIn: [Mauro N. Uriarte](https://www.linkedin.com/in/mauronu/)

Email: mauronuriarte@gmail.com
