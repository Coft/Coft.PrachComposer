using Coft.PreachComposer.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coft.PreachComposer.Models.Messages
{
    public class ShowFileDialogResult
    {
        public Enums.FileType FileType { set; get; }
        public bool IsFolder { get; set; }
        public string Filename { get; set; }
        public string SafeFilename { get; set; }
    }
}
