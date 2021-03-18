
WSUserAccountManager app
- This is an ASP.NET Core Web API project to process API messages for the User Registration and Authentication, Session Management and Verification Code System.

It uses the following:
- Low-level Websocket protocol 
- .Net Core Middleware, DependencyInjection
- Entity Framework Core (currenlty in InMemoryDatabase)
- AutoMapper
- Linq
- NewtonJson

HashFunction app 
- Performs hashing algorithm to encrypt sensitive information like password and salt.

It uses the following:
- jsSHA to perform hashing algorithm
- yargs to enable inline arguments


To run the project, open the WSUserAccountManager.sln and run the WSUserAccountManager.csproj.
It can be achieved thru cmd/terminal by running the following command:
dotnet run WSUserAccountManager

To test the API:

1) Add the WebSocket Client Chrome extension using this link and run the extension.
- https://chrome.google.com/webstore/detail/smart-websocket-client/omalebghpgejjiaoknljcfmglgbpocdp

2) Put wss://localhost:5001/user in the WebSocket Address and click Connect.

API Messages
Described below are the API messages supported by the WSUserAccountManager app.

For Registration:

Input:
  
    {
     "command": "register",
     "username": "johndoe",
     "displayName": "Bigjohndoe",
     "password": "8437ae0231129d7038809d7aa68e89430b73e245b99b9cc662cbc0bd9cc6f6da",
     "password2": "8437ae0231129d7038809d7aa68e894902345bde25ad0fb662cbc0bd9cc6f6da",
     "email": "john.doe@mail.com",
     "verificationCode": "123456"
    }


Output:
      
    {
      "command": "register",
      "username": "johndoe",
      "success": true
    }

For Checking Username Availability:

Input:

    {
        "command": "checkUsername",
        "username": "johndoe"
    }
    
Output:

    {
        "command": "checkUsername",
        "email": "johndoe",
        "available": true
    }

For Checking Email Availability:

Input:

    {
        "command": "checkEmail",
        "email": "john.doe@mail.com"
    }
    
Output:

    {
        "command": "checkEmail",
        "email": "john.doe@mail.com",
        "available": true
    }

For Requesting Login Salt:

Input:

    {
        "command": "loginSalt",
        "username": "johndoe"
    }
    
Output:

    {
        "command": "loginSalt",
        "username": "johndoe",
        "validity": 300,
        "salt": "cbb4a64006378ec261840d39ab6cc76048f3dad16e19b7db508fb11ba4594c51"
    }

For Performing Login:

Input:

    {
        "command": "login",
        "usernameOrEmail": "johndoe",
        "challenge": "02b78364fee0f76cdfb64c17b6a919b2940198742cb3f989af9271a81e9471c8"
    }

Output:

Successful login:

    {
        "command": "login",
        "username": "johndoe",
        "success": true,
        "sessionID": "4a339896dfe936f2372d95bf2046871e56809a0b3ccae9d7f187418039971671",
        "userID": "buifu0812ehudasudg790382rh",
        "validity": 300
    }

Failed login:

    {
        "command": "login",
        "username": "johndoe",
        "success": false,
        "sessionID": null,
        "validity": 0
    }

For the Registration and Login functionality, below steps are prerequisite to obtain other needed information:
- To get the password and password2 values, perform the following instructions:

1) Get the hashed password value by running the HashFunction app. 

	1.1) To run the HashFunction app, install nodejs.
	
	1.2) Open the HashFunction folder in the terminal and run npm install to install dependencies
	
	1.3) Perform the following:
	
	![image](https://user-images.githubusercontent.com/68279185/111518262-df84e100-8790-11eb-9e97-5dc01efb5689.png)

	InitialHashValue: secretKey = choice of password, plain text; message = username
  
	Password/Password2: secretKey = 'superSecretKey', plain text; message = InitialHashValue
		
- To get the challenge value for login, perform the following instructions:

1) Run the HashFunction app again perform above 1.1 to 1.3 steps with the following values:

	Challenge: secretKey = salt value from the Login Salt output; message = Password/Password2 value
  
	![image](https://user-images.githubusercontent.com/68279185/111518300-e7448580-8790-11eb-870d-eef5c1b0adba.png)






