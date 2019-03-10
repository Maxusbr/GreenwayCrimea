namespace AdvantShop.Web.Admin.Models.TaskGroups
{
    public partial class TaskGroupModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public int TasksCount { get; set; }

        public bool CanBeDeleted
        {
            get { return TasksCount == 0; }
        }
    }
}
