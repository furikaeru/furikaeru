using Microsoft.WindowsAzure.Storage.Table;

namespace SakashimaSystem.Entities
{
    public class UserEntity : TableEntity
    {
        public enum UserStatusEnum
        {
            Blank, KeepDone, ProblemDone, TryDone
        }
        public UserEntity() {}

        public UserEntity(int boardId, string userName, UserStatusEnum userStatus)
        {
            PartitionKey = boardId.ToString();
            RowKey = userName;
            this.UserStatus = (int)userStatus;
        }

        public int UserStatus { get; set; }
    }
}
