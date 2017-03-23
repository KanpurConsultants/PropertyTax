using Models.Customize;

namespace Models.Customize.ViewModels
{
    public class DocumentAttachmentViewModel
    {
        public int DocumentAttachmentId { get; set; }
        public int DocTypeId { get; set; }
        public int DocId { get; set; }
        public string FileFolderName { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string IcoClass { get; set; }

        public void SetExtension()
        {
            if(this.FileName.Contains(Constants.ExtensionConstants.BMP) || this.FileName.Contains(Constants.ExtensionConstants.JPEG) || this.FileName.Contains(Constants.ExtensionConstants.JPG)|| this.FileName.Contains(Constants.ExtensionConstants.PNG))
            {
                this.Extension = Constants.ExtensionConstants.JPG;
                this.IcoClass = "fa fa-file-image-o fa-2x image atch";
            }
            else if(this.FileName.Contains(Constants.ExtensionConstants.XLS) || this.FileName.Contains(Constants.ExtensionConstants.XLSX))
            {
                this.Extension=Constants.ExtensionConstants.XLS;
                this.IcoClass = "fa fa-file-excel-o fa-2x excel atch";
            }
            else if(this.FileName.Contains(Constants.ExtensionConstants.DOC))
            {
                this.Extension = Constants.ExtensionConstants.DOC;
                this.IcoClass = "fa fa-file-word-o fa-2x word atch";
            }
            else if(this.FileName.Contains(Constants.ExtensionConstants.PDF))
            {
                this.Extension = Constants.ExtensionConstants.PDF;
                this.IcoClass = "fa fa-file-pdf-o fa-2x pdf atch";
            }
            else
            {
                this.Extension = Constants.ExtensionConstants.NA;
                this.IcoClass = "fa fa-file-o fa-2x";
            }
        }

    }    
}
