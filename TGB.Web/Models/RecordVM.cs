using TGB.Domain.Entities;

namespace TGB.Web.Models
{
    public class RecordVM
    {
        public RecordVM() { }
        public RecordVM(Record record)
        {
            Id = record.Id;
            Name = record.Name;
            Amount = record.Amount;
            Description = record.Description;
            Tags = TagHelpers.FlattenTags(record.Tags);

            UnsavedChanges = false;
        }

        private Guid _id;
        private string _name;
        private double _amount = 1;
        private string _description;
        private string _tags;

        public Guid Id { get => _id; set { _id = value; UnsavedChanges = true; } }
        public string Name { get => _name; set { _name = value; UnsavedChanges = true; } }
        public double Amount { get => _amount; set { _amount = value; UnsavedChanges = true; } }
        public string Description { get => _description; set { _description = value; UnsavedChanges = true; } }
        public string Tags { get { return _tags; } set { _tags = TagHelpers.FormatTags(value); UnsavedChanges = true; } }

        public bool UnsavedChanges = false;

        public Record ToRecord()
        {
            return new Record
            {
                Id = Id,
                Name = Name,
                Amount = Amount,
                Description = Description,
                Tags = TagHelpers.UnflattenTags(Tags)
            };
        }
    }
}
