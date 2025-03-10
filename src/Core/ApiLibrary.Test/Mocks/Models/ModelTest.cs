﻿using ApiLibrary.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiLibrary.Test.Mocks.Models
{
    public class ModelTest : BaseModel<int>
    {
        [Required]
        public string String { get; set; }
        [Required]
        public int Integer { get; set; }
        public double Double { get; set; }
        public decimal Decimal { get; set; }
        public DateTime Date { get; set; }
        public int? ParentModelId { get; set; }
        [ForeignKey("ParentModelId")]
        public ParentModelTest Parent { get; set; }
    }
}
