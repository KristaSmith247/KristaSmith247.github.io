
### [Hangman Game](https://github.com/KristaSmith247/KristaSmith247.github.io/tree/main/MERN-Hangman)

The purpose of this project was to demonstrate the use of React, Node.js, Express and MongoDB via a game of hangman. The project was worked on with @ashiggles as part of a software engineering class. Some of the technical challenges I faced with this project included keeping a list of guessed letters, checking for a win condition, and displaying a high scores table which contains scores for words of the same lengths (which required the use of sessions).
Features of the game include:
+ an index page which asks the user for their name
  - the name is stored in a session and put into the high scores table if applicable
+ a hangman game
  - displays number of letters in a word
  - displays incorrect guesses
  - informs user if they have already chosen a letter
  - informs user of the word at the end of the game
+ a high scores page
  - uses information from a database to display previous game information
    * displays entered username, number of guesses, and word length
+ allows user to play again

My contributions include:
+ creation of the frontend
  - game mechanics including
    * keeping a list of incorrect guesses
    * displaying hangman drawing
    * adding another part for each incorrect guess
    * retrieving a random word for each round
+ some backend additions
  - creating a connection to a database

![Screenshot 2024-08-14 103850](https://github.com/user-attachments/assets/b5d0d5ce-efee-4ef7-a2c7-38a06c65dcbd)
![Screenshot 2024-08-14 103842](https://github.com/user-attachments/assets/6ae1d389-3670-409d-b432-81c64f101cd5)

