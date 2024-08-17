using TGB.Domain.Entities;

namespace TGB.Web.Models
{
    public class NoteVM
    {
        public NoteVM() { }
        public NoteVM(Note note)
        {
            Id = note.Id;
            Title = note.Title;
            Content = note.Content;
            Tags = TagHelpers.FlattenTags(note.Tags);

            UnsavedChanges = false;
        }

        private Guid _id;
        private string _title;
        private string _content;
        private string _tags;

        public Guid Id { get => _id; set { _id = value; UnsavedChanges = true; } }
        public string Title { get => _title; set { _title = value; UnsavedChanges = true; } }
        public string Content { get => _content; set { _content = value; UnsavedChanges = true; } }
        public string Tags { get { return _tags; } set { _tags = TagHelpers.FormatTags(value); UnsavedChanges = true; } }

        public bool UnsavedChanges = false;

        public Record ToRecord()
        {
            return new Record
            {
                Id = Id,
                Name = Title,
                Description = Content,
                Tags = TagHelpers.UnflattenTags(Tags)
            };
        }
    }
}
