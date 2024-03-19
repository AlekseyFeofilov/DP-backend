using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DP_backend.Models.DTOs.TSUAccounts
{
    public class TSUUserShortModelDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsVerified { get; set; }
        public string Avatar { get; set; }
        public string BirthDate { get; set; }
        public string VerifyDateTime { get; set; }
        public string LastOnlineDateTime { get; set; }
        public string Email { get; set; }
        public string Faculty { get; set; }
        public string Group { get; set; }
        public string Phone { get; set; }
    }
}
