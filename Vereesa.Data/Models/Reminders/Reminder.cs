using System;
using Vereesa.Data.Interfaces;

namespace Vereesa.Data.Models.Reminders
{
    public class Reminder : IEntity
    {
        public Reminder() 
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public ulong UserId { get; set; }
        public long RemindTime { get; set; }
        public string Message { get; set; }
        public ulong ChannelId { get; set; }
    }
}