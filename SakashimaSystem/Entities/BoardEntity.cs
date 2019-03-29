using Microsoft.WindowsAzure.Storage.Table;

namespace SakashimaSystem.Entities
{
    public class BoardEntity : TableEntity
    {
        public BoardEntity() { }

        public BoardEntity(int boardId, string boradName, bool isKeepDone, bool isProblemDone, bool isTryDone)
        {
            PartitionKey = "PartitionKey";
            RowKey = boardId.ToString();
            this.BoardName = boradName;
            this.IsKeepDone = isKeepDone;
            this.IsProblemDone = isProblemDone;
            this.IsTryDone = isTryDone;
        }
        public string BoardName { get; set; }
        public bool IsKeepDone { get; set; }
        public bool IsProblemDone { get; set; }
        public bool IsTryDone { get; set; }
    }
}
