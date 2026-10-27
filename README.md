# CampusChat

CampusChat is a web application developed for Arbeidskrav 2. The application allows students to create profiles, participate in discussion threads, and communicate through direct chats.

## Technology

- C# and .NET 8
- Blazor Web App with Interactive Server rendering
- Bootstrap for styling
- Dependency injection and service-based application logic
- ProtectedSessionStorage for restoring the selected student after a browser refresh

## Requirements

- .NET 8 SDK
- A modern web browser
- JetBrains Rider, Visual Studio, or another compatible development environment

## How to Run

1. Clone or download the CampusChat repository.
2. Open the project folder in a terminal.
3. Restore dependencies:

   ```bash
   dotnet restore
   ```

4. Build the project:

   ```bash
   dotnet build
   ```

5. Start the application:

   ```bash
   dotnet run
   ```

6. Open the local address displayed in the terminal.

## Student Login

CampusChat uses a simplified student-selection system instead of passwords.

The application includes three example students:

- Mostafa — student
- Sara — student
- Ahmed — auditor

Select a student on the Login page to use the application. The selected student is stored using ProtectedSessionStorage so that the selection can be restored after refreshing the browser.

To test communication between students, open CampusChat in separate browser sessions and select different students.

## Features

### Student Profiles

Students can:

- View student profiles.
- Create new profiles.
- Edit their own profiles.
- Set a display name, biography, study year, and avatar color.
- View threads they have joined.

Each profile has its own URL.

### Discussion Threads

Students can:

- Browse and search discussion threads.
- Create public or private threads.
- Join and leave threads.
- Join private threads using a secret code.
- View their joined threads on the My Threads page.

Private-thread reading permissions are checked in the service layer.

### Posts and Replies

Students can:

- Write posts in discussion threads.
- Reply to existing posts.
- Delete their own posts.
- See the author and creation time of each post.

Empty posts are rejected. Posts and replies are managed through the PostService.

### Unread Counts and Live Updates

The My Threads page displays unread post counts for joined threads.

- Posts written by other students increase the unread count.
- Opening a joined thread marks its current posts as read.
- New posts and replies appear automatically in open discussions.
- Unread counts update automatically without manually refreshing the browser.

### Direct Chats

Students can create direct conversations with other students.

- Only conversation participants can send messages.
- Only participants and auditors can read a conversation.
- Auditors may read conversations they do not participate in, but cannot send messages in those conversations.
- Messages are displayed newest first.
- New messages appear automatically without refreshing the browser.

Each direct chat has its own URL.

## Application Structure

- `Models/` — data models for students, threads, posts, and direct chats.
- `Services/` — business logic, permissions, and application data.
- `Components/Pages/` — application pages.
- `Components/Profiles/` — student profile pages.
- `Components/Layout/` — navigation and layout.

## Data Storage

CampusChat currently stores application data in memory using services registered with dependency injection.

Data remains available during normal browser refreshes while the application server is running. However, changes are not permanently saved when the server process stops or restarts.

The selected student is separately stored in browser session storage.

## Testing

The application has been manually tested with different student accounts in separate browser sessions.

Tests include:

- Creating and joining public and private threads.
- Writing, replying to, and deleting posts.
- Viewing unread counts and marking posts as read.
- Receiving new posts and messages without manually refreshing.
- Sending direct messages between students.
- Verifying that an auditor can read other students' direct chats without participating.
- Building the application using `dotnet build`.

## Additional Documentation

- `design.md` — architecture and design decisions.
- `safety.md` — access control, validation, and security considerations.
- `ai.md` — use of AI assistance during development.

## Repository

https://github.com/mostafa321881/CampusChat