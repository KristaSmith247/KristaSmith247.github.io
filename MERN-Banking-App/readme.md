The purpose of this project is to demonstrate the use of React, Node.js, Express and MongoDB to create a website. This was a group project done for a software engineering class with @ashiggles and @anniecroft using the name "Team Gecko". The project is a mock banking website. Some of the technical challenges of this project included establishing communications between the client and server sides and creating events for when the user hits "submit".
Overall features of the website include:
+ login page
  - account verification
+ user account page
  - each account is associated with an id
  - accounts have different options depending on user's credentials
    * Administrators may change the rights associated with each account, access any page, create accounts, and have an account of their own
    * Employees may view customer accounts, create accounts, and have their own account
    * Customers may access only their own accounts
+ transaction history
  - users may view the history of their checking, savings, and deposit accounts
  - history includes a date and description of transaction
+ list of all accounts
  - only accessible to employees and administrators, displays information about all accounts in database

My contributions include:
+ created the login page
+ used sha-256 to hash passwords (no salting)
+ contributed to the css page
+ added error checking on the account creation page
  - accounts are created with an accepted role, email, password, and an associated first and last name
+ added in error checking on the account editing page
  - user must enter the same password twice

![Screenshot 2024-08-14 112345](https://github.com/user-attachments/assets/ccbdc9df-9e4c-4891-a6c9-b8482b4d90b2)
![Screenshot 2024-08-14 112403](https://github.com/user-attachments/assets/7612dd74-14a6-4f81-b4ea-588e191f189f)
![Screenshot 2024-08-14 112509](https://github.com/user-attachments/assets/c45e5a2d-9d92-4d58-ab69-7bd76cf9ade6)
![Screenshot 2024-08-14 112601](https://github.com/user-attachments/assets/99f2ad8f-305b-4867-ac1e-8dc15a0d3a5c)
![Screenshot 2024-08-14 112552](https://github.com/user-attachments/assets/cb7fa5e7-4b74-45a8-987d-3d3d59dfe5a3)

