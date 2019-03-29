using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SakashimaSystem.Entities.Kpt;

namespace SakashimaSystem.Entities
{
    public class KptEntity : TableEntity
    {
        public KptEntity(){}

        public KptEntity(int boardId, KptType type, string userName, string title, string comment)
        {
            PartitionKey = string.Format("{0}_{1}", boardId.ToString(),Enum.GetName(typeof(KptType), type));
            RowKey = userName + "_" + title;
            this.UserName = userName;
            this.Title = title;
            this.Comment = comment;
        }
        public string UserName { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
    }
}
