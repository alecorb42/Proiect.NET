## Game Description
Space Shooter is an arcade-style action game where the player controls a defensive spaceship at the bottom of the screen. 

### Controls & Movement
* **Movement:** The player can move the spaceship dynamically to the left or right based on their preference, using either the **Left/Right Arrow keys** or the **A / D keys**.
* **Combat:** To fire laser beams and eliminate incoming asteroids, the player must press the **Spacebar**. 
* **Mechanics:** The ship is fully capable of continuous movement while simultaneously shooting lasers, allowing for fluid and responsive gameplay.

### Win & Lose Conditions
* **How to Win:** The player wins the game by successfully filling up the progress bar (achieving a high enough score by destroying asteroids) before the time runs out.
* **How to Lose:** The game ends in a defeat under two conditions:
  1. **Time's Up:** If the 60-second countdown timer reaches zero before the progress bar is filled.
  2. **Collision:** If an incoming asteroid hits the player's spaceship, causing an immediate game over.

## Build & Run Instructions
1. Navigate to the project root folder in your terminal.
2. Execute the following command to restore packages and run the game:
   ```bash
   dotnet run

## AI Usage Summary
AI (Gemini) was utilized to assist in designing the character rendering properties for the spaceship and asteroids, implementing the scoring system mechanics, and creating the high score data persistence layer with custom exception handling. The comprehensive, line-by-line disclosure is fully documented in the `AI_USAGE.md` file located at the repository root.