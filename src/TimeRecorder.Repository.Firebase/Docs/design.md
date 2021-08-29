# RootCollection
## users
◆ documentId: authentication.uid
- name: string
- createdAt: datetime
- updatedAt: datetime
- signinedAt: datetime　＃最終ログイン時間

### todos
◆ documentId: authentication.uid ＃owner
- userId: string / authentication.uid ＃owner
  - ▽ subcollection: todoItems
    - documentId: auto
    - title: string
    - memo: string
    - dueDate: datetime
    - alertDateTime: datetime
    - todayTaskDate: string
    - belongingTaskListId: tasklists.documentId
    - done: bool
    - createdAt: datetime
    - updatedAt: datetime
  - ▽ completedTodoItems

### archivedTodos
◆ documentId: authentication.uid ＃owner
  - userId
  - todos: subcollection ＃same as tasks.

### tasklists
◆ documentId: authentication.uid ＃owner
- userId: authentication.uid ＃owner
  - ▽ subcollection
  - title: string
  - iconCharacter: string
  - createdAt: datetime
  - updatedAt: datetime



