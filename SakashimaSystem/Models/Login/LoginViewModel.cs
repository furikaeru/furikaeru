using System;
using System.ComponentModel.DataAnnotations;

namespace SakashimaSystem.Models.Login
{
    public class LoginViewModel
    {
        /// <summary>
        /// ボードID
        /// </summary>
        public int BoardId { get; set; }

        /// <summary>
        /// 旧ボード名
        /// </summary>
        public string OldBoardName { get; set; }

        /// <summary>
        /// ボード名
        /// </summary>
        [Display(Name = "ボード名")]
        [Required(ErrorMessage = "ボード名を入力してください。")]
        [MaxLength(50, ErrorMessage = "50文字以内で入力してください。")]
        public string BoardName { get; set; }

        /// <summary>
        /// ユーザー名
        /// </summary>
        [Display(Name = "ユーザー名")]
        [Required(ErrorMessage = "名前を入力してください。")]
        [MaxLength(50, ErrorMessage = "50文字以内で入力してください。")]
        public string UserName { get; set; }
    }
}