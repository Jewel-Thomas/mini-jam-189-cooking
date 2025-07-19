// PLAN: Mini Jam 189 - Cooking Game
// ---------------------------------
// Game Concept:
// - 2D Unity game for a 1-day game jam.
// - Player is a chef. The boss/enemy is a grumpy, cruel clock named Mr. Clock.
// - Mr. Clock throws ingredients at the chef (player).
// - Chef must use specific ingredients to make a dish for Mr. Clock within a time limit.
// - If the chef succeeds, the clock moves forward by 1 hour (closer to end of day).
// - If the chef fails, the clock moves back by 1 hour (further from end of day).
//
// Core Loop:
// 1. Mr. Clock announces a dish (randomly selected from a list).
// 2. Mr. Clock throws ingredients (some needed, some not) at the chef.
// 3. Chef/player must catch and use correct ingredients to assemble the dish before time runs out.
// 4. If dish is completed correctly and on time:
//    - Clock moves forward 1 hour.
//    - Progress toward end of shift.
// 5. If dish is not completed or is incorrect:
//    - Clock moves back 1 hour.
//    - Player loses progress.
// 6. Repeat until end of shift (e.g., 8 hours) or time runs out.
//
// Key Features to Implement:
// - Mr. Clock character (sprite, animations, dialogue, ingredient throwing logic)
// - Chef/player character (sprite, movement, catching mechanic, assembling dishes)
// - Ingredient system (types, spawning, catching, using)
// - Dish recipes (list of dishes, required ingredients)
// - Timer/clock system (visual clock, hour tracking, win/lose conditions)
// - UI (current dish, timer, clock, feedback)
// - SFX/music (optional, if time allows)
//
// Questions/Clarifications Needed:
// - How does the chef assemble a dish? Drag & drop mechanic.
// - How are ingredients thrown? Random pattern.
// - How many different dishes/recipes do we want? Start with 5 dishes, made from 10 different ingredients.
// - Any power-ups or special events? None for now.
// - What is the target platform? PC.
//
// Next Steps:
// 1. Decide on core mechanics for catching/assembling.
// 2. List out 3-5 simple dishes and their required ingredients.
// 3. Create placeholder sprites for chef, clock, ingredients.
// 4. Prototype ingredient throwing and catching.
// 5. Implement basic clock/timer system.
// 6. Add win/lose conditions.
// 7. Polish visuals and add juice if time allows.
//
// (Update this plan as we go!)
