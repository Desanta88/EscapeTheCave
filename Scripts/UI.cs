using Godot;
using System;

public class UI : CanvasLayer
{
    // Called when the node enters the scene tree for the first time.
    Global h;
    TextureRect HeartsFull;
    TextureRect HeartsEmpty;
 
    public override void _Ready()
    {
       h=GetNode<Global>("/root/Global");
       HeartsFull=GetNode<TextureRect>("HeartsFull");
       HeartsEmpty=GetNode<TextureRect>("HeartsEmpty");
       h.UI=this;
       LoadHearts();
       
    }

    public void LoadHearts(){
        GD.Print(h.p.Health);
        HeartsFull.RectSize=new Vector2(h.p.Health*17,0);
        if(h.p.Health<=0)
            HeartsFull.Visible=false;
    }
}
