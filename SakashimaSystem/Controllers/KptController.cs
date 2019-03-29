using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SakashimaSystem.Entities;
using SakashimaSystem.Models.Kpt;
using SakashimaSystem.Logic;
using static SakashimaSystem.Entities.Kpt;
using SakashimaSystem.Settings;
using System.Collections.Generic;
using System.Linq;


namespace SakashimaSystem.Controllers
{
    public class KptController : Controller
    {
        private const string cUserName = "SSSUserName";
        private const string cBoardName = "SSSBoardName";

        private readonly ITableUtils tableUtils;
        private readonly IKptListLogic kptListLogic;

        public KptController(ITableUtils tableUtils, IKptListLogic kptListLogic)
        {
            this.tableUtils = tableUtils;
            this.kptListLogic = kptListLogic;
        }

        /*
        public IActionResult Index()
        {
            return View();
        }
        */
        /// <summary>
        /// 表示
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int boardId, Kpt.KptType type)
        {
            Console.WriteLine(HttpContext.Request.Cookies[cBoardName] + ":" + HttpContext.Request.Cookies[cUserName]);

            //セッションから取得する。
            var username = HttpContext.Request.Cookies[cUserName];

            var model = kptListLogic.GetModel(boardId, type);
            model.BoardId = boardId;

            return View(model);
        }

        /// <summary>
        /// KPT入力
        /// </summary>
        /// <returns></returns>
        public ActionResult KptInput(int boardId, Kpt.KptType type)
        {
            var model = new KptListModel();

            //セッションから取得する。
            var username = HttpContext.Request.Cookies[cUserName];

            model = kptListLogic.GetUserModel(boardId, type, username);
            model.BoardId = boardId;

            return View(model);
        }
        /// <summary>
        /// KPTアイテムのリスト登録
        /// </summary>
        /// <param name="id"></param>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> KptInput(KptListModel inputModel, IList<Kpt> kpt, string inputitem, string commititem)
        {

            var list = kpt;

            if (list == null || list.Count == 0)
            {
                list = new List<Kpt>();
            }

            //セッションから取得する。
            var username = HttpContext.Request.Cookies[cUserName];

            if (inputitem != null)
            {

                Kpt newkpt = new Kpt()
                {
                    Type = inputModel.InputType,
                    UserName = username,
                    Title = inputModel.Title,
                    Comment = inputModel.Comment
                };
                list.Add(newkpt);

                //リスト置き換え
                ModelState.Clear();
                inputModel.Title = string.Empty;
                inputModel.Comment = string.Empty;
                inputModel.KptList = list;
                inputModel.IsDeleteMode = false;
                inputModel.DeleteItemIndex = -1;

                return View(inputModel);

            }

            //リストから項目を削除する。
            if (inputModel.IsDeleteMode)
            {
                Kpt delItem = list[inputModel.DeleteItemIndex];

                //テーブルに既に登録されている場合は、削除する。
                IList<Kpt> tlist = kptListLogic.GetUserKptList(inputModel.BoardId, inputModel.InputType, username);
                var isExist = tlist.Any(x => x.Title == delItem.Title);

                if (isExist)
                {
                    //KPTアイテムの削除
                    await tableUtils.DelKptAsync(inputModel.BoardId, inputModel.InputType, delItem);
                }

                //リストから削除
                list.RemoveAt(inputModel.DeleteItemIndex);

                //リスト置き換え
                ModelState.Clear();
                inputModel.Title = string.Empty;
                inputModel.Comment = string.Empty;
                inputModel.KptList = list;
                inputModel.IsDeleteMode = false;
                inputModel.DeleteItemIndex = -1;

                return View(inputModel);
            }


            //リストの登録
            await tableUtils.CommitKptListByUserAsync(inputModel.BoardId, username, inputModel.InputType, list.ToList());

            if (inputModel.InputType == KptType.Try)
            {
                return Redirect("/Kpt/Index?boardId=" + inputModel.BoardId + "&type=" + inputModel.InputType);
            }
            return Redirect("/Kpt/KptInput?boardId=" + inputModel.BoardId + "&type=" + (inputModel.InputType + 1));

        }


