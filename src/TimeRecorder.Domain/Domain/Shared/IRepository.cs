using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Domain.Domain
{
    // ・「集約」の境界線はドメインモデルの設計上非常に重要
    // ・境界線はユースケース毎に変わるものではない
    // →リポジトリのIFはユースケース層ではなく、ドメイン層に定義する

    /// <summary>
    /// Repositoryであることを表すマーカIFです
    /// </summary>
    public interface IRepository
    {
    }
}
