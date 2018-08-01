using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web365Base;
using Web365Domain;

namespace Web365Business.Front_End.IRepository
{
    public interface IFileRepositoryFE
    {
        List<FileItem> GetListByTypeId(out int total, int type, int page, int pagesize);
        FileItem GetFileById(int id);
        List<FileItem> GetOtherFile(int type, int current, int top);
    }
}
