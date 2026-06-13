# AI Usage Disclosure
This document outlines the artificial intelligence tools and methodologies used during the development of this project, in compliance with the assignment requirements.

## 1. Tools Used
* **Gemini 1.5 Pro / 2.0** - Used for architectural suggestions, asset design logic, code refactoring, and structural formatting.


## 2. Methodology & How Tools Were Used
* **Asset & Character Design:** AI was used to assist in defining the visual logic, dimensions, and rendering properties for the main game characters, specifically the player's spaceship and the incoming asteroids.
* **Score System Creation & Data Persistence:** AI assisted in writing the scoring system mechanics, which includes tracking points during asteroid destruction, displaying the progress bar, and handling the personal best score logic (saving and loading from disk safely via custom exceptions).

## 3. AI-Generated Files and Regions

While the core game loop framework and SDL2 bindings rely on the provided project template, specific regions were generated or heavily assisted by AI:

### File: `TheAdventure/Program.cs`
* **Custom Exception Definition (Lines 6-10):** The `HighScoreStorageException` class was fully generated to satisfy the custom framework exception criteria.
* **Score Tracking & Persistence - Loading (Lines 52-69):** The logic involving `File.Exists`, `File.ReadAllText`, and the `try-catch` safety block that initializes the player's historical high score at launch.
* **Score Tracking & Persistence - Saving (Lines 207-224):** The end-game scoring system logic that evaluates if the current session score beats the previous record and writes the new highest score back to `highscore.txt`.