# SO-SignalR

## About this project
This repo is an example of integrating an [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-2.2) backend with [SignalR](https://docs.microsoft.com/en-us/aspnet/core/signalr/introduction?view=aspnetcore-2.2) and a [VueJS](https://vuejs.org/) frontend.

It serves as an example of how VueJS can be integrated with SignalR by implementing a minimalistic version of Stack Overflow. SignalR is then used to propagate the changes to the question score and new questions, which serve as examples for receiving and sending events through the SignalR connection.

## Quick Start
1. Clone this repo.
1. Open a terminal and navigate to the client folder. Restore the dependencies with `npm install`, then run `npm run serve`. It will automatically reload when the client code changes.
1. Open a second terminal and navigate to the server folder. Restore the dependencies with `dotnet restore`, then run the server with `dotnet run`. Alternatively use `dotnet watch run` if you want to automatically reload when the server code changes.
1. The frontend will be listening on http://localhost:8080 while the backend will be listening at http://localhost:5100

## Backend
The backed is an ASP.NET Core 2.2 project providing a REST API. It was initialized using the [.NET Core CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/?tabs=netcore2x) with `dotnet new webapi` and provides a simple API for viewing and adding the questions and answers of the site.

On top of that:
- it provides a SignalR hub where clients can connect so they receive an event when question scores change and new questions are added.
- allows clients authenticating using either of a cookie based schema or a jwt bearer schema

## Frontend
The frontend is a Vue 2.5 project. It was initialized using the [Vue-CLI 3](https://cli.vuejs.org/) with `vue create` and provides a minimalistic Stack Overflow site on top of the API provided by the backend, with Bootstrap used for styling.

The [SignalR JavaScript client](https://docs.microsoft.com/en-us/aspnet/core/signalr/javascript-client?view=aspnetcore-2.2) was installed and is used to intialize a connection with the backend SignalR hub during app startup. A simple Vue plugin is provided so any component can receive SignalR events from the server as well as submit events to the server.

[Vuex](https://vuex.vuejs.org/) is used to manage the state related with the user context. It provides methods to login/logout using either of the cookie or jwt bearer schemas, as well as centrally storing the current user profile.

