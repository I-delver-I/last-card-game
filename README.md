# kursach
Implementation of Last Card game on Unity

This game has some troubles at its end since it's my first one and I wasn't familiar enough with OOP principles and best world practices during the time of creation

# Game rules
The field is set, on which the program generates players who will make moves, which are accompanied by placing cards in the counter and taking them from the deck.
- The number of cards in the deck is 52
- The number of players must be no less than 2 and no more than 6
- The number of cards per hand must be no less than 4 and no more than 8
- The goal of the game is to score the least number of points

After the cards are dealt to the players and the top card from the deck is placed on the pile, a random player is searched to give him the first turn. The turn is then passed to the next player clockwise, who must place a card of the same value or suit. If a player does not have a card for a turn, then he takes one card from the stock and passes the turn to the next player clockwise. Some of the cards require the presence of appropriate actions from the players.

| Value | Action |
|:----------:|----------|
| 2 | The next player must take a card from the stock cards and pass the turn |
| 3 | If the player lays a three, then he can lay another card of his choice on it |
| 4 | Each subsequent player clockwise must place a card of the same suit, but one value older (up to and including 10). For example, the next player must put 5 of the same suit, and the next player - put 6 of the same suit, etc. If any player does not have such a card, then he must take from the reserve cards an amount equivalent to the value of the top card of the rejection |
| 5, 6, 7 | It is allowed to put a four |
| 8 | It is allowed to bet on any card |
| Jack | The next player clockwise skips his turn |
| Ace | The direction of the game changes to the opposite |

In this way, the game continues until one player without cards appears. Players who are left with cards in their hands count penalty points for them. Card value in penalty points: king is worth 13 points, queen – 12, jack – 11, ace – 15 points, 8 – 25 points; all others - according to their meaning. Any player who exceeds a certain number of points at the time of scoring is eliminated from the game. The game will continue until the last player is left on the field.

# Gameplay
### Main window
![image](https://github.com/Scarmaing-Whebrolted/last-card-game/assets/89275215/fa3a5712-ca6b-42c2-ba99-1a9287cf66e1)

### Game field
![image](https://github.com/Scarmaing-Whebrolted/last-card-game/assets/89275215/0f423a81-45e1-47e6-989e-e82522d6ee62)

### Leaderboard
![image](https://github.com/Scarmaing-Whebrolted/last-card-game/assets/89275215/49c2d589-f14d-40b4-ad3d-8a8480c2b8e2)
