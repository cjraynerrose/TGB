﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TGB.Domain.Entities
{
    public class GroupBank
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public virtual List<Record> Records { get; set; } = [];

        public virtual List<Note> Notes { get; set; } = [];
    }
}
