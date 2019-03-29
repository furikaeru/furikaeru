using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using SakashimaSystem.Entities;

namespace SakashimaSystem.Logic
{
    public interface ITableUtils
    {
        CloudTableClient Client { get; }
        string ETag { get; }

        /// <summary>
        /// Kptリストを取得する
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <param name="type">Kptの種別</param>
        /// <returns>Kptリスト</returns>
        List<Kpt> GetKptList(int boardId, Kpt.KptType type);

        /// <summary>
        /// Kptを追加する
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <param name="type">Kpt種別</param>
        /// <param name="kptList">Kptのリスト</param>
        Task AddKptListAsync(int boardId, Kpt.KptType type, List<Kpt> kptList);

        /// <summary>
        /// Kptを削除する
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <param name="type">Kpt種別</param>
        /// <param name="kptItem">削除アイテム</param>
        Task DelKptAsync(int boardId, Kpt.KptType type, Kpt kptItem);

        /// <summary>
        /// Actionリストを取得する
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <param name="type">Kptの種別</param>
        /// <returns>Kptリスト</returns>
        List<ActionItem> GetActionList(int boardId);

        /// <summary>
        /// Actionを追加する
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <param name="actionList">Actionのリスト</param>
        Task AddActionListAsync(int boardId, List<ActionItem> actionList);

        /// <summary>
        /// ボード名を取得する
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <returns>ボード名</returns>
        string GetBoardName(int boardId);

        /// <summary>
        /// ボード一覧を取得する
        /// </summary>
        /// <returns></returns>
        List<BoardEntity> GetBoardList();

        /// <summary>
        /// ボード情報を更新する
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <param name="boardName">ボード名</param>
        /// <param name="isKeepDone">Keep完了フラグ</param>
        /// <param name="isProblemDone">Problem完了フラグ</param>
        /// <param name="isTryDone">Try完了フラグ</param>
        Task UpdateBoardAsync(int boardId, string boardName, bool isKeepDone, bool isProblemDone, bool isTryDone);

        /// <summary>
        /// ボード名を変更する
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <param name="newName">新しいボード名</param>
        Task UpdateBoardNameAsync(int boardId, string newName);

        /// <summary>
        /// ボードのKptの登録を締め切る
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <param name="userName">ユーザ名</param>
        /// <param name="type">Kpt種別</param>
        /// <param name="kptList">Kptリスト</param>
        Task CloseCommitKptListAsync(int boardId, string boardName, Kpt.KptType type);

        /// <summary>
        /// ボードのKPTステータスが初期状態か否か
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <returns>True/初期状態、False/否</returns>
        bool IsInitialStateBoardStatus(int boardId);

        /// <summary>
        /// ボードのKPTステータスがKeep完了状態か否か
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <returns>True/Keep完了状態、False/否</returns>
        bool IsKeepDoneBoardStatus(int boardId);

        /// <summary>
        /// ボードのKPTステータスがProblem完了状態か否か
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <returns>True/Problem完了状態、False/否</returns>
        bool IsProblemDoneBoardStatus(int boardId);

        /// <summary>
        /// ボードのKPTステータスがTry完了状態か否か
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <returns>True/Try完了状態、False/否</returns>
        bool IsTryDoneBoardStatus(int boardId);

        /// <summary>
        /// ボードを新規作成する
        /// </summary>
        /// <returns>ボードID</returns>
        Task<int> CreateBoardAsync();

        /// <summary>
        /// Kptボードを初期化する
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <param name="boardName">ボード名</param>
        Task InitializeKptBoardAsync(int boardId, string boardName);

        /// <summary>
        /// ユーザ情報を追加/更新する
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <param name="userName">ユーザ名</param>
        /// <param name="status">Kptステータス</param>
        Task UpsertUserAsync(int boardId, string userName, UserEntity.UserStatusEnum status);

        /// <summary>
        /// ユーザのKptの登録を完了する
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <param name="userName">ユーザ名</param>
        /// <param name="type">Kpt種別</param>
        /// <param name="kptList">Kptリスト</param>
        Task CommitKptListByUserAsync(int boardId, string userName, Kpt.KptType type, List<Kpt> kptList);

        /// <summary>
        /// ユーザを追加する
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <param name="userName">ユーザ名</param>
        Task AddUserAsync(int boardId, string userName);

        /// <summary>
        /// ユーザのKPTステータスを取得する
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <param name="userName">ユーザ名</param>
        /// <returns>ユーザのKPTステータス</returns>
        UserEntity.UserStatusEnum GetUserStatus(int boardId, string userName);

        /// <summary>
        /// Boardテーブルの内容をクリアする
        /// </summary>
        /// <param name="boardId">ボードID</param>
        Task ClearBoardTableAsync(int boardId);

        /// <summary>
        /// Userテーブルの内容をクリアする
        /// </summary>
        /// <param name="boardId">ボードID</param>
        Task ClearUserTableAsync(int boardId);

        /// <summary>
        /// Kptテーブルの内容をクリアする
        /// </summary>
        /// <param name="type">Kpt種別</param>
        Task ClearKptTableAsync(int boardId, Kpt.KptType type);
    }
}