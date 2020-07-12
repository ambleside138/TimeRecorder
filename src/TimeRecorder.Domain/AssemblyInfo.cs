// internalなクラスをテストできるようにする
// ファイル名はなんでもいいらしい
// https://www.nuits.jp/entry/net-standard-internals-visible-to

using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("TimeRecorder.Domain.Test")]