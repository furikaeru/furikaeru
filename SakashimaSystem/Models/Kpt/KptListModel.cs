using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SakashimaSystem.Models.Kpt
{
    /// <summary>
    /// KPT���X�g ���f��
    /// </summary>
    public class KptListModel
    {
        /// <summary>
        /// �{�[�hID
        /// </summary>
        public int BoardId { get; set; }

        /// <summary>
        /// �^�C�g��
        /// </summary>
        [Display(Name = "�^�C�g��")]
        [Required(ErrorMessage = "�^�C�g������͂��Ă��������B")]
        [MaxLength(50, ErrorMessage = "50�����ȓ��œ��͂��Ă��������B")]
        public string Title { get; set; }

        /// <summary>
        /// �R�����g
        /// </summary>
        [Display(Name = "�R�����g")]
        [Required(ErrorMessage = "�R�����g����͂��Ă��������B")]
        [MaxLength(400, ErrorMessage = "400�����ȓ��œ��͂��Ă��������B")]
        public string Comment { get; set; }


        public string InputTitle { get; set; }
        public  Entities.Kpt.KptType InputType { get; set; }

        public Entities.Kpt.KptType InputNextType { get; set; }

        public bool IsDeleteMode { get; set; }
        public int DeleteItemIndex { get; set; }

        /// <summary>
        /// KPT���
        /// </summary>
        public IList<SakashimaSystem.Entities.Kpt> KptList { get; set; }
    }

}