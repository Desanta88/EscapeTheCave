using Godot;
using System;

public class Enemy : KinematicBody2D
{
    [Export]
     public int movementspeed=100;

     private float gravity=9.81f;

    [Export]
     public float mass=15f;
     public int direzione=-1;

    public Vector2 move;
    private AnimationTree tree;
    public AnimationNodeStateMachinePlayback FSME;

    private RayCast2D[] rayCastsDown=new RayCast2D[2];
    private RayCast2D rayCastForward;
    
    private AnimatedSprite an;
    public Global gg;
    float hp;
    int player_dir;
    public Vector2 knockback_dir=Vector2.Zero;

    private AnimatedSprite p;

    

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
        an=this.GetNode<AnimatedSprite>("AnimatedSprite");
        gg=GetNode<Global>("/root/Global");
        hp=gg.e.Health;
        p=(AnimatedSprite)GetNode<Player2>("../Player2").an;
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
        knockback_dir=new Vector2(direzione,0);
        rayCastForward.CastTo=new Vector2(20*direzione,0);
        an.Scale=v;
      }
      if(hp>gg.e.Health){
         FSME.Travel("TakeHit");
         hp=gg.e.Health;
      }
  }

  public bool shouldTurn(){
    if(rayCastForward.IsColliding()){
      return true;
    }

    if(direzione==-1)
        return !rayCastsDown[0].IsColliding();
    else if(direzione==1)
        return !rayCastsDown[1].IsColliding();

    return false;
  }
  public void Hurt(float dmg){
    gg.e.Health-=dmg;
    if(gg.e.Health<=0)
      CallDeferred("free");
  }
}
