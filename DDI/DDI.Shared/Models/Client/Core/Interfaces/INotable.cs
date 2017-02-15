using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace DDI.Shared.Models.Client.Core.Interfaces
{
    public interface INotable
    {
        ICollection<Note> GetAllNotesByTopic(NoteTopic topic);
        ICollection<Note> GetAllNotesByCategory(NoteCategory category);
        ICollection<Note> GetAllNotesByDate(DateTime? dateFrom, DateTime? dateTo);
        ICollection<Note> GetAllNotesByContactCode(NoteContactCode contactCode);
    }
}