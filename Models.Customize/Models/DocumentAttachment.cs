using Model;
using Models.Company.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Customize.Models
{
    public class DocumentAttachment : EntityBase, IHistoryLog
    {
        [Key]
        public int DocumentAttachmentId { get; set; }
        [Display(Name="Document Type")]
        public int DocTypeId { get; set; }
        [Display(Name="Document")]
        public int DocId { get; set; }
        public string FileFolderName { get; set; }
        public string FileName { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }
    }
}
