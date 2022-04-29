using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models;

public class Customer 
{
    public int Id{get; set;}
    public string cName{get; set;} = "";
    public string Cpassword{get; set;} = "";
    public bool IsManager{get; set;}
}


