using Godot;
using System;

public class Global : Node
{
   public PlayerC p=new PlayerC(5,1.2F);
   public EnemyC e=new EnemyC(7);
   public UI UI;

   

   public void LoseHeart(){
        UI.LoadHearts();
   }
}
