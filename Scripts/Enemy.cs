using Godot;
using System;

public class Enemy : KinematicBody2D
{
    [Export]
     public int movementspeed;
     private float gravity=9.81f;

     private float mass=15f;
     private int direzione=-1;

    private Vector2 move;
    private AnimationTree tree;
    AnimationNodeStateMachinePlayback FSM;

    private RayCast2D[] rayCastsDown=new RayCast2D[2];
    private RayCast2D rayCastForward;

    private AnimatedSprite an;
    public Global gg;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        tree=this.GetNode<AnimationTree>("AnimationTree");
        FSM=(AnimationNodeStateMachinePlayback)tree.Get("parameters/playback");
        FSM.Start("Idle");
        move=Vector2.Zero;
        rayCastsDown[0]=this.GetNode<RayCast2D>("RayCastLeft");
        rayCastsDown[1]=this.GetNode<RayCast2D>("RayCastRight");
        rayCastForward=this.GetNode<RayCast2D>("RayCastForward");
        an=this.GetNode<AnimatedSprite>("AnimatedSprite");
        gg=GetNode<Global>("/root/Global");


    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
      Vector2 v=new Vector2(direzione*-1,1);
      move.y+=mass*gravity;
      move.x=movementspeed*direzione;
      MoveAndSlide(move);
      rayCastForward.CastTo=new Vector2(20*direzione,0);
      if(shouldTurn()){
        direzione*=-1;
        an.Scale=v;
      }
  }

  public bool shouldTurn(){
    if(rayCastForward.IsColliding())
        return true; 

    if(direzione==-1)
        return !rayCastsDown[0].IsColliding();
    else if(direzione==1)
        return !rayCastsDown[1].IsColliding();

    return false;
  }
  public void Hurt(){
    CallDeferred("free");
  }
}
