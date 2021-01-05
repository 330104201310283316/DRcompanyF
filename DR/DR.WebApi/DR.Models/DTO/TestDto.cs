using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DR.Models
{
    /// <summary>
    /// testdto
    /// </summary>
    public class CreateUpdateTestDto
    {
        /// <summary>
        /// testname
        /// </summary>
        [Required(ErrorMessage = "此字段必填")]
        [StringLength(50, ErrorMessage = "长度限制0-50")]
        public string TestName { get; set; }
    }
}
