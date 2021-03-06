# Post Office Web App

## About
Used technologies:
* Client app: Angular 10, bootstrap, ng-bootstrap
* Backend: .NET Core 3.1, C#

## Run dev app
1. Clone the repository
    `git clone https://gitlab.cs.ttu.ee/tekald/helmes-test-assessment.git`
2. To run the backend service type into command line: 
    * `cd helmes-test-assessment`
    * `dotnet run -p TestAssessment/WebApp`
    
    Backend service is now running on `https://localhost:5001/`.
    
3. To run the client app type into command line:
    * `cd post-office-client`
    * `npm install`
    * `npm start --open`
    Then open `http://localhost:4200/`.
    The backend service must be running for client app to work properly.
    
    *Backend service is running on `https://localhost:5001/` by default. 
        If you are running it on another address, you have to chane the 'backendUrl' variable in environment.json file.*

## Swagger
Backend API documentation is set up in Swagger. 
You can access the Swagger by clicking on the "API Documentation" link in client app or opening "https://localhost:5001/swagger".

-------------------------------------------------------------------
Teele Kaldaru 2020