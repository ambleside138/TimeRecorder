# TimeRecorder
工数管理アプリ

詳細な使い方はQiitaに記載しています　→ [Qiitaの記事を表示](https://qiita.com/ambleside138/items/e05e6d1fad91dd3c05c0)

![capture_main](https://user-images.githubusercontent.com/12669997/86535106-c6951200-bf18-11ea-8745-e953097e8e46.png)

### マスタメンテナンスについて

タスク入力項目のうち、
- 製品
- 作業工程
- ユーザ
についてはDBでのマスタ管理となっています。

マスタ管理用のUIが用意できていないため、DBを直接編集してください。

|項目|対応テーブル名|
|:---:|:---:|
|製品|products|
|作業工程|processes|
|ユーザ|clients|

SQLiteの編集は下記のツールが便利です

https://sqlitebrowser.org/
