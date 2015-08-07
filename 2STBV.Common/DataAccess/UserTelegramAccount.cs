using System;
using System.ComponentModel.DataAnnotations;

namespace _2STBV.Common.DataAccess
{
    public class UserTelegramAccount
    {
        [Key]
        public string UserId { get; set; }

        public int TelegramUserId { get; set; }

        public string VerificationToken { get; set; }

        public string VerificationCode { get; set; }

        public bool Verified { get; set; }

        public DateTime? VerificationCodeExpiration { get; set; }
    }
}
