using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SakashimaSystem.Models.Kpt;
using SakashimaSystem.Entities;
namespace SakashimaSystem.Logic
{
    public class KptListLogic : IKptListLogic
    {
        private readonly ITableUtils tableUtils;

        public KptListLogic(ITableUtils tableUtils)
        {
            this.tableUtils = tableUtils;
        }

        /// <summary>
        /// モデルを取得
        /// </summary>
        /// <returns></returns>
        public KptListModel GetModel(int boardId, Kpt.KptType type)
        {
            var _kptListModel = new KptListModel();
            var list = tableUtils.GetKptList(boardId, type);

            switch (type)
            {
                case Kpt.KptType.Keep:
                    _kptListModel.InputTitle = "Keep";
                    _kptListModel.InputNextType = Kpt.KptType.Problem;
                    break;
                case Kpt.KptType.Problem:
                    _kptListModel.InputTitle = "Problem";
                    _kptListModel.InputNextType = Kpt.KptType.Try;
                    break;
                case Kpt.KptType.Try:
                    _kptListModel.InputTitle = "Try";
                    _kptListModel.InputNextType = Kpt.KptType.Try;
                    break;
                default:
                    break;
            }

            // ViewModel作成
            _kptListModel.KptList = list;
            _kptListModel.InputType = type;
            return _kptListModel;
        }

        /// <summary>
        /// モデルを取得(ユーザーで絞込）
        /// </summary>
        /// <returns></returns>
        public KptListModel GetUserModel(int boardId, Kpt.KptType type, string username)
        {

            var model = this.GetModel(boardId, type);
            var userList = model.KptList.Where(x => x.UserName == username);
            model.KptList = userList.ToList();

            return model;
        }

        /// <summary>
        /// KPTリストを取得(ユーザーで絞込）
        /// </summary>
        /// <returns></returns>
        public IList<Kpt> GetUserKptList(int boardId, Kpt.KptType type, string username)
        {

            var model = this.GetModel(boardId, type);
            var userList = model.KptList.Where(x => x.UserName == username);

            return userList.ToList();
        }

        #region ACTION PLAN

        /// <summary>
        /// アクション用のモデルを取得
        /// </summary>
        /// <returns></returns>
        public ActionListModel GetActionModel(int boardId)
        {
            var _actionListModel = new ActionListModel();
            _actionListModel.ActionList = tableUtils.GetActionList(boardId);
            _actionListModel.BoardId = boardId;

            return _actionListModel;
        }
        #endregion

        #region KPT追加
        /// <summary>
        /// KPT追加
        /// </summary>
        /// <param name="typeid"></param>
        /// <param name="info"></param>
        public void AddItem(Kpt.KptType type, Kpt info)
        {
            /*
            var deploymentType = _optimDbContext.ThreadSafe(context => context.DeploymentTypes.FirstAsync(x => x.ID == typeid));

            var addInfo = Mapper.Map<Common.DatabaseConnector.Models.Pricing>(info);
            addInfo.DeploymentType = deploymentType;
            _optimDbContext.DeploymentTypes.Attach(deploymentType);

            _optimDbContext.Add(addInfo);
            _optimDbContext.ThreadSafe(context => context.SaveChangesAsync());
            */
        }
        #endregion

        #region KPT変更
        /// <summary>
        /// KPT変更
        /// </summary>
        /// <param name="typeid"></param>
        /// <param name="info"></param>
        public void UpdateItem(Kpt.KptType type, Kpt info)
        {
            /*
            var updPricing = _optimDbContext.ThreadSafe(context => context.Pricing.Include(x => x.DeploymentType)
                .FirstOrDefaultAsync(x => x.DeploymentType.ID == typeid && x.ID == info.ID));
            if (updPricing != null)
            {
                var updateInfo = Mapper.Map<Common.DatabaseConnector.Models.Pricing>(info);
                updPricing.StartDate = updateInfo.StartDate;
                updPricing.EndDate = updateInfo.EndDate;
                updPricing.MonthlyPrice = updateInfo.MonthlyPrice;
                _optimDbContext.Update(updPricing);
                _optimDbContext.ThreadSafe(context => context.SaveChangesAsync());
            }
            */
        }
        #endregion

        #region KPT削除
        /// <summary>
        /// KPT変更
        /// </summary>
        /// <param name="typeid"></param>
        /// <param name="info"></param>
        public void DeleteItem(Kpt.KptType type, Kpt info)
        {
            /*
            var delPricing = _optimDbContext.ThreadSafe(context => context.Pricing.Include(x => x.DeploymentType)
                .FirstOrDefaultAsync(x => x.DeploymentType.ID == typeid && x.ID == info.ID));
            if (delPricing != null)
            {
                // 基本単価の場合は自動で終了日を設定
                if (delPricing.PricingType == Common.DatabaseConnector.Models.PricingTypeEnum.Base)
                {
                    var pricing = _optimDbContext.ThreadSafe(context => context.Pricing.Include(x => x.DeploymentType)
                        .Where(x => x.DeploymentType.ID == typeid && x.PricingType == delPricing.PricingType)
                        .OrderByDescending(x => x.EndDate)
                        .FirstOrDefaultAsync(x => x.EndDate.HasValue));
                    if (pricing != null)
                    {
                        pricing.EndDate = null;
                        _optimDbContext.Update(pricing);
                    }
                }
                _optimDbContext.Remove(delPricing);
                _optimDbContext.ThreadSafe(context => context.SaveChangesAsync());
            }
            */
        }
        #endregion
    }
}
