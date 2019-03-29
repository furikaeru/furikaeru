using System;
using System.ComponentModel.DataAnnotations;

namespace SakashimaSystem.Models.Login
{
    public class LoginViewModel
    {
        /// <summary>
        /// �{�[�hID
        /// </summary>
        public int BoardId { get; set; }

        /// <summary>
        /// ���{�[�h��
        /// </summary>
        public string OldBoardName { get; set; }

        /// <summary>
        /// �{�[�h��
        /// </summary>
        [Display(Name = "�{�[�h��")]
        [Required(ErrorMessage = "�{�[�h������͂��Ă��������B")]
        [MaxLength(50, ErrorMessage = "50�����ȓ��œ��͂��Ă��������B")]
        public string BoardName { get; set; }

        /// <summary>
        /// ���[�U�[��
        /// </summary>
        [Display(Name = "���[�U�[��")]
        [Required(ErrorMessage = "���O����͂��Ă��������B")]
        [MaxLength(50, ErrorMessage = "50�����ȓ��œ��͂��Ă��������B")]
        public string UserName { get; set; }
    }
}