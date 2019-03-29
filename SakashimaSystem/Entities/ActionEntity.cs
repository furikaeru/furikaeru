using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SakashimaSystem.Entities.Kpt;

namespace SakashimaSystem.Entities
{
    public class ActionEntity : TableEntity
    {
        public ActionEntity(){}

        public ActionEntity(int boardId, string title, string description, string assigned, string period)
        {
            PartitionKey = boardId.ToString();
            RowKey = assigned + "_" + title;
            this.Title = title;
            this.Description = description;
            this.Assigned = assigned;
            this.Period = period;
        }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Assigned { get; set; }
        public string Period { get; set; }
    }
}
