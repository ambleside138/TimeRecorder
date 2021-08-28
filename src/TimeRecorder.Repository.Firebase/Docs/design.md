### rootCollection
- users
  - documentId: authentication.uid
  - name: string
  - createdAt: datetime
  - updatedAt: datetime
  - signinedAt: datetime　＃最終ログイン時間
  - tasks: subcollection
  - tasklists: subcollection


### subCollection
- tasks @users
  - createdAt: datetime
  - updatedAt: datetime

- tasklists @users
  - createdAt: datetime
  - updatedAt: datetime