using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SakashimaSystem.Models.Kpt
{
    /// <summary>
    /// KPTリスト モデル
    /// </summary>
    public class ActionListModel
    {
        /// <summary>
        /// ボードID
        /// </summary>
        public int BoardId { get; set; }

        /// <summary>
        /// アクション名
        /// </summary>
        [Display(Name = "アクションタイトル")]
        [Required(ErrorMessage = "アクションタイトルを入力してください。")]
        [MaxLength(50, ErrorMessage = "50文字以内で入力してください。")]
        public string Title { get; set; }

        /// <summary>
        /// アクション詳細
        /// </summary>
        [Display(Name = "アクション詳細")]
        [Required(ErrorMessage = "アクション詳細を入力してください。")]
        [MaxLength(400, ErrorMessage = "400文字以内で入力してください。")]
        public string Description { get; set; }

        /// <summary>
        /// 担当者
        /// </summary>
        [Display(Name = "担当者")]
        [Required(ErrorMessage = "担当者を入力してください。")]
        [MaxLength(50, ErrorMessage = "50文字以内で入力してください。")]
        public string Assigned { get; set; }

        /// <summary>
        /// 実施時期
        /// </summary>
        [Display(Name = "実施時期")]
        [Required(ErrorMessage = "実施時期を入力してください。")]
        [MaxLength(50, ErrorMessage = "50文字以内で入力してください。")]
        public string Period { get; set; }

        
        /// <summary>
        /// KPT情報
        /// </summary>
        public IList<SakashimaSystem.Entities.ActionItem> ActionList { get; set; }
    }

}