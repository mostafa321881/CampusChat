# CampusChat — Design

## 1. Introduction

CampusChat is a web application where students can create profiles, join discussion threads, and send direct messages.

The project is developed using **C#, .NET 8, and Blazor Interactive Server**.

## 2. Project Structure

CampusChat is divided into three main parts:

- **Models:** Store information about students, threads, posts, and messages.
- **Services:** Handle application logic, data, and permissions.
- **Components:** Display pages and allow students to interact with the application.

## 3. Main Features

- **Student Profiles:** Students can create, view, and edit their profiles.
- **Discussion Threads:** Students can create public or private threads, join them, and write posts and replies.
- **Direct Chats:** Two students can send private messages to each other.
- **Unread Counts:** Students can see unread posts in their joined threads.
- **Live Updates:** New posts and messages appear automatically without refreshing the browser.

## 4. Security and Permissions

Permissions are checked in the services.

Private threads require a secret code to join. Students can delete only their own posts.

The auditor can read private threads and direct chats, but cannot send messages in direct chats unless they are a participant.

## 5. Data Storage

CampusChat uses in-memory storage through services.

Data remains available during browser refreshes while the server is running, but is lost when the server restarts.

## 6. Design Choices

I used Blazor because it allows building interactive web pages with C#.

I separated models, services, and components to make the code easier to understand, maintain, and test.

## 7. Conclusion

CampusChat is a simple student communication application that demonstrates Blazor, service-based architecture, permissions, and live updates.