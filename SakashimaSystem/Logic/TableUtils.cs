using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SakashimaSystem.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SakashimaSystem.Models;
using static SakashimaSystem.Entities.Kpt;
using static SakashimaSystem.Entities.UserEntity;

namespace SakashimaSystem.Logic
{
    public class TableUtils : ITableUtils
    {

        private static string PartitonKey = "PartitionKey";
        private static string RowKey = "RowKey";

        private static string TblKpt = "kpt";
        private static string TblBoard = "board";
        private static string TblUser = "user";
        private static string TblAction = "action";

        private readonly AppSettingsModel appSettingsModel;

        public TableUtils(IOptions<AppSettingsModel> options)
        {
            this.appSettingsModel = options.Value;
        }

        private CloudTableClient client;

        public CloudTableClient Client
        {
            get
            {
                if (client == null)
                {
                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(appSettingsModel.CloudStorage.ConnectionString);

                    client = storageAccount.CreateCloudTableClient();
                }
                return client;
            }
            private set
            {
                client = value;
            }
        }

        public string ETag { get; private set; }

        /// <summary>
        /// Kptリストを取得する
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <param name="type">Kptの種別</param>
        /// <returns>Kptリスト</returns>
        public List<Kpt> GetKptList(int boardId, KptType type)
        {
            CloudTable table = this.Client.GetTableReference(TblKpt);
            TableQuery<KptEntity> query = new TableQuery<KptEntity>().Where(TableQuery.GenerateFilterCondition(PartitonKey, QueryComparisons.Equal, this.GetKptTableKey(boardId, type)));
            TableContinuationToken continuationToken = null;
            var items = new List<KptEntity>(table.ExecuteQuerySegmentedAsync<KptEntity>(query, continuationToken).Result);
            var result = new List<Kpt>();
            foreach (var item in items)
            {
                var kpt = new Kpt()
                {
                    Type = type,
                    UserName = item.UserName,
                    Title = item.Title,
                    Comment = item.Comment
                };
                result.Add(kpt);
            }
            return result;
        }

        /// <summary>
        /// Kptを追加する
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <param name="type">Kpt種別</param>
        /// <param name="kptList">Kptのリスト</param>
        public async Task AddKptListAsync(int boardId, KptType type, List<Kpt> kptList)
        {
            CloudTable table = this.Client.GetTableReference(TblKpt);

            foreach (var kptItem in kptList)
            {
                var emp = new KptEntity(boardId, type, kptItem.UserName, kptItem.Title, kptItem.Comment);
                TableOperation insertOp = TableOperation.InsertOrReplace(emp);
                await table.ExecuteAsync(insertOp);
            }
        }

        /// <summary>
        /// Kptを削除する
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <param name="type">Kpt種別</param>
        /// <param name="kptItem">削除アイテム</param>
        public async Task DelKptAsync(int boardId, KptType type, Kpt kptItem)
        {
            CloudTable table = this.Client.GetTableReference(TblKpt);

            var emp = new KptEntity(boardId, type, kptItem.UserName, kptItem.Title, kptItem.Comment) { ETag = "*" };
            TableOperation deleteOp = TableOperation.Delete(emp);
            await table.ExecuteAsync(deleteOp);
        }

        /// <summary>
        /// Actionリストを取得する
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <param name="type">Kptの種別</param>
        /// <returns>Kptリスト</returns>
        public List<ActionItem> GetActionList(int boardId)
        {
            CloudTable table = this.Client.GetTableReference(TblAction);
            TableQuery<ActionEntity> query = new TableQuery<ActionEntity>().Where(TableQuery.GenerateFilterCondition(PartitonKey, QueryComparisons.Equal, boardId.ToString()));
            TableContinuationToken continuationToken = null;
            var items = new List<ActionEntity>(table.ExecuteQuerySegmentedAsync<ActionEntity>(query, continuationToken).Result);
            var result = new List<ActionItem>();
            foreach (var item in items)
            {
                var actionItem = new ActionItem()
                {
                    Title = item.Title,
                    Description = item.Description,
                    Assigned = item.Assigned,
                    Period = item.Period
                };
                result.Add(actionItem);
            }
            return result;
        }

