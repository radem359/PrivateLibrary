using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PrivateLibrary.Models
{
    public class BookViewModel
    {

        public int Book_Id { get; set; }
        [Display(Name="Book Name")]
        public string Book_Name { get; set; }
        [Display(Name = "ISBN Number")]
        public string ISBN_Number { get; set; }
        [Display(Name = "Book Description")]
        public string Book_Desription { get; set; }
        [Display(Name = "Username")]
        public Nullable<int> User_Id { get; set; }

        public List<string> ChoosenAuthors { get; set; }
        public List<string> ChoosenGenres { get; set; }
        public string Language { get; set; }

    }
}