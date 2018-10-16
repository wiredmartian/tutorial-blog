using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BlogWebApp.Services.Handlers
{
    public class CheckImage
    {
        public string FileName { get; set; }
        public bool IsUploaded { get; set; }
    }
    public class FileHandler
    {

        private HttpPostedFileBase formFile;

        public FileHandler(HttpPostedFileBase formFile)
        {
            this.formFile = formFile;
        }

        public CheckImage UploadImage()
        {
            if (IsImageValid())
            {
                try
                {
                    string Folder = CreateUserFilePath();
                    string path = HttpContext.Current.Server.MapPath(Folder);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string FileName = RenameFile();
                    path = Path.Combine(path, FileName);
                    formFile.SaveAs(path);
                    return new CheckImage { FileName = Folder + FileName , IsUploaded = true };
                }
                catch (Exception e)
                {
                    /* Log (e) to a txt file or something */
                    return new CheckImage { FileName = string.Empty, IsUploaded = false };
                }
            }
            return new CheckImage { FileName = string.Empty, IsUploaded = false };
        }
        private string CreateUserFilePath()
        {
            string user = HttpContext.Current.User.Identity.Name; // email
            string Folder = user.Substring(0, user.IndexOf('@'));
            return "/Content/Images/Blog/" + Folder + "/";
        }

        private bool IsImageValid()
        {
            if (formFile != null)
            {
                return (IsExtensionValid() && IsValidSize() && IsMimeType());
            }
            return false;
        }

        private bool IsExtensionValid()
        {
            string[] exts = { ".jpg", ".jpeg", ".png", ".gif" };
            bool isEXTValid = false;
            string ext = Path.GetExtension(Path.GetFileName(formFile.FileName));
            foreach (string e in exts)
            {
                isEXTValid |= ext.Equals(e);
                if (isEXTValid)
                    break;
                continue;
            }
            return isEXTValid;
        }

        private bool IsMimeType()
        {
            string[] mimetypes = { "image/jpeg", "image/jpeg", "image/png", "image/gif" };
            bool isMimeValid = false;
            foreach (string mime in mimetypes)
            {
                isMimeValid |= formFile.ContentType.Equals(mime);
            }
            return isMimeValid;
        }

        private bool IsValidSize()
        {
            return (formFile.ContentLength > 0 && formFile.ContentLength <= 3145728);
        }

        private string RenameFile()
        {
            return Guid.NewGuid() + Path.GetExtension(Path.GetFileName(formFile.FileName));
        }
    }
    
}