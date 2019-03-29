using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SakashimaSystem.Models.Kpt
{
    /// <summary>
    /// KPTリスト モデル
    /// </summary>
    public class KptListModel
    {
        /// <summary>
        /// ボードID
        /// </summary>
        public int BoardId { get; set; }

        /// <summary>
        /// タイトル
        /// </summary>
        [Display(Name = "タイトル")]
        [Required(ErrorMessage = "タイトルを入力してください。")]
        [MaxLength(50, ErrorMessage = "50文字以内で入力してください。")]
        public string Title { get; set; }

        /// <summary>
        /// コメント
        /// </summary>
        [Display(Name = "コメント")]
        [Required(ErrorMessage = "コメントを入力してください。")]
        [MaxLength(400, ErrorMessage = "400文字以内で入力してください。")]
        public string Comment { get; set; }


        public string InputTitle { get; set; }
        public  Entities.Kpt.KptType InputType { get; set; }

        public Entities.Kpt.KptType InputNextType { get; set; }

        public bool IsDeleteMode { get; set; }
        public int DeleteItemIndex { get; set; }

        /// <summary>
        /// KPT情報
        /// </summary>
        public IList<SakashimaSystem.Entities.Kpt> KptList { get; set; }
    }

}