using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Repository.SQLite.System.Versions
{
    class Version_000_009_001_000 : IVersion
    {
        // worktaskの定義を大幅に変更
        // remark, plan/actualtime: 削除
        // source, importkey: 削除
        // ※SQLiteは列の削除に対応していないため、テーブルの作り直しが必要...

        public string CommandQuery => @"
ALTER TABLE
  worktasks
 RENAME TO 
  worktasks_bk;

CREATE TABLE worktasks
(
	id	INTEGER PRIMARY KEY AUTOINCREMENT,
	title	varchar(64),
	taskcategory	int,
	productid	int,
	clientId	int,
	processId	int,
    tasksource int
);

INSERT INTO 
  worktasks
(
  id
  , title 
  , taskcategory 
  , productid
  , clientId 
  , processId 
  , tasksource
) 
SELECT
  id
  , title 
  , taskcategory 
  , productid
  , clientId 
  , processId 
  , CASE
      WHEN importkey <> '' THEN 2
      WHEN istemporary = '1' THEN 1
      ELSE 0
    END
FROM 
  worktasks_bk;

ALTER TABLE
  importedtasks
ADD COLUMN
  worktaskid;

UPDATE
  importedtasks
SET
  worktaskid = (select id from worktasks_bk where worktasks_bk.importkey = importedtasks.importkey);

CREATE TABLE worktaskscompleted
(
	worktaskid	int PRIMARY KEY,
    registdatetime
);

INSERT INTO
  worktaskscompleted
(
  worktaskid,
  registdatetime
 )
SELECT
  id,
  actualenddatetime
FROM
  worktasks_bk
WHERE
  actualenddatetime IS NOT NULL;

";

        // 作業中タスクにもテーブルを用意する手法もありそうだったが、
        // オーバースペックな気がするので今回はなしで
        // https://flxy.jp/article/10813
        //CREATE TABLE worktasksinprogress
        //(
        //    worktaskid  int PRIMARY KEY,
        //    registdatetime
        //);

        //        INSERT INTO
        //  worktasksinprogress
        //(
        //  worktaskid,
        //  registdatetime
        // )
        //SELECT
        //  id,
        //  actualenddatetime
        //FROM
        //  worktasks_bk
        //WHERE
        //  actualenddatetime IS NULL;

        public string Version => "000.009.001.000";
    }
}
