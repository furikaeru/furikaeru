using System.Collections.Generic;
using SakashimaSystem.Entities;
using SakashimaSystem.Models.Kpt;

namespace SakashimaSystem.Logic
{
    public interface IKptListLogic
    {
        /// <summary>
        /// モデルを取得
        /// </summary>
        /// <returns></returns>
        KptListModel GetModel(int boardId, Kpt.KptType type);

        /// <summary>
        /// モデルを取得(ユーザーで絞込）
        /// </summary>
        /// <returns></returns>
        KptListModel GetUserModel(int boardId, Kpt.KptType type, string username);

        /// <summary>
        /// KPTリストを取得(ユーザーで絞込）
        /// </summary>
        /// <returns></returns>
        IList<Kpt> GetUserKptList(int boardId, Kpt.KptType type, string username);

        /// <summary>
        /// アクション用のモデルを取得
        /// </summary>
        /// <returns></returns>
        ActionListModel GetActionModel(int boardId);

        /// <summary>
        /// KPT追加
        /// </summary>
        /// <param name="typeid"></param>
        /// <param name="info"></param>
        void AddItem(Kpt.KptType type, Kpt info);

        /// <summary>
        /// KPT変更
        /// </summary>
        /// <param name="typeid"></param>
        /// <param name="info"></param>
        void UpdateItem(Kpt.KptType type, Kpt info);

        /// <summary>
        /// KPT変更
        /// </summary>
        /// <param name="typeid"></param>
        /// <param name="info"></param>
        void DeleteItem(Kpt.KptType type, Kpt info);
    }
}