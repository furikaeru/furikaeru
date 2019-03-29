using System.ComponentModel.DataAnnotations;

namespace SakashimaSystem.Entities
{
    public class ActionItem
    {
        /// <summary>
        /// アクションタイトル
        /// </summary>
        [Display(Name = "アクションタイトル")]
        public string Title { get; set; }
        /// <summary>
        /// アクション詳細
        /// </summary>
        [Display(Name = "アクション詳細")]
        public string Description { get; set; }
        /// <summary>
        /// 担当者
        /// </summary>
        [Display(Name = "担当者")]
        public string Assigned { get; set; }
        /// <summary>
        /// 実施時期
        /// </summary>
        [Display(Name = "実施時期")]
        public string Period { get; set; }

        /// <summary>
        /// 編集中
        /// </summary>
        public bool IsEditing { get; set; }
        /// <summary>
        /// 適用中
        /// </summary>
        public bool IsCurrent { get; set; }
        /// <summary>
        /// 編集可
        /// </summary>
        public bool IsEditable { get; set; }

    }
}
