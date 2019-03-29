using System.ComponentModel.DataAnnotations;

namespace SakashimaSystem.Entities
{
    public class Kpt
    {
        /// <summary>
        /// 名前
        /// </summary>
        [Display(Name = "名前")]
        public string UserName { get; set; }
        /// <summary>
        /// タイトル
        /// </summary>
        [Display(Name = "タイトル")]
        public string Title { get; set; }
        /// <summary>
        /// コメント
        /// </summary>
        [Display(Name = "コメント")]
        public string Comment { get; set; }
        /// <summary>
        /// KPT種別
        /// </summary>
        [Display(Name = "KPT種別")]
        public KptType Type { get; internal set; }
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

        public enum KptType
        {
            Keep,
            Problem,
            Try
        }
    }
}
