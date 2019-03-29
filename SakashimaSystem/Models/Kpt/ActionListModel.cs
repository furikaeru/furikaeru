using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SakashimaSystem.Models.Kpt
{
    /// <summary>
    /// KPT���X�g ���f��
    /// </summary>
    public class ActionListModel
    {
        /// <summary>
        /// �{�[�hID
        /// </summary>
        public int BoardId { get; set; }

        /// <summary>
        /// �A�N�V������
        /// </summary>
        [Display(Name = "�A�N�V�����^�C�g��")]
        [Required(ErrorMessage = "�A�N�V�����^�C�g������͂��Ă��������B")]
        [MaxLength(50, ErrorMessage = "50�����ȓ��œ��͂��Ă��������B")]
        public string Title { get; set; }

        /// <summary>
        /// �A�N�V�����ڍ�
        /// </summary>
        [Display(Name = "�A�N�V�����ڍ�")]
        [Required(ErrorMessage = "�A�N�V�����ڍׂ���͂��Ă��������B")]
        [MaxLength(400, ErrorMessage = "400�����ȓ��œ��͂��Ă��������B")]
        public string Description { get; set; }

        /// <summary>
        /// �S����
        /// </summary>
        [Display(Name = "�S����")]
        [Required(ErrorMessage = "�S���҂���͂��Ă��������B")]
        [MaxLength(50, ErrorMessage = "50�����ȓ��œ��͂��Ă��������B")]
        public string Assigned { get; set; }

        /// <summary>
        /// ���{����
        /// </summary>
        [Display(Name = "���{����")]
        [Required(ErrorMessage = "���{��������͂��Ă��������B")]
        [MaxLength(50, ErrorMessage = "50�����ȓ��œ��͂��Ă��������B")]
        public string Period { get; set; }

        
        /// <summary>
        /// KPT���
        /// </summary>
        public IList<SakashimaSystem.Entities.ActionItem> ActionList { get; set; }
    }

}