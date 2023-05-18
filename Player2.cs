using Godot;
using System;

public class Player2 : KinematicBody2D
{
    // Called when the node enters the scene tree for the first time.

    [Export]
     public int movementspeed=300;

     private AnimatedSprite an;
     private AnimationTree tree;
     Vector2 v;
     string currentState;
     AnimationNodeStateMachinePlayback FSM;
    public override void _Ready()
    {
        an=this.GetNode<AnimatedSprite>("AnimatedSprite");
        v=new Vector2(-1,1);
        tree=this.GetNode<AnimationTree>("AnimationTree");
        FSM = (AnimationNodeStateMachinePlayback)tree.Get("parameters/playback");
        FSM.Start("Idle");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
        if(Input.IsKeyPressed((int)KeyList.D)){

            this.Position+=Vector2.Right*movementspeed*delta;
            an.Scale=Vector2.One;
            FSM.Travel("Run");
        }
        else if(Input.IsKeyPressed((int)KeyList.A)){
            this.Position+=Vector2.Left*movementspeed*delta;
            an.Scale=v;
            FSM.Travel("Run");
        }
        else
            FSM.Travel("Idle"); 

        if(Input.IsKeyPressed((int)KeyList.Space)){
            FSM.Travel("Jump");
            this.Position+=Vector2.Up*100*delta;
        }   

  }
    public override void _UnhandledInput(InputEvent @event)
    {
        if(@event is InputEventMouseButton Click){
            FSM.Travel("Attack");
        }
    }
    public void checkJumping(float delta){
        
    }

}