        /// <summary>
        /// Actionを追加する
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <param name="actionList">Actionのリスト</param>
        public async Task AddActionListAsync(int boardId, List<ActionItem> actionList)
        {
            CloudTable table = this.Client.GetTableReference(TblAction);

            foreach (var actionItem in actionList)
            {
                var emp = new ActionEntity(boardId, actionItem.Title, actionItem.Description, actionItem.Assigned, actionItem.Period);
                TableOperation insertOp = TableOperation.InsertOrReplace(emp);
                await table.ExecuteAsync(insertOp);
            }
        }

        /// <summary>
        /// ボード名を取得する
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <returns>ボード名</returns>
        public string GetBoardName(int boardId)
        {
            var boardEntity = this.GetBoardEntity(boardId);
            return boardEntity.BoardName;
        }

        /// <summary>
        /// ボード一覧を取得する
        /// </summary>
        /// <returns></returns>
        public List<BoardEntity> GetBoardList()
        {
            CloudTable table = this.Client.GetTableReference(TblBoard);

            TableQuery<BoardEntity> query = new TableQuery<BoardEntity>().Where(TableQuery.GenerateFilterCondition(PartitonKey, QueryComparisons.Equal, PartitonKey));
            TableContinuationToken continuationToken = null;

            return table.ExecuteQuerySegmentedAsync<BoardEntity>(query, continuationToken).Result.Results;
        }

        /// <summary>
        /// ボード情報を更新する
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <param name="boardName">ボード名</param>
        /// <param name="isKeepDone">Keep完了フラグ</param>
        /// <param name="isProblemDone">Problem完了フラグ</param>
        /// <param name="isTryDone">Try完了フラグ</param>
        public async Task UpdateBoardAsync(int boardId, string boardName, bool isKeepDone, bool isProblemDone, bool isTryDone)
        {
            CloudTable table = this.Client.GetTableReference(TblBoard);

            // ボードテーブルをクリアする
            await this.ClearBoardTableAsync(boardId);

            // 新しいボード情報を追加する
            var emp = new BoardEntity(boardId, boardName, isKeepDone, isProblemDone, isTryDone);
            TableOperation insertOp = TableOperation.InsertOrReplace(emp);
            await table.ExecuteAsync(insertOp);
        }

        /// <summary>
        /// ボード名を変更する
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <param name="newName">新しいボード名</param>
        public async Task UpdateBoardNameAsync(int boardId, string newName)
        {
            var boardEntity = this.GetBoardEntity(boardId);

            // ボードテーブルをクリアする
            await this.ClearBoardTableAsync(boardId);

            // ボード名を変更する
            await UpdateBoardAsync(boardId, newName, boardEntity.IsKeepDone, boardEntity.IsProblemDone, boardEntity.IsTryDone);

        }

        /// <summary>
        /// ボードのKptの登録を締め切る
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <param name="userName">ユーザ名</param>
        /// <param name="type">Kpt種別</param>
        /// <param name="kptList">Kptリスト</param>
        public async Task CloseCommitKptListAsync(int boardId, string boardName, KptType type)
        {
            var boardEntity = this.GetBoardEntity(boardId);

            // ボードのステータスを更新する
            switch (type)
            {
                case KptType.Keep:
                    await UpdateBoardAsync(boardId, boardName, true, boardEntity.IsProblemDone, boardEntity.IsTryDone);
                    break;

                case KptType.Problem:
                    await UpdateBoardAsync(boardId, boardName, boardEntity.IsKeepDone, true, boardEntity.IsTryDone);
                    break;

                case KptType.Try:
                    await UpdateBoardAsync(boardId, boardName, boardEntity.IsKeepDone, boardEntity.IsProblemDone, true);
                    break;

                default:
                    break;
            }
        }
        /// <summary>
        /// ボードのKPTステータスが初期状態か否か
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <returns>True/初期状態、False/否</returns>
        public bool IsInitialStateBoardStatus(int boardId)
        {
            var targetBoard = this.GetBoardEntity(boardId);
            return (targetBoard.IsKeepDone == false) && (targetBoard.IsProblemDone == false) && (targetBoard.IsTryDone == false);
        }

        /// <summary>
        /// ボードのKPTステータスがKeep完了状態か否か
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <returns>True/Keep完了状態、False/否</returns>
        public bool IsKeepDoneBoardStatus(int boardId)
        {
            var targetBoard = this.GetBoardEntity(boardId);
            return targetBoard.IsKeepDone == true;
        }

