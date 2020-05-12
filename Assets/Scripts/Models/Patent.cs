using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patent
{
    public int Id { get; set; }
    public int Subject { get; set; }
    public bool IsPublic { get; set; }
    public Patent(int id,int subject,bool isPrivate)
    {
        this.Id = id;
        this.Subject = subject;
        this.IsPublic = isPrivate;
    }
}
