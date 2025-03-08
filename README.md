# Mawasure

## 📌 Description

Mawasure is a Unity RPG where players take on the role of a mage, designing combat strategies through a modular spell system. This project demonstrates the use of multiple design patterns to optimize performance, scalability, and code maintainability.
You can download the build to test the prototype here: https://mauronu.itch.io/mawasure

## 🎮 Main Features

**Modular spell system:** Implemented with ScriptableObjects and a custom editor to create different spell-casting strategies (single cast, multi-cast, AOE cast, chain cast, etc.).

**Strategic combat mechanics:** Based on casting times, ranges, effects, and spell synergies.

**UI system for spell customization and stats:** Allows players to dynamically assign and modify their spells.

**State-based AI:** Enemies use a state machine to patrol, attack, and cast spells based on the situation.

**Optimized design using design patterns:** Implementation of advanced patterns to enhance code organization and efficiency.

## 🛠️ Design Patterns Implemented

**Object Pool:** A creational pattern that minimizes memory use and processing time by reusing objects instead of creating and destroying them repeatedly.

**Service Locator:** A pattern that provides a centralized registry to retrieve services, reducing dependency coupling in a flexible way.

**Flyweight:** A structural pattern that reduces memory consumption by sharing common parts of objects instead of storing them separately.

**Command:** A behavioral pattern that turns a request into a stand-alone object, allowing parameterization, queuing, and undo/redo operations.

**Abstract Factory:** A creational pattern that provides an interface for creating families of related objects without specifying their concrete classes.

**State:** A behavioral pattern that allows an object to change its behavior when its internal state changes. Instead of using conditionals, different states are represented as separate classes, making transitions more explicit and manageable.

**State Machine:** A design concept that models an entity as a set of states, transitions, and actions. It provides a structured way to handle different behaviors and conditions, often implemented using the *State pattern* to manage state transitions dynamically.

**Strategy:** A behavioral pattern that lets a class define a family of algorithms, encapsulate each one, and switch them dynamically.

**Wrapper:** A structural pattern that provides a simplified interface to a more complex system, improving usability and modularity, and enhancing functionality.

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
