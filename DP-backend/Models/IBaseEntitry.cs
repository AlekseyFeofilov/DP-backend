﻿
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DP_backend.Models
{
    public class IBaseEntitry
    {
        public interface IBaseEntity
        {
            public Guid Id { get; set; }

            public DateTime CreateDateTime { get; set; }

            public DateTime ModifyDateTime { get; set; }

            public DateTime? DeleteDateTime { get; set; }
        }
        public abstract class BaseEntity : IBaseEntity
        {
            public Guid Id { get; set; }
            public DateTime CreateDateTime { get; set; }
            public DateTime ModifyDateTime { get; set; }
            public DateTime? DeleteDateTime { get; set; }
        }
    }
}
