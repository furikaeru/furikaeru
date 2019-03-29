using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SakashimaSystem.Entities;
using SakashimaSystem.Logic;
using SakashimaSystem.Models.Login;

namespace SakashimaSystem.Controllers
{

    public class LoginController : Controller
    {
        private const string cUserName = "SSSUserName";
        private const string cBoardName = "SSSBoardName";
        private const int cookieExtensionDay = 60;

        private readonly ITableUtils tableUtils;

        public LoginController(ITableUtils tableUtils)
        {
            this.tableUtils = tableUtils;
        }

        /*
        public IActionResult Index()
        {
            return View();
        }*/
        #region ボード作成
        /// <summary>
        /// ログイン
        /// </summary>
        /// <returns></returns>
        public ActionResult BoardIndex()
        {
            var loginModel = new LoginViewModel();

            return View(loginModel);
        }

        /// <summary>
        /// ボード作成
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateBoard(LoginViewModel loginModel)
        {
            //ボートを作成する。
            int boardId = await tableUtils.CreateBoardAsync();

            return Redirect("index/" + boardId);
        }
        #endregion

        #region ログイン
        /// <summary>
        /// ログイン
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Login/index/{boardId}")]
        public ActionResult Index(int boardId)
        {
            var loginModel = new LoginViewModel();

            // Cookie からログイン名を取得する
            var userName = HttpContext.Request.Cookies[cUserName];

            if (!string.IsNullOrEmpty(userName))
            {
                // ユーザ名を取得出来たら、入室名のところに名前をセットする
                loginModel.UserName = userName;
            }

            //ボードIDを設定する。
            loginModel.BoardId = boardId;

            try
            {
                loginModel.BoardName = tableUtils.GetBoardName(loginModel.BoardId);
            }
            catch (Exception e)
            {
                // ignored
                Console.WriteLine(e);
            }

            loginModel.OldBoardName = loginModel.BoardName;

            return View(loginModel);
        }

        /// <summary>
        /// ボード名変更
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeBoardName(LoginViewModel loginModel)
        {
            ModelState["UserName"].Errors.Clear();

            if (string.IsNullOrEmpty(loginModel.BoardName) || string.IsNullOrEmpty(loginModel.OldBoardName))
            {
                return View("Index", loginModel);
            }

            // ボード名の変更を記録
            await tableUtils.UpdateBoardNameAsync(loginModel.BoardId, loginModel.BoardName);

            return Redirect("index/" + loginModel.BoardId);
        }

        /// <summary>
        /// ボードに入室する（Entry的な名前に変えたい）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginModel)
        {
            var validate = TryValidateModel(loginModel);

            if (!validate)
            {
                return View("Index", loginModel);
            }

            // 名前をテーブルに入れる
            tableUtils.AddUserAsync(loginModel.BoardId, loginModel.UserName);

            // ユーザ名とボード名を Cookie に書き込む
            var option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(cookieExtensionDay);
            HttpContext.Response.Cookies.Append(cUserName, loginModel.UserName, option);
            HttpContext.Response.Cookies.Append(cBoardName, loginModel.BoardName, option);

            return Redirect("/Kpt/KptInput?boardId=" + loginModel.BoardId + "&type=0");
        }

        /// <summary>
        /// キープ完了
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CompleteKeep(LoginViewModel loginModel)
        {
            await tableUtils.CloseCommitKptListAsync(loginModel.BoardId, loginModel.OldBoardName, Kpt.KptType.Keep);

            return Redirect("index/" + loginModel.BoardId);
        }

        /// <summary>
        /// プロブレム完了
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CompleteProblem(LoginViewModel loginModel)
        {
            await tableUtils.CloseCommitKptListAsync(loginModel.BoardId, loginModel.OldBoardName, Kpt.KptType.Problem);

            return Redirect("index/" + loginModel.BoardId);
        }

        /// <summary>
        /// トライ完了
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CompleteTry(LoginViewModel loginModel)
        {
            await tableUtils.CloseCommitKptListAsync(loginModel.BoardId, loginModel.OldBoardName, Kpt.KptType.Try);

            return Redirect("index/" + loginModel.BoardId);
        }

        /// <summary>
        /// ボード初期化
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetBoard(LoginViewModel loginModel)
        {
            await tableUtils.InitializeKptBoardAsync(loginModel.BoardId, loginModel.OldBoardName);

            return Redirect("index/" + loginModel.BoardId);
        }


        /// <summary>
        /// アクション入力
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActionInput(LoginViewModel loginModel)
        {

            return Redirect("/Kpt/ActionInput?boardId=" + loginModel.BoardId);
        }

        #endregion
    }
}