        /// <summary>
        /// KPT(登録)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="typeid"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpsertItem(Kpt.KptType type, Kpt info = null)
        {
            //(int id = 1, string typeid = null, Kpt info = null)
            var hasError = false;

            /*
            if (!ViewData.ModelState.IsValid)
            {
                hasError = true;
            }

            if (hasError)
            {
                var validatedModel = _basePriceLogic.GetModel(id, typeid, info);
                ViewBag.ErrorTypeId = typeid;
                ViewBag.ErrorPricingType = info.PricingType;
                return View("Index", validatedModel);
            }
            */
            /*
            if (info.ID == 0)
            {
                _kptListLogic.AddPricing(typeid, info);
            }
            else
            {
                _kptListLogic.UpdatePricing(typeid, info);
            }
            */
            kptListLogic.AddItem(type, info);
            //var model = _kptListLogic.GetModel(type);
            return View("Index");
        }
        /// <summary>
        /// KPT(削除)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="typeid"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteItem(Kpt.KptType type, Kpt info = null)
        {
            /*
                var hasError = false;

                if (!_basePriceLogic.CheckValidPeriod(typeid, info, false))
                {
                    hasError = true;
                    ViewData.ModelState.AddModelError(nameof(info.StartDate), string.Format(Constants.Messages.ERROR_CHECK_VALID_PERIOD, "削除"));
                }

                if (hasError)
                {
                    var validatedModel = _basePriceLogic.GetModel(id, typeid, info);
                    ViewBag.ErrorTypeId = typeid;
                    ViewBag.ErrorPricingType = info.PricingType;
                    return View("Index", validatedModel);
                }
                */
            kptListLogic.DeleteItem(type, info);
            //var model = _kptListLogic.GetModel(type);
            return View("Index");
        }

        /// <summary>
        /// ACTION入力
        /// </summary>
        /// <returns></returns>
        //public ActionResult ActionInput(int boardId, Kpt.KptType type)
        public ActionResult ActionInput(int boardId)
        {
            var model = new ActionListModel();


            model = kptListLogic.GetActionModel(boardId);
            model.BoardId = boardId;

            return View(model);
        }

        /// <summary>
        /// KPTアイテムのリスト登録
        /// </summary>
        /// <param name="id"></param>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> ActionInput(ActionListModel inputModel, IList<ActionItem> actionItem, string inputitem, string commititem)
        {

            var list = actionItem;

            if (list == null || list.Count == 0)
            {
                list = new List<ActionItem>();
            }

            if (inputitem != null)
            {
                ActionItem actItem = new ActionItem()
                {
                    Title = inputModel.Title,
                    Description = inputModel.Description,
                    Assigned = inputModel.Assigned,
                    Period = inputModel.Period
                };
                list.Add(actItem);

                //リスト置き換え
                ModelState.Clear();
                inputModel.Title = string.Empty;
                inputModel.Description = string.Empty;
                inputModel.Assigned = string.Empty;
                inputModel.Period = string.Empty;
                inputModel.ActionList = list;

                return View(inputModel);

            }
            else
            {
                //リストの登録
                await tableUtils.AddActionListAsync(inputModel.BoardId, list.ToList());

                return Redirect("/Kpt/actionlist/" + inputModel.BoardId);
            }

        }

        [HttpGet("kpt/list/{boardId}/{type}")]
        public ActionResult KptResultList(int boardId, string type)
        {
            ViewBag.Title = string.Format("{0} 一覧画面", type.ToUpper());
            var model = new KptListModel();
            var kptType = (KptType)Enum.Parse(typeof(KptType), type, true);
            model.KptList = tableUtils.GetKptList(boardId, kptType);
            model.BoardId = boardId;
            return View("Index", model);
        }

        [HttpGet("kpt/actionlist/{boardId}")]
        public ActionResult ActionResultList(int boardId)
        {
            ViewBag.Title = string.Format("アクションプラン 一覧画面");

            var model = kptListLogic.GetActionModel(boardId);

            return View(model);
        }
    }
}