        /// <summary>
        /// ボードのKPTステータスがProblem完了状態か否か
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <returns>True/Problem完了状態、False/否</returns>
        public bool IsProblemDoneBoardStatus(int boardId)
        {
            var targetBoard = this.GetBoardEntity(boardId);
            return (targetBoard.IsKeepDone == true) && (targetBoard.IsProblemDone == true);
        }

        /// <summary>
        /// ボードのKPTステータスがTry完了状態か否か
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <returns>True/Try完了状態、False/否</returns>
        public bool IsTryDoneBoardStatus(int boardId)
        {
            var targetBoard = this.GetBoardEntity(boardId);
            return (targetBoard.IsKeepDone == true) && (targetBoard.IsProblemDone == true) && (targetBoard.IsTryDone = true);
        }

        /// <summary>
        /// ボードを新規作成する
        /// </summary>
        /// <returns>ボードID</returns>
        public async Task<int> CreateBoardAsync()
        {
            CloudTable table = this.Client.GetTableReference(TblBoard);

            //ボードのRawKeyで一番大きい数字に1加えたものをボードIDとする
            var boardResults = this.GetBoardList();
            var boardId = boardResults.Count + 1;
            string boardName = string.Format("新しいKPTボード-{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));

            // 新しいボード情報を追加する
            var emp = new BoardEntity(boardId, boardName, false, false, false);
            TableOperation insertOp = TableOperation.InsertOrReplace(emp);
            await table.ExecuteAsync(insertOp);

            return boardId;
        }

        /// <summary>
        /// Kptボードを初期化する
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <param name="boardName">ボード名</param>
        public async Task InitializeKptBoardAsync(int boardId, string boardName)
        {
            // KPT テーブルをクリアする
            await this.ClearKptTableAsync(boardId, KptType.Keep);
            await this.ClearKptTableAsync(boardId, KptType.Problem);
            await this.ClearKptTableAsync(boardId, KptType.Try);

            // User テーブルをクリアする
            await this.ClearUserTableAsync(boardId);

            // Board テーブルのステータスを初期に戻す
            await UpdateBoardAsync(boardId, boardName, false, false, false);
        }

        /// <summary>
        /// ユーザ情報を追加/更新する
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <param name="userName">ユーザ名</param>
        /// <param name="status">Kptステータス</param>
        public async Task UpsertUserAsync(int boardId, string userName, UserStatusEnum status)
        {
            CloudTable table = this.Client.GetTableReference(TblUser);
            var emp = new UserEntity(boardId, userName, status);
            TableOperation insertOp = TableOperation.InsertOrReplace(emp);
            await table.ExecuteAsync(insertOp);
        }

        /// <summary>
        /// ユーザのKptの登録を完了する
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <param name="userName">ユーザ名</param>
        /// <param name="type">Kpt種別</param>
        /// <param name="kptList">Kptリスト</param>
        public async Task CommitKptListByUserAsync(int boardId, string userName, KptType type, List<Kpt> kptList)
        {
            // Kpt リストを登録する
            await this.AddKptListAsync(boardId, type, kptList);

            // ユーザステータスを更新する
            switch (type)
            {
                case KptType.Keep:
                    await UpsertUserAsync(boardId, userName, UserStatusEnum.KeepDone);
                    break;

                case KptType.Problem:
                    await UpsertUserAsync(boardId, userName, UserStatusEnum.ProblemDone);
                    break;

                case KptType.Try:
                    await UpsertUserAsync(boardId, userName, UserStatusEnum.TryDone);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// ユーザを追加する
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <param name="userName">ユーザ名</param>
        public async Task AddUserAsync(int boardId, string userName)
        {
            await this.UpsertUserAsync(boardId, userName, UserStatusEnum.Blank);
        }

        /// <summary>
        /// ユーザのKPTステータスを取得する
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <param name="userName">ユーザ名</param>
        /// <returns>ユーザのKPTステータス</returns>
        public UserStatusEnum GetUserStatus(int boardId, string userName)
        {
            CloudTable table = this.Client.GetTableReference(TblUser);

            TableQuery<UserEntity> query = new TableQuery<UserEntity>().Where(TableQuery.GenerateFilterCondition(PartitonKey, QueryComparisons.Equal, boardId.ToString()))
            .Where(TableQuery.GenerateFilterCondition(RowKey, QueryComparisons.Equal, userName));
            TableContinuationToken continuationToken = null;

            var userResults = table.ExecuteQuerySegmentedAsync<UserEntity>(query, continuationToken).Result.Results;
            if (userResults.Count == 0)
            {
                throw new InvalidOperationException(string.Format("Not Found UserName: {0}", userName));
            }

            var targetUser = userResults[0];
            return (UserStatusEnum)Enum.ToObject(typeof(UserStatusEnum), targetUser.UserStatus);
        }

        /// <summary>
        /// BoardEntityを取得する
        /// </summary>
        /// <param name="boardName">ボードID</param>
        /// <returns>BoardEntity</returns>
        private BoardEntity GetBoardEntity(int boardId)
        {
            CloudTable table = this.Client.GetTableReference(TblBoard);

            TableQuery<BoardEntity> query = new TableQuery<BoardEntity>().Where(TableQuery.GenerateFilterCondition(PartitonKey, QueryComparisons.Equal, PartitonKey))
            .Where(TableQuery.GenerateFilterCondition(RowKey, QueryComparisons.Equal, boardId.ToString()));
            TableContinuationToken continuationToken = null;

            var boardResults = table.ExecuteQuerySegmentedAsync<BoardEntity>(query, continuationToken).Result.Results;
            if (boardResults.Count == 0)
            {
                throw new InvalidOperationException(string.Format("Not Found BoardId: {0}", boardId));
            }

            return boardResults[0];
        }

        /// <summary>
        /// Boardテーブルの内容をクリアする
        /// </summary>
        /// <param name="boardId">ボードID</param>
        public async Task ClearBoardTableAsync(int boardId)
        {
            CloudTable table = this.Client.GetTableReference(TblBoard);
            TableQuery<BoardEntity> getQuery = new TableQuery<BoardEntity>().Where(TableQuery.GenerateFilterCondition(PartitonKey, QueryComparisons.Equal, PartitonKey))
            .Where(TableQuery.GenerateFilterCondition(RowKey, QueryComparisons.Equal, boardId.ToString()));
            TableContinuationToken continuationToken = null;
            var items = new List<BoardEntity>(table.ExecuteQuerySegmentedAsync<BoardEntity>(getQuery, continuationToken).Result);
            foreach (var item in items)
            {
                TableOperation deleteOperation = TableOperation.Delete(item);
                await table.ExecuteAsync(deleteOperation);
            }
        }

        /// <summary>
        /// Userテーブルの内容をクリアする
        /// </summary>
        /// <param name="boardId">ボードID</param>
        public async Task ClearUserTableAsync(int boardId)
        {
            CloudTable table = this.Client.GetTableReference(TblUser);
            TableQuery<UserEntity> getQuery = new TableQuery<UserEntity>().Where(TableQuery.GenerateFilterCondition(PartitonKey, QueryComparisons.Equal, boardId.ToString()));
            TableContinuationToken continuationToken = null;
            var items = new List<UserEntity>(table.ExecuteQuerySegmentedAsync<UserEntity>(getQuery, continuationToken).Result);
            foreach (var item in items)
            {
                TableOperation deleteOperation = TableOperation.Delete(item);
                await table.ExecuteAsync(deleteOperation);
            }
        }

        /// <summary>
        /// Kptテーブルの内容をクリアする
        /// </summary>
        /// <param name="type">Kpt種別</param>
        public async Task ClearKptTableAsync(int boardId, KptType type)
        {
            CloudTable table = this.Client.GetTableReference(TblKpt);
            TableQuery<KptEntity> getQuery = new TableQuery<KptEntity>().Where(TableQuery.GenerateFilterCondition(PartitonKey, QueryComparisons.Equal, this.GetKptTableKey(boardId, type)));
            TableContinuationToken continuationToken = null;
            var items = new List<KptEntity>(table.ExecuteQuerySegmentedAsync<KptEntity>(getQuery, continuationToken).Result);
            foreach (var item in items)
            {
                TableOperation deleteOperation = TableOperation.Delete(item);
                await table.ExecuteAsync(deleteOperation);
            }
        }

        /// <summary>
        /// KptテーブルKeyを取得する
        /// </summary>
        /// <param name="boardId">ボードID</param>
        /// <param name="type">Kpt種別</param>
        /// <returns></returns>
        private string GetKptTableKey(int boardId, KptType type)
        {
            return string.Format("{0}_{1}", boardId.ToString(), Enum.GetName(typeof(KptType), type));
        }

    }
}
