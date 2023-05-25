using Godot;
using System;

public class Enemy : KinematicBody2D
{
    [Export]
     public int movementspeed=100;
     private float gravity=9.81f;

     private float mass=15f;
     private int direzione=-1;

    public Vector2 move;
    private AnimationTree tree;
    public AnimationNodeStateMachinePlayback FSME;

    private RayCast2D[] rayCastsDown=new RayCast2D[2];
    private RayCast2D rayCastForward;
    
    private AnimatedSprite an;
    public Global gg;

    public RayCast2D rayCastDistance;
    public Timer T=new Timer();
    

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        tree=this.GetNode<AnimationTree>("AnimationTree");
        FSME=(AnimationNodeStateMachinePlayback)tree.Get("parameters/playback");
        FSME.Start("Idle");
        move=Vector2.Zero;
        rayCastsDown[0]=this.GetNode<RayCast2D>("RayCastLeft");
        rayCastsDown[1]=this.GetNode<RayCast2D>("RayCastRight");
        rayCastForward=this.GetNode<RayCast2D>("RayCastForward");
        rayCastDistance=this.GetNode<RayCast2D>("RayCastDistance");
        an=this.GetNode<AnimatedSprite>("AnimatedSprite");
        gg=GetNode<Global>("/root/Global");
        AddChild(T);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
      Vector2 v=new Vector2(direzione*-1,1);
      move.y+=mass*gravity;
      move.x=movementspeed*direzione;
      MoveAndSlide(move);
      FSME.Travel("Run");
      rayCastForward.CastTo=new Vector2(20*direzione,0);
      if(shouldTurn()){
        direzione*=-1;
        rayCastForward.CastTo=new Vector2(20*direzione,0);
        an.Scale=v;
      }
      StopAnim();
  }

  public bool shouldTurn(){
    if(rayCastForward.IsColliding()){
      GD.Print("turn");
      return true;
    }
        //return true; 

    if(direzione==-1)
        return !rayCastsDown[0].IsColliding();
    else if(direzione==1)
        return !rayCastsDown[1].IsColliding();

    return false;
  }
  public void Hurt(){
    CallDeferred("free");
  }
  private void StopAnim(){
        Timer T=new Timer();
        T.WaitTime=1;
        AddChild(T);
        Timer[] arr=new Timer[1]{T};
        T.Connect("timeout",this,"onAttackWaitTimetimeout");
        if(movementspeed==0){
            T.Start();
            FSME.Travel("Idle");
            FSME.Travel("Attack");
           // waitTime.Connect("timeout",this,"onAttackWaitTimetimeout");
        }
  }
  private void onAttackWaitTimetimeout(){
      FSME.Travel("Idle");
      GD.Print("yay");
      movementspeed=100;
      //GD.Print(movementspeed);
     // waitTime.Paused=true;
      T.Stop();
      T=null;
      

  }
}
