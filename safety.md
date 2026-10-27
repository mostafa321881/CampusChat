# CampusChat — Safety and Security

## 1. Access Control

CampusChat checks permissions in its services, not only in the user interface.

- Students can edit only their own profiles.
- Private threads require a secret code to join.
- Students cannot read private threads unless they are members.
- Students can delete only their own posts.
- Direct chats can be accessed only by participants and auditors.

## 2. Auditor Permissions

Ahmed is the auditor.

- He can read all private threads.
- He can read direct chats between other students.
- He cannot send messages in direct chats unless he is a participant.
- He must join private threads before posting.

## 3. Input Validation

CampusChat rejects empty posts and messages.

Posts and replies are checked for valid permissions. Blazor also encodes ordinary displayed text to help prevent HTML and JavaScript injection.

## 4. Student Login

Students select an account instead of using a password.

`ProtectedSessionStorage` remembers the selected student after a browser refresh.

**Limitation:** This is not secure authentication because users can select another student's account.

## 5. Data Storage

CampusChat stores data in memory.

Data survives browser refreshes while the server is running, but changes are lost when the server restarts.

## 6. Testing

We manually tested:

- Private-thread access.
- Post creation and deletion.
- Auditor permissions.
- Direct-chat privacy.
- Unread counts and live updates.

## 7. Conclusion

CampusChat implements basic permission checks and input validation. However, it is an educational project, not a production-ready secure messaging system.