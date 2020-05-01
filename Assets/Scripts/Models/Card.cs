using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public int Id { get; set; }
    public int Subject { get; set; }
    public int Type { get; set; }
    public Card(int id,int subject,int type)
    {
        this.Id = id;
        this.Subject = subject;
        this.Type = type;
    }


}
