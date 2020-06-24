using System;
using System.ComponentModel.DataAnnotations;

namespace Source.Domain
{
    public class Player
    {
        public long Id { get; set; }
        public long TeamId { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }

        [Range(0, 100)]
        public int SkillLevel { get; set; }
        public decimal Salary { get; set; }
    }
